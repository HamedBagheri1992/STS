using STS.Common.BaseModels;

namespace STS.WebApi.Helper
{
    public class ErrorDetailsHelper
    {
        public static ErrorDetails VerificationErrorDetails(string error)
        {
            return new ErrorDetails
            {
                ErrorCode = "100",
                Messages = new List<string> { error }
            };
        }
    }
}
