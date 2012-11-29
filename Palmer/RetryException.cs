using Palmer.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palmer
{
    public class RetryException : Exception
    {
        public RetryException(RetryContext context)
            : base(string.Format(Resources.ExceptionMessage, context.Exceptions.Count, context.LastException.Message), context.LastException)
        {
            Context = context;
        }

        public RetryContext Context { get; private set; }
    }
}
