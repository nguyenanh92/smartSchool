using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common;
using Common.ZoomConfiguration;
using Microsoft.Ajax.Utilities;
using Models;
using Models.User;
using Newtonsoft.Json;
using SmartSchool.Areas.Admin.Helper;

namespace SmartSchool.Controllers
{
    public class LearingController : Controller
    {
        private static SubjectBus _subjectBus = new SubjectBus();
        private static RoomMeetingBus _roomMeetingBus = new RoomMeetingBus();

        // GET: Learing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StartLearning(int subjectId)
        {
            string role = "0";

            var meeting = _roomMeetingBus.GetInfMeetingBySubjectId(subjectId);

            meeting = JsonConvert.DeserializeObject<MeetingInfo>(meeting.Meeting_Info);

            var token = SignatureZoom.GenerateSignature(meeting.id, role);
            ViewBag.Topic = meeting.topic;

            return RedirectToAction("ScreenLearn", new
            {
                name = "Student",
                mn = meeting.id,
                email = "student@gmail.com",
                pwd = meeting.password,
                role = role,
                lang = "vi-VN",
                signature = token,
                china = 0,
                apiKey = JWT.APIKey
            });

        }

        public ActionResult ScreenLearn()
        {
            return View();
        }

    }
}