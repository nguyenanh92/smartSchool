using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Common.Config;
using Common.Config;
using RestSharp;
using Common.Config;

namespace SmartSchool.Controllers
{
    public class ZoomAPIController : Controller
    {
        // GET: Zoom
        public ActionResult Index()
        {
            var client = new RestSharp.RestClient("https://zoom.us/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Basic "+ Base64Encode(OAuthConfiguration.clientId+ ':' +OAuthConfiguration.clientSecret));
            IRestResponse response = client.Execute(request);
            ViewBag.Response = response;
            return View();
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}