using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Palmer
{
    public class Retry
    {
        public Retry()
        {
            Conditions = new Collection<RetryCondition>();
        }

        public static RetryCondition On(Func<RetryConditionHandle, bool> predicate)
        {
            var retry = new Retry();
            return new RetryCondition(retry, predicate);
        }

        private static RetryCondition OnInternal<TException>(Retry retry) where TException : Exception
        {
            return OnInternal<TException>(retry, (handle) => true);
        }

        private static RetryCondition OnInternal<TException>(Retry retry, Func<RetryConditionHandle, bool> predicate) where TException : Exception
        {
            Func<RetryConditionHandle, bool> typeCheckingPredicate = (handle) => handle.Context.DidExceptionOnLastRun && handle.Context.LastException is TException && predicate(handle);
            return new RetryCondition(retry, typeCheckingPredicate);
        }

        public static RetryCondition On<TException>() where TException : Exception
        {
            var retry = new Retry();
            return OnInternal<TException>(retry);
        }

        public static RetryCondition On<TException>(Func<RetryConditionHandle, bool> predicate) where TException : Exception
        {
            var retry = new Retry();
            return OnInternal<TException>(retry, predicate);
        }

        public RetryCondition AndOn<TException>() where TException : Exception
        {
            return OnInternal<TException>(this);
        }

        public RetryCondition AndOn<TException>(Func<RetryConditionHandle, bool> predicate) where TException : Exception
        {
            return OnInternal<TException>(this, predicate);
        }

        public RetryResult<TOutput> With<TOutput>(Func<RetryContext, TOutput> target)
        {
            TOutput output = default(TOutput);
            Action<RetryContext> capturedTarget = (context) => output = target(context);
            var result = With(capturedTarget);
            var resultWithValue = result.WithValue(output);
            return resultWithValue;
        }

        public RetryResult With(Action<RetryContext> target)
        {
            var context = new RetryContext(this);

            do
            {
                try
                {
                    target(context);
                    context.DidExceptionOnLastRun = false;
                    var anyConditionsMet = CheckAndUpdateFilterConditions(context);

                    if (!anyConditionsMet)
                    {
                        return new RetryResult(context);
                    }
                }
                catch (Exception ex)
                {
                    context.DidExceptionOnLastRun = true;
                    context.Exceptions.Push(ex);

                    var anyConditionsMet = CheckAndUpdateFilterConditions(context);
                    if (!anyConditionsMet)
                    {
                        throw ex;
                    }
                }
            } while (context.KeepRetrying);

            throw new RetryException(context);
        }

        private bool CheckAndUpdateFilterConditions(RetryContext context)
        {
            var handlesForFilterConditionsMet = context.FilteredConditionHandles.Where(handle => handle.Condition.FilterCondition(handle));

            if (handlesForFilterConditionsMet.Count() > 0)
            {
                foreach (var handle in handlesForFilterConditionsMet)
                {
                    handle.Occurences = handle.Occurences + 1;

                    if (handle.Occurences == 1)
                    {
                        handle.FirstOccured = DateTimeOffset.Now;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public Collection<RetryCondition> Conditions { get; private set; }
    }
}
