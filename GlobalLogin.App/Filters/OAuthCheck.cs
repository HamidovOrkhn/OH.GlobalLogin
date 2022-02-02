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
    public class OAuthCheck : Attribute, IActionFilter
    {
        private readonly IConfiguration _configuration;
        public OAuthCheck()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("Auth middleware invoked");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Headers.TryGetValue("Authorization", out var value);
            string token = "";
            if (value != string.Empty)
            {
                token = value.First();
            }
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
                context.Result = new ObjectResult(new { Token = "", Status = "NotRefreshed" })
                {
                    StatusCode = 401
                };
                return;
            }
        }
    }
}
