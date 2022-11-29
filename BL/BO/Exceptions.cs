

namespace BO
{
    [Serializable]
    public class DataRequestFailedException: Exception
    {
        public DataRequestFailedException(): base(){}
        public DataRequestFailedException(string message) : base(message) { }
        public DataRequestFailedException(string message, Exception inner): base(message, inner) { }
        public override string ToString()
        {
            return "Data Request Failed Exception:" + InnerException;
        }
    }

    [Serializable]
    public class InvalidValueException : Exception
    {
        public InvalidValueException() : base() { }
        public InvalidValueException(string message) : base(message) { }
        public override string ToString()
        {
            return "invalid input value: " + Message;
        }
    }
}
