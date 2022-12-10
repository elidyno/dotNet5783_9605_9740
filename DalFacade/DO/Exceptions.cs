
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    public class NotFoundException: Exception
    {
        public NotFoundException() : base() { }
        public NotFoundException(string message): base(message){}
        public override string ToString()
        => $@"Dal Exception - Data not found: " + Message;

    }

    [Serializable]
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() : base() { }
        public override string ToString()
        => $@"The object already exists";

    }
    [Serializable]
    public class NullException : Exception
    {
        public NullException() : base() { }
        public override string ToString()
        => $@"Can't return a null entity";

    }
}   

