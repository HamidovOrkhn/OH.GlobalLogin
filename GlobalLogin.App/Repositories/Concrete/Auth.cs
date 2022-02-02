using GlobalLogin.App.DTO;
using GlobalLogin.App.Libs;
using GlobalLogin.App.Models;
using GlobalLogin.App.Repositories.Abstracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GlobalLogin.App.Repositories.Concrete
{
    public class Auth : IAuth
    {
        private readonly HttpClient _client;
        private readonly IHttpContextAccessor _context;
        private readonly ITempDataDictionaryFactory _tempDataDictionaryFactory;
        public Auth(IHttpClientFactory factory, IHttpContextAccessor context, ITempDataDictionaryFactory tempData)
        {
            _client = factory.CreateClient(name: "LoginApi.V1");
            _context = context;
            _tempDataDictionaryFactory = tempData;
        }
        public void Authenticate(HttpContext context, UserInfo entity, Controller controller, int permId)
        {
            var response = _client.PostAsJsonAsync("/api/globalauth/login" + "/" + permId + "/" + StaticHelper.GetIpAddress(), new
            {
                pin = entity.Pin,
                password = entity.Password
            }).Result;
            ResponseMessage<TokenDto> res = response.Content.ReadAsAsync<ResponseMessage<TokenDto>>().Result;
            controller.TempData["Test"] = "test";
            if (response.IsSuccessStatusCode)
            {
                _context.HttpContext.Session.SetString("ssid_token", res.Data.Token);
            }
            else
            {
                controller.TempData["RemoteAddressError"] = res.Message;
            }
        }
        public void IsExistedUrl(string url, Controller controller, out UrlAddDTO outUrl)
        {
            var response = _client.PostAsJsonAsync("/api/tools/validateurl", new { url = url }).Result;
            outUrl = new UrlAddDTO();
            if (response.IsSuccessStatusCode)
            {
                outUrl = response.Content.ReadAsAsync<UrlAddDTO>().Result;
            }
            else
            {
                controller.TempData["NotValidUrlError"] = "Not Valid Url";
            }

        }
        public async Task<List<UserRole>> InjectRoles(string token)
        {
            var response = await _client.GetAsync("/api/globalauth/getuser/" + token);
            ResponseMessage<List<UserRole>> userRoles = await response.Content.ReadAsAsync<ResponseMessage<List<UserRole>>>();
            return userRoles.Data;
        }
        public async void Logout(string token, int permId)
        {
            var response = await _client.GetAsync($"/api/globalauth/logout/{token}/{permId}/{StaticHelper.GetIpAddress()}");
        }

    }
}
