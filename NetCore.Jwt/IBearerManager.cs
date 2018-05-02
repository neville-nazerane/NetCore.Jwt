using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace NetCore.Jwt
{
    public interface IBearerManager
    {

        string Generate(ClaimsIdentity identity);

        string Generate(IEnumerable<Claim> claims);

        string Generate(string userName, IEnumerable<string> roles = null);

    }
}
