
namespace STS.Common.BaseModels
{
    public class STSGlobalException : Exception
    {
        public STSGlobalException(STSErrorMessage error)
        {
            ErrorMessage = error.Message;
            ErrorCode = error.Code;
            Exception = null;
        }

        public STSGlobalException(STSErrorMessage error, Exception e)
        {
            ErrorMessage = error.Message;
            ErrorCode = error.Code;
            Exception = e;
        }


        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public Exception Exception { get; set; }

    }
}
