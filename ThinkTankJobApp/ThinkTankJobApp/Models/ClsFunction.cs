using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ThinkTankJobApp.Models
{
    public class ClsFunction
    {
        public static string GenerateResetToken(string content)
        {
            var data = Encoding.UTF7.GetBytes(content);
            var newData = new List<byte>();
            foreach (var item in data)
            {

                newData.Add(unchecked((byte)(item + 4)));

            }
            return Convert.ToBase64String(newData.ToArray());
        }
        public static string GetVisitorIPAddress(bool GetLan = false)
        {
            try
            {
                string visitorIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (String.IsNullOrEmpty(visitorIPAddress))
                {
                    visitorIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.IsNullOrEmpty(visitorIPAddress))
                {
                    visitorIPAddress = HttpContext.Current.Request.UserHostAddress;
                }

                if (string.IsNullOrEmpty(visitorIPAddress) || visitorIPAddress.Trim() == "::1")
                {
                    GetLan = true;
                    visitorIPAddress = string.Empty;
                }

                if (GetLan && string.IsNullOrEmpty(visitorIPAddress))
                {
                    //This is for Local(LAN) Connected ID Address
                    string stringHostName = Dns.GetHostName();
                    //Get Ip Host Entry
                    IPHostEntry ipHostEntries = Dns.GetHostEntry(stringHostName);
                    //Get Ip Address From The Ip Host Entry Address List
                    IPAddress[] arrIpAddress = ipHostEntries.AddressList;

                    try
                    {
                        visitorIPAddress = arrIpAddress[arrIpAddress.Length - 1].ToString();
                    }
                    catch
                    {
                        try
                        {
                            visitorIPAddress = arrIpAddress[0].ToString();
                        }
                        catch
                        {
                            try
                            {
                                arrIpAddress = Dns.GetHostAddresses(stringHostName);
                                visitorIPAddress = arrIpAddress[0].ToString();
                            }
                            catch
                            {
                                visitorIPAddress = "127.0.0.1";
                            }
                        }

                    }
                }
                return visitorIPAddress;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
    }
}