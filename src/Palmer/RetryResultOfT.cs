using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palmer
{
    public class RetryResult<TResult> : RetryResult
    {
        public RetryResult(RetryContext context, TResult value) : base(context)
        {
            Value = value;
        }

        public TResult Value { get; private set; }
    }
}
