using Microsoft.IdentityModel.Tokens;
using System;

namespace NetCore.Jwt
{
    interface ITokenOptions
    {

        TokenValidationParameters TokenValidationParameters { get; }

        TimeSpan Expiary { get; }

    }
}