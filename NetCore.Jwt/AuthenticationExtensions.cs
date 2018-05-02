using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;


using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace NetCore.Jwt
{
    public static class AuthenticationExtensions
    {

        public static AuthenticationBuilder AddNetCoreJwt(this AuthenticationBuilder builder, Action<NetCoreTokenOptions> configuration = null)
        {

            var opts = new NetCoreTokenOptions();
            configuration?.Invoke(opts);

            builder.Services.AddSingleton<ITokenOptions>(opts)
                            .AddSingleton<IBearerManager, BearerManager>();

            builder.AddJwtBearer(NetCoreJwtDefaults.SchemeName, config => {
                
                config.TokenValidationParameters = opts.TokenValidationParameters;
            });

            return builder;
        }

        static IBearerManager GetManager(this HttpContext context) => context.RequestServices.GetService<IBearerManager>();

        public static string GenerateBearerToken(this HttpContext context, ClaimsIdentity identity)
            => context.GetManager().Generate(identity);

        public static string GenerateBearerToken(this HttpContext context, IEnumerable<Claim> claims)
            => context.GetManager().Generate(claims);

        public static string GenerateBearerToken(this HttpContext context, string userName, IEnumerable<string> roles = null)
            => context.GetManager().Generate(userName, roles);

    }
}
