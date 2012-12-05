using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Palmer
{
    public class RetryContext
    {
        public RetryContext(Retry retry)
        {
            Retry = retry;
            ConditionHandles = retry.Conditions.Select(condition => new RetryConditionHandle(this, condition)).ToList();
        }

        public IEnumerable<RetryConditionHandle> ConditionHandles { get; private set; }

        public IEnumerable<RetryConditionHandle> FilteredConditionHandles
        {
            get
            {
                return ConditionHandles.Where(handle => handle.Condition.FilterCondition(handle));
            }
        }

        public IEnumerable<RetryConditionHandle> TerminatingConditionHandles
        {
            get
            {
                return FilteredConditionHandles.Where(handle => handle.Condition.TerminationCondition(handle));
            }
        }

        public Retry Retry { get; private set; }

        private Stack<Exception> m_Exceptions = null;

        public Stack<Exception> Exceptions
        {
            get
            {
                if (m_Exceptions == null)
                {
                    m_Exceptions = new Stack<Exception>();
                }

                return m_Exceptions;
            }
        }

        public bool DidExceptionOnLastRun { get; set; }

        public Exception LastException
        {
            get
            {
                return Exceptions.Peek();
            }
        }

        public bool KeepRetrying
        {
            get
            {
                var handles = TerminatingConditionHandles.ToList();
                var any = handles.Any(handle => handle.Condition.TerminationCondition(handle));
                return any != true;
            }
        }

        public DateTimeOffset Started { get; private set; }
    }
}
