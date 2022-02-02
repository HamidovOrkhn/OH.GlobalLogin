using GlobalLogin.App.DTO;
using GlobalLogin.App.Repositories;
using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace GlobalLogin.App.Libs.JWT
{
    public class JWTService
    {
        private static IConfiguration configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();
        public static bool ValidateToken(string authToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();

                SecurityToken validatedToken;
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
            };
        }
        public static bool IsExpiredToken(string jwtToken)
        {
            IJsonSerializer _serializer = new JsonNetSerializer();
            IDateTimeProvider _provider = new UtcDateTimeProvider();
            IBase64UrlEncoder _urlEncoder = new JwtBase64UrlEncoder();
            IJwtAlgorithm _algorithm = new HMACSHA256Algorithm();
            try
            {
                IJwtValidator _validator = new JwtValidator(_serializer, _provider);
                IJwtDecoder decoder = new JwtDecoder(_serializer, _validator, _urlEncoder, _algorithm);
                var token = decoder.DecodeToObject<JwtToken>(jwtToken);
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(token.exp);
                if (DateTime.Compare(dateTimeOffset.LocalDateTime, DateTime.Now.AddMinutes(1)) < 0)
                {
                    return false;
                }
                return true;
            }
            catch (TokenExpiredException)
            {
                return false;
            }
            catch (SignatureVerificationException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static async Task<bool> RefreshExpiredToken(HttpContext context, string token, int permId)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(configuration["Remotes:Login-Api-Url"]);
            var response = await client.GetAsync("/api/globalauth/refresh/" + token + "/" + permId);
            ResponseMessage<TokenDto> res = await response.Content.ReadAsAsync<ResponseMessage<TokenDto>>();
            if (response.IsSuccessStatusCode)
            {
                context.Session.SetString("ssid_token", res.Data.Token);
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
