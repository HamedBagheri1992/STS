using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using STS.Common.BaseModels;
using STS.Common.Configuration;
using STS.DTOs.PermissionModels.ViewModels;
using STS.DTOs.UserModels.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace STS.Services.Helper
{
    public static class JwtHelper
    {
        public static string ToJwtTokenGenerator(this UserIdentityBaseModel userIdentity, IOptionsMonitor<BearerTokensConfigurationModel> options)
        {
            var claims = InitClaims(userIdentity, options);
            AddClaims(claims, userIdentity.PermissionPairs);

            JwtSecurityToken token = CreateToken(options, claims, userIdentity.ExpirationDuration);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static JwtSecurityToken CreateToken(IOptionsMonitor<BearerTokensConfigurationModel> options, List<Claim> claims, int expirationDuration)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.CurrentValue.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(issuer: options.CurrentValue.Issuer, audience: options.CurrentValue.Audience, claims: claims, notBefore: now,
                expires: now.AddMinutes(expirationDuration), signingCredentials: creds);
            return token;
        }

        private static List<Claim> InitClaims(UserIdentityBaseModel userIdentity, IOptionsMonitor<BearerTokensConfigurationModel> options)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, options.CurrentValue.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.NameIdentifier, userIdentity.Id.ToString()),
                new Claim(ClaimTypes.Name, userIdentity.UserName),
                new Claim(ClaimTypes.Surname, userIdentity.LastName??""),
                new Claim(ClaimTypes.GivenName, userIdentity.FirstName??""),
                new Claim("displayName",$"{userIdentity.FullName ?? ""}")
            };
        }

        private static void AddClaims(List<Claim> claims, List<KeyValueModel> permissions)
        {
            foreach (KeyValueModel permission in permissions)
            {
                claims.Add(new Claim(ClaimTypes.Role, permission.Value));
            }
        }
    }
}
