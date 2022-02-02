using GlobalLogin.App.Filters;
using GlobalLogin.App.Models;
using GlobalLogin.App.Repositories.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GlobalLogin.App.Controllers
{

    public class AuthController : Controller
    {
        private readonly HttpClient _client;
        private readonly IAuth _auth;
        private readonly IConfiguration _config;
        public AuthController(IHttpClientFactory factory, IAuth auth, IConfiguration config)
        {
            _auth = auth;
            _config = config;
            _client = factory.CreateClient(name: "LoginApi.V1");
        }
        [LoginFilter]
        public IActionResult Login([FromQuery] string url)
        {
            string baseurl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Value;
            if (url is object)
            {
                baseurl = url;
            }
            _auth.IsExistedUrl(baseurl, this, out UrlAddDTO outUrl);
            return View();
        }
        [HttpPost]
        public IActionResult Login([FromForm] UserInfo req, [FromQuery] string url, [FromQuery] int permId)
        {

            _auth.Authenticate(HttpContext, req, this, permId);
            if (url is null)
            {
                return RedirectToAction("Sites", "Home");
            }
            return Redirect(url);
        }
        public IActionResult Logout([FromQuery] string url, [FromQuery] int permId)
        {
            if (permId == 0)
            {
                permId = int.Parse(_config["MainConfig:PermId"]);
            }
            string sess = HttpContext.Session.GetString("ssid_token");
            if (sess is object)
            {
                _auth.Logout(sess, permId);
                HttpContext.Session.Remove("ssid_token");
            }
            if (url is object)
            {
                return RedirectToAction("Login", "Auth", new { url = url, permId = permId });
            }
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
