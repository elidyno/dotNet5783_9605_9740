﻿namespace BO
{
    [Serializable]
    public class DataRequestFailedException : Exception
    {
        [NonSerialized] bool message_ = false;
        public DataRequestFailedException() : base() { }
        public DataRequestFailedException(string message) : base(message) { message_ = true; }
        public DataRequestFailedException(string message, Exception inner) : base(message, inner) { }
        public override string ToString()
        {
            string generalMess = "Data Request Failed Exception:";
            string toString = message_ ? (Message + " " + generalMess + " " + InnerException)
                : (generalMess + " " + InnerException);
            return toString;
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

    [Serializable]
    public class InvalidEmailFormatException : Exception
    {
        public InvalidEmailFormatException() : base("invalid Email format:") { }
        public override string ToString()
        {
            return @"
invalid Email forma:
The format must be: Example@domain.suffix";
        }
    }

    [Serializable]
    public class AmountAndPriceException : Exception
    {
        public AmountAndPriceException() : base() { }
        public AmountAndPriceException(string message) : base(message) { }
        public override string ToString()
        {
            return "Error - Amount / Price: " + Message;
        }
    }
    [Serializable]
    public class CantBeDeletedException : Exception
    {
        public CantBeDeletedException() : base() { }
        public CantBeDeletedException(string message) : base(message) { }
        public override string ToString()
        {
            return "The Item can't be deleted: " + Message;
        }
    }
}