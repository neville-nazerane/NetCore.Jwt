using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCore.Jwt;
using Sample.Internal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.Internal.Controllers
{

    [Route("")]
    public class ApiController : Controller
    {

        [Authorize, HttpGet]
        public string Get() => "Entered as " + User.Identity.Name;

        [HttpPost]
        public IActionResult Post([FromBody]LoginModel login)
        {
            if (login.Password == "pass")
            {
                return Ok(HttpContext.GenerateBearerToken(login.UserName));
            }
            return BadRequest();
        }

    }
}
