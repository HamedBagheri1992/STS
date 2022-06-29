using System.Text;
using System.Security.Cryptography;

namespace STS.Common.Extensions
{
    public static class StringExtensions
    {
        public static string ToHashPassword(this string pass)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(pass);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);
            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }

        public static bool HasSpecialChars(this string input)
        {
            //return input.Any(ch => !Char.IsLetterOrDigit(ch));
            return input.Contains(' ');
        }
    }
}
