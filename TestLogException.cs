using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elmah
{
    public class TestLogException : Exception
    {
        /// <summary>
        /// Log index (base 0).
        /// </summary>
        public int Index { get; private set; }

        public TestLogException(int index) : base(string.Format("Exception logged into log #{0} at {1:HH:mm:ss}", index, DateTime.Now))
        {
            Index = index;
        }

        public TestLogException(int index, string message) : base(message)
        {

        }

        public TestLogException(int index, string message, params object[] args) : base(string.Format(message, args))
        {
            Index = index;
        }
    }
}
