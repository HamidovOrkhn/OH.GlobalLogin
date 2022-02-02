using GlobalLogin.App.Libs.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalLogin.App.Filters
{
    public class OAuth : Attribute, IActionFilter
    {
        private readonly IConfiguration _configuration;
        public OAuth()
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
                context.Result = new ObjectResult(new { Token = "", Status = "SessionExpired" })
                {
                    StatusCode = 500
                };
                return;
            }
            bool isValid = false;
            isValid = JWTService.ValidateToken(token);
            if (!isValid)
            {
                context.Result = new ObjectResult(new { Token = "", Status = "UnAuthorized" })
                {
                    StatusCode = 401
                };
                return;
            }
            bool isValidExp = JWTService.IsExpiredToken(token);
            if (!isValidExp)
            {
                string permId = _configuration["MainConfig:PermId"];
                string routePerm = context.RouteData.Values["permId"].ToString();
                if (routePerm is object)
                {
                    permId = routePerm;
                }
                bool isRefreshed = JWTService.RefreshExpiredToken(context.HttpContext, token, int.Parse(permId)).Result;
                if (!isRefreshed)
                {
                    context.Result = new ObjectResult(new { Token = "", Status = "NotRefreshed" })
                    {
                        StatusCode = 401
                    };
                    return;
                }
                token = context.HttpContext.Session.GetString("ssid_token");
                context.Result = new ObjectResult(new { Token = token, Status = "Refreshed" })
                {
                    StatusCode = 200
                };
                return;
            }
        }
    }
}
