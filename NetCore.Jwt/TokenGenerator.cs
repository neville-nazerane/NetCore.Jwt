using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetCore.Jwt
{
    internal class TokenGenerator
    {

        private const string DefaultSecretKey = "MyVeryLongAndSecretKey";

        private TokenValidationParameters TokenValidationParameters
            => new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSignInKey,

                ValidateIssuer = false,

                ValidateAudience = false,

                ValidateLifetime = true,
                RequireExpirationTime = true,

                ClockSkew = TimeSpan.Zero
            };

        private SymmetricSecurityKey GetSignInKey
            => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        public TimeSpan Expiary { get; set; }

        public string SecretKey { private get; set; }

        public TokenGenerator()
        {
            SecretKey = DefaultSecretKey;
            Expiary = TimeSpan.FromDays(1);
        }

        public string Generate(IEnumerable<Claim> claims)
        {
            return Generate(new ClaimsIdentity(claims, "local"));
        }

        public string Generate(ClaimsIdentity identity)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.CreateJwtSecurityToken(new SecurityTokenDescriptor
            {

                Expires = DateTime.UtcNow.Add(Expiary),
                Subject = identity,
                Audience = TokenValidationParameters.ValidAudience,
                Issuer = TokenValidationParameters.ValidIssuer,
                SigningCredentials = new SigningCredentials(
                            TokenValidationParameters.IssuerSigningKey, SecurityAlgorithms.HmacSha256)
            });
            return handler.WriteToken(jwt);
        }

        public string Generate(string username, string roles = null)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, username),
            };
            if (roles != null)
                foreach (string role in roles.Split(','))
                    claims.Add(new Claim(ClaimTypes.Role, role));
            return Generate(claims);
        }

        public ClaimsPrincipal Validate(string token)
        {
            var handle = new JwtSecurityTokenHandler();
            SecurityToken sToken = handle.CreateJwtSecurityToken();
            return handle.ValidateToken(token, TokenValidationParameters, out sToken);
        }

    }
}
