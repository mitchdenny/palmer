using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palmer
{
    public class RetryCondition
    {
        public Retry Retry { get; set; }

        public RetryCondition(Retry retry, Func<RetryContext, bool> predicate)
        {
            Retry = retry;
            FilterCondition = predicate;
        }

        public Retry For(uint times)
        {
            return null;
        }

        public Retry For(TimeSpan duration)
        {
            return null;
        }

        public Retry Until(Func<RetryContext, bool> predicate)
        {
            TerminationCondition = predicate;
            return Retry;
        }

        public Retry Until<TException>(Func<RetryContext, bool> predicate)
        {
            return null;
        }

        public Retry Indefinately()
        {
            TerminationCondition = (context) => false;
            return Retry;
        }

        public Func<RetryContext, bool> FilterCondition { get; private set; }
        public Func<RetryContext, bool> TerminationCondition { get; private set; }
    }
}
