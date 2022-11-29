

namespace BO
{
    [Serializable]
    public class DataRequestFailedException: Exception
    {
        public DataRequestFailedException(): base(){}
        public DataRequestFailedException(string message, Exception inner): base(message, inner) { }
        public override string ToString()
        {
            return Message + "\nInner exception: " + InnerException;
        }
    }
}
