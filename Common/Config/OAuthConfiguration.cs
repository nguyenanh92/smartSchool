using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common.Helper;

namespace Common.Config
{
    public class OAuthConfiguration
    {
        //Variables for storing the clientID and clientSecret key
        public static string URL_API = "https://zoom.us/oauth/authorize?response_type=code&client_id=";
        public static string clientId = ConfigurationManager.AppSettings["clientId"];
        public static string clientSecret = ConfigurationManager.AppSettings["clientSecret"];
        public static string redirectURL = ConfigurationManager.AppSettings["redirectURL"];

        public static string Uri = URL_API + clientId + "&redirect_uri=" + redirectURL;

    }
}
