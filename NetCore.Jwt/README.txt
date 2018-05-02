
> Add authentication using "services.AddAuthentication(NetCoreJwtDefaults.SchemeName).AddNetCoreJwt();"
> Generate tokens using "HttpContext.GenerateBearerToken()" overloads
> To generate using DI, use IBearerManager service