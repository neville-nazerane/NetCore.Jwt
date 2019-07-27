using Microsoft.AspNetCore.Authentication;
using NetCore.Jwt;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceExtensions
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


    }
}
