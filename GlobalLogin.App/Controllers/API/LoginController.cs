using GlobalLogin.App.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalLogin.App.Controllers.API
{
    [EnableCors("CorsPolicy")]
    [Route("api/oauth/[controller]")]
    [ApiController]
 
    public class LoginController : ControllerBase
    {
        [OAuth]
        [HttpGet("[action]/{permId}")]
        public IActionResult Token(int permId)
        {
            string authToken = HttpContext.Session.GetString("ssid_token");
            return Ok(new { Token = authToken, Status = "Authenticated" });
        }
        [OAuthCheck]
        [HttpGet("[action]/{permId}")]
        public IActionResult Check(int permId)
        {
            string authToken = HttpContext.Session.GetString("ssid_token");
            return Ok(new { Token = authToken, Status = "Authenticated" });
        }
    }
}
