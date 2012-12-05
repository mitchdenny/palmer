using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palmer
{
    public class RetryCondition
    {
        public Retry Retry { get; set; }

        public RetryCondition(Retry retry, Func<RetryConditionHandle, bool> predicate)
        {
            Retry = retry;
            Retry.Conditions.Add(this);
            FilterCondition = predicate;
        }

        public Retry For(uint times)
        {
            TerminationCondition = (handle) => handle.Occurences > times;
            return Retry;
        }

        public Retry For(TimeSpan duration)
        {
            TerminationCondition = (handle) =>
            {
                var durationSinceFirstOccured = DateTimeOffset.Now - handle.FirstOccured;
                var result = durationSinceFirstOccured > duration;
                return result;
            };

            return Retry;
        }

        public Retry Until(Func<RetryConditionHandle, bool> predicate)
        {
            TerminationCondition = predicate;
            return Retry;
        }

        public Retry Indefinately()
        {
            TerminationCondition = (handle) => false;
            return Retry;
        }

        public Func<RetryConditionHandle, bool> FilterCondition { get; private set; }
        public Func<RetryConditionHandle, bool> TerminationCondition { get; private set; }
    }
}
