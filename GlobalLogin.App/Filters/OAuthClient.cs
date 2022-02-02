using GlobalLogin.App.Libs.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalLogin.App.Filters
{
    public class OAuthClient : Attribute, IActionFilter
    {
        private readonly IConfiguration _configuration;
        public OAuthClient()
        {
             _configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("Auth middleware invoked");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.HttpContext.Session.GetString("ssid_token");
            if (token is null)
            {
                context.Result = new RedirectToRouteResult(
                   new RouteValueDictionary
                   {
                    { "controller", "Auth" },
                    { "action", "Login" }
                   });
                return;
            }
            bool isValid = false;
            isValid = JWTService.ValidateToken(token);
            if (!isValid)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                    { "controller", "Auth" },
                    { "action", "Login" }
                    });
                return;
            }
            bool isValidExp = JWTService.IsExpiredToken(token);
            if (!isValidExp)
            {
                string permId = _configuration["MainConfig:PermId"];
                var routePerm = context.RouteData.Values["permId"];
                if (routePerm is object)
                {
                    permId = routePerm.ToString();
                }
                bool isRefreshed = JWTService.RefreshExpiredToken(context.HttpContext, token, int.Parse(permId)).Result;
                if (!isRefreshed)
                {
                    context.Result = new RedirectToRouteResult(
                   new RouteValueDictionary
                   {
                    { "controller", "Auth" },
                    { "action", "Login" }
                   });
                    return;
                }
                token = context.HttpContext.Session.GetString("ssid_token");
                return;
            }
        }
    }
}
