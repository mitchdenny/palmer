using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palmer
{
    public class Retry
    {
        public static RetryCondition On(Func<RetryContext, bool> predicate)
        {
            var retry = new Retry();
            return new RetryCondition(retry, predicate);
        }

        public static RetryCondition On<TException>() where TException : Exception
        {
            var retry = new Retry();
            return new RetryCondition(retry, (context) => true);
        }

        private static bool EvaluateException<TException>(RetryContext context, Func<RetryContext, bool> predicate) where TException: Exception
        {
            return context.LastException.GetType().IsSubclassOf(typeof(TException)) && predicate(context);
        }

        public static RetryCondition On<TException>(Func<RetryContext, bool> predicate = null) where TException: Exception
        {
            var retry = new Retry();
            return new RetryCondition(retry, context => EvaluateException<TException>(context, predicate));
        }

        public RetryResult<TResult> With<TResult>(Func<RetryContext, TResult> target)
        {
            return null;
        }

        public RetryResult With(Action<RetryContext> target)
        {
            return null;
        }
    }
}
