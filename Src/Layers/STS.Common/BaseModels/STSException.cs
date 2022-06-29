
using System.Globalization;

namespace STS.Common.BaseModels
{
    public class STSException : Exception
    {
        public STSException() : base() { }

        public STSException(string message) : base(message) { }

        public STSException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
    }
}
