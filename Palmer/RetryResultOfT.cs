using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Palmer
{
    public class RetryResult<TResult> : RetryResult
    {
        public TResult Value { get; private set; }
    }
}
