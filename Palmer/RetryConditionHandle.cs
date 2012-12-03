using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Palmer
{
    public class RetryConditionHandle
    {
        private static Random m_Generator = new Random();

        public RetryConditionHandle(RetryContext context, RetryCondition condition)
        {
            Context = context;
            Condition = condition;
        }

        public DateTimeOffset FirstOccured { get; internal set; }
        public uint Occurences { get; internal set; }
        
        public RetryContext Context { get; private set; }
        public RetryCondition Condition { get; private set; }
    }
}
