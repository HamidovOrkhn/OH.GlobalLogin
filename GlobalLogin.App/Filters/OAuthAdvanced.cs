using GlobalLogin.App.Libs.JWT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlobalLogin.App.Filters
{
    public class OAuthAdvanced : Attribute
        //: Attribute, IAsyncActionFilter
    {
        //public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    string token = context.HttpContext.Session.GetString("ssid_token");
        //    if (token is null)
        //    {
        //        context.Result = new ObjectResult(new { Token = "", Status = "SessionExpired" })
        //        {
        //            StatusCode = 500
        //        };
        //    }
        //    bool isValid = JWTService.ValidateToken(token);
        //    if (!isValid)
        //    {
        //        context.Result = new ObjectResult(new { Token = "", Status = "UnAuthorized" })
        //        {
        //            StatusCode = 401
        //        };
        //    }
        //    bool isValidExp = JWTService.IsExpiredToken(token);
        //    if (!isValidExp)
        //    {
        //        context.Result = new ObjectResult(new { Token = "", Status = "TokenExpired" })
        //        {
        //            StatusCode = 401
        //        };
        //    }
        //    bool isRefreshed = await JWTService.RefreshExpiredToken(context.HttpContext, token);
        //    if (!isRefreshed)
        //    {
        //        context.Result = new ObjectResult(new { Token = "", Status = "NotRefreshed" })
        //        {
        //            StatusCode = 401
        //        };
        //    }
        //    token = context.HttpContext.Session.GetString("ssid_token");
        //    context.HttpContext.Request.Headers.Add("Authorization", "Bearer " + token);
        //    await next();


        //}
    }
}
