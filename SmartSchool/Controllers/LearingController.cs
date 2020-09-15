using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common;
using Common.ZoomConfiguration;
using Models;
using Models.User;

namespace SmartSchool.Controllers
{
    public class LearingController : Controller
    {
        private static SubjectBus _subjectBus = new SubjectBus();
        private static readonly char[] padding = { '=' };

        // GET: Learing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StartLearning(int subjectId)
        {

            //get thông tin  của lesson ở session
            List<SubjectItem> list = Session["Subjects"] as List<SubjectItem>;
            SubjectItem item = list.FirstOrDefault(a => a.Id == subjectId);

            string apiKey = JWT.APIKey;
            string apiSecret =  JWT.APISecret;
            string meetingNumber = item.MeetingId.ToString();
            String ts = (ToTimestamp(DateTime.UtcNow.ToUniversalTime()) - 30000).ToString();
            string role = "0";
            string token = GenerateToken(apiKey, apiSecret, meetingNumber, ts, role);

            //var Url = "?name=Student&mn=" + item.MeetingId + "&email=&pwd="+ item.Password +"&role=0&lang=vi-VN&signature="+ token +"&china=0&apiKey=" + apiKey;
            return RedirectToAction("ScreenLearn", new
            {
                name = "Student",
                mn = meetingNumber,
                email = "student@gmail.com",
                pwd = item.Password,
                role = 0,
                lang = "vi-VN",
                signature = token,
                china = 0 ,
                apiKey = apiKey
            });

        }

        public ActionResult ScreenLearn()
        {
            return View();
        }
        public static long ToTimestamp(DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000;
            return epoch;
        }

        public static string GenerateToken(string apiKey, string apiSecret, string meetingNumber, string ts, string role)
        {
            string message = String.Format("{0}{1}{2}{3}", apiKey, meetingNumber, ts, role);
            apiSecret = apiSecret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(apiSecret);
            byte[] messageBytesTest = encoding.GetBytes(message);
            string msgHashPreHmac = System.Convert.ToBase64String(messageBytesTest);
            byte[] messageBytes = encoding.GetBytes(msgHashPreHmac);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                string msgHash = System.Convert.ToBase64String(hashmessage);
                string token = String.Format("{0}.{1}.{2}.{3}.{4}", apiKey, meetingNumber, ts, role, msgHash);
                var tokenBytes = System.Text.Encoding.UTF8.GetBytes(token);
                return System.Convert.ToBase64String(tokenBytes).TrimEnd(padding);
            }
        }
    }
}