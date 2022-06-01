using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STS.Common.Configuration
{
    public class BearerTokensConfigurationModel
    {
        public const string Name = "BearerTokens";
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpirationDays { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
        public bool AllowMultipleLoginsFromTheSameUser { get; set; }
        public bool AllowSignoutAllUserActiveClients { get; set; }

    }
}
