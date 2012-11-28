using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Palmer
{
    public class RetryContext
    {
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

        public Exception LastException
        {
            get
            {
                return Exceptions.Peek();
            }
        }
    }
}
