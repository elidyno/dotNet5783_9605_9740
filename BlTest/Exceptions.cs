using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlTest
{
    [Serializable]
    public class InvalidInputFormatException : Exception
    {
        public InvalidInputFormatException() : base() { }
        public InvalidInputFormatException(string message) : base(message) { }
        public override string ToString()
        {
            return "Invalid input format: " + Message;
        }
    }
}
