using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palmer
{
    public class RetryResult
    {
        public RetryResult(RetryContext context)
        {
            Context = context;
        }

        public RetryContext Context { get; private set; }

        public RetryResult<TResult> WithValue<TResult>(TResult value)
        {
            var result = new RetryResult<TResult>(Context, value);
            return result;
        }
    }
}
