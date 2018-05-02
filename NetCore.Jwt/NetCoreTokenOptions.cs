using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetCore.Jwt
{
    public class NetCoreTokenOptions : ITokenOptions
    {

        public TokenValidationParameters TokenValidationParameters { get; set; }

        public string Secret
        {
            set
            {
                TokenValidationParameters.IssuerSigningKey = GetSignInKey(value);
            }
        }

        public string Issuer
        {
            set
            {
                TokenValidationParameters.ValidateIssuer = true;
                TokenValidationParameters.ValidIssuer = value;
            }
        }

        public string Audience
        {
            set
            {
                TokenValidationParameters.ValidateAudience = true;
                TokenValidationParameters.ValidAudience = value;
            }
        }

        public TimeSpan Expiary
        {
            get => _expiary; set
            {
                _expiary = value;
                TokenValidationParameters.ValidateLifetime = true;
                TokenValidationParameters.RequireExpirationTime = true;
            }
        }

        public string GenerateSecret(int size)
        {
            string secret = RandomString(size);
            Secret = secret;
            return secret;
        }

        internal NetCoreTokenOptions()
        {
            TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSignInKey(RandomString(20)),

                ValidateIssuer = false,

                ValidateAudience = false,

                ValidateLifetime = false,
                RequireExpirationTime = false,

                ClockSkew = TimeSpan.Zero
            };
        }

        static SymmetricSecurityKey GetSignInKey(string secretKey)
            => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        // https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings-in-c
        static Random random = new Random();
        private TimeSpan _expiary;

        static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
