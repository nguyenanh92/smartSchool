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

        // GET: Learing
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult StartLearning(int subjectId)
        //{

        //    //get thông tin  của lesson ở session
        //    List<SubjectItem> list = Session["Subjects"] as List<SubjectItem>;
        //    SubjectItem item = list.FirstOrDefault(a => a.Id == subjectId);
        //    string role = "0";
        //    var token = GenerateSignature(item.MeetingId.ToString() , role );
        //    //var Url = "?name=Student&mn=" + item.MeetingId + "&email=&pwd="+ item.Password +"&role=0&lang=vi-VN&signature="+ token +"&china=0&apiKey=" + apiKey;
        //    return RedirectToAction("ScreenLearn", new
        //    {
        //        name = "Student",
        //        mn = meetingNumber,
        //        email = "student@gmail.com",
        //        pwd = item.Password,
        //        role = role,
        //        lang = "vi-VN",
        //        signature = token,
        //        china = 0 ,
        //        apiKey = apiKey
        //    });

        //}

        public ActionResult ScreenLearn()
        {
            return View();
        }

    }
}