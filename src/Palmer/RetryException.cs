using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palmer
{
    public class RetryException : Exception
    {
        private const string ExceptionMessage = "An error occured performing an operation. The operation we retried '{0}' times and failed, the last exception message was '{1}'. Check the inner exception for details.";

        public RetryException(RetryContext context)
            : base(string.Format(ExceptionMessage, context.Exceptions.Count, context.LastException.Message), context.LastException)
        {
            Context = context;
        }

        public RetryContext Context { get; private set; }
    }
}
