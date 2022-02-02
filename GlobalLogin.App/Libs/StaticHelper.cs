using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace GlobalLogin.App.Libs
{
    public class StaticHelper
    {
        internal static string GetIpAddress()
        {
                try
                {
                    string hostname = Dns.GetHostName();
                    var ip = Dns.GetHostByName(hostname).AddressList;

                    string response = "";
                    foreach (var item in ip)
                    {
                        if (item.ToString().Contains("192"))
                        {
                            response = item.ToString();
                        }
                    }
                    return response;
                }
                catch (Exception)
                {
                    return null;
                }

            
        }
    }
}
