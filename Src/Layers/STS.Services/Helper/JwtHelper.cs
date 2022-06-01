using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
        public static string ToJwtTokenGenerator(this UserViewModel user, IOptionsMonitor<BearerTokensConfigurationModel> options, List<PermissionViewModel> permissions)
        {
            var claims = InitClaims(user, options);
            AddClaims(claims, permissions);

            JwtSecurityToken token = CreateToken(options, claims);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static JwtSecurityToken CreateToken(IOptionsMonitor<BearerTokensConfigurationModel> options, List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.CurrentValue.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(issuer: options.CurrentValue.Issuer, audience: options.CurrentValue.Audience, claims: claims, notBefore: now,
                expires: now.AddDays(options.CurrentValue.AccessTokenExpirationDays), signingCredentials: creds);
            return token;
        }

        private static List<Claim> InitClaims(UserViewModel user, IOptionsMonitor<BearerTokensConfigurationModel> options)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iss, options.CurrentValue.Issuer),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Surname, user.LastName??""),
                new Claim(ClaimTypes.GivenName, user.FirstName??""),
                new Claim("displayName",$"{user.FullName ?? ""}")
            };
        }

        private static void AddClaims(List<Claim> claims, List<PermissionViewModel> permissions)
        {
            foreach (PermissionViewModel permission in permissions)
            {
                claims.Add(new Claim(ClaimTypes.Role, permission.Title));
            }
        }
    }
}
