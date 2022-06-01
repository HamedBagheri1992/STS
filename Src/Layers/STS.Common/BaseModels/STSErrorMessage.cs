
namespace STS.Common.BaseModels
{
    public class STSErrorMessage
    {
        public STSErrorMessage(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return $"{Code}:\t{Message}";
        }
    }
}
