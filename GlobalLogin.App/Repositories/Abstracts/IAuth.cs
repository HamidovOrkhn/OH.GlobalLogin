using GlobalLogin.App.DTO;
using GlobalLogin.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalLogin.App.Repositories.Abstracts
{
    public interface IAuth
    {
        public void Authenticate(HttpContext context, UserInfo entity, Controller controller, int permId);
        public void IsExistedUrl(string url, Controller controller, out UrlAddDTO outUrl);
        public Task<List<UserRole>> InjectRoles(string token);
        public void Logout(string token, int permId);
    }
}
