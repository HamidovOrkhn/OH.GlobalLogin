using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalLogin.App.Filters
{
    public class LoginFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("Auth middleware invoked");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string token = context.HttpContext.Session.GetString("ssid_token");
            if (token is object)
            {
                context.Result = new RedirectToRouteResult(
                   new RouteValueDictionary
                   {
                    { "controller", "Home" },
                    { "action", "Sites" }
                   });
                return;
            }
        }
    }
}
