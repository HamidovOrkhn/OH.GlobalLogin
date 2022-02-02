using GlobalLogin.App.Filters;
using GlobalLogin.App.Libs;
using GlobalLogin.App.Models;
using GlobalLogin.App.Repositories.Abstracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalLogin.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuth _authService;
        public HomeController(IAuth auth)
        {
            _authService = auth;
        }
        [LoginFilter]
        public IActionResult Index()
        {
            return View();
        }
        [OAuthClient]
        public  async Task<IActionResult> Sites()
        {
            List<UserRole> roles = await _authService.InjectRoles(HttpContext.Session.GetString("ssid_token"));
            return View(roles);
        }
        [OAuthClient]
        public IActionResult Services()
        {
            return View();
        }
    }
}
