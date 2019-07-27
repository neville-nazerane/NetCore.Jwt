using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Text;


using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using NetCore.Jwt;

namespace Microsoft.AspNetCore.Mvc
{
    public static class AuthenticationExtensions
    {


        static IBearerManager GetManager(this HttpContext context) => context.RequestServices.GetService<IBearerManager>();

        public static string GenerateBearerToken(this HttpContext context, ClaimsIdentity identity)
            => context.GetManager().Generate(identity);

        public static string GenerateBearerToken(this HttpContext context, IEnumerable<Claim> claims)
            => context.GetManager().Generate(claims);

        public static string GenerateBearerToken(this HttpContext context, string userName, IEnumerable<string> roles = null)
            => context.GetManager().Generate(userName, roles);

    }
}
