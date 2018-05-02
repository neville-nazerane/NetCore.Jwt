using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetCore.Jwt
{
    class BearerManager : IBearerManager
    {
        private readonly ITokenOptions options;

        public BearerManager(ITokenOptions options)
        {
            this.options = options;
        }

        public string Generate(ClaimsIdentity identity)
        {
            var handler = new JwtSecurityTokenHandler();
            var security = new SecurityTokenDescriptor
            {
                Subject = identity,
                Audience = options.TokenValidationParameters.ValidAudience,
                Issuer = options.TokenValidationParameters.ValidIssuer,
                SigningCredentials = new SigningCredentials(
                options.TokenValidationParameters.IssuerSigningKey, SecurityAlgorithms.HmacSha256)
            };
            if (options.Expiary != TimeSpan.FromMilliseconds(0))
                security.Expires = DateTime.UtcNow.Add(options.Expiary);
            var jwt = handler.CreateJwtSecurityToken(security);
            
            return handler.WriteToken(jwt);
        }

        public string Generate(IEnumerable<Claim> claims)
            => Generate(new ClaimsIdentity(claims, "local"));

        public string Generate(string userName, IEnumerable<string> roles = null)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, userName)
            };
            if (roles != null)
                foreach (var role in roles)
                    claims.Add(new Claim(ClaimTypes.Role, role));
            return Generate(claims);
        }
    }
}
