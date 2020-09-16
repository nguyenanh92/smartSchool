using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common;
using Common.ZoomConfiguration;
using Models;
using Models.User;
using Models.ZoomConnect;
using Newtonsoft.Json;
using RestSharp;
using SmartSchool.Areas.Admin.Helper;

namespace SmartSchool.Areas.Admin.Controllers
{
    public class SubjectController : BaseController
    {
        private static SubjectBus _subjectBus = new SubjectBus();
        private static ZoomConnectBus _connectBus = new ZoomConnectBus();
        private static ZoomInfoBus _zoomInfoBus = new ZoomInfoBus();
        // GET: Admin/Subject

        public ActionResult Index()
        {
            var sesUser = (UserLogin)Session[Contans.USER_SESSION];
            List<Subject> list = new List<Subject>();
             

            list = _subjectBus.GetById(sesUser.ID);
            ViewBag.Subject = list;
            return View();
        }

        public ActionResult StartLesson(CreateMeetingModel model, string status = "LIVE")
        {
            var sesUser = (UserLogin)Session[Contans.USER_SESSION];

            var info = _zoomInfoBus.GetZoomInfoByTearchId(sesUser.ID);

            if (info !=  null)
            {
                var data = JsonConvert.DeserializeObject<ZoomInfo>(info.Info);

                var create = AuthorZoom.CreateMeetingAPI(sesUser.ID, data.Id, model);
                //nếu tạo thành công nhớ đổi status 

                var result = _subjectBus.Update(model.SubjectId, status);

                var p = JsonConvert.DeserializeObject<MeetingViewModel>(create);


                var signature = SignatureZoom.GenerateSignature(p.id, model.Role);

                ViewBag.Name = sesUser.username;
                ViewBag.Title = p.topic;
                ViewBag.Id = p.id;
                ViewBag.Pwd = p.password;
                ViewBag.Role = model.Role;
                ViewBag.Signature = signature;
                ViewBag.ApiKey = JWT.APIKey;

                return View("ScreenLesson");


                //return RedirectToAction("ScreenLesson", new
                //{
                //    name = sesUser.username,
                //    mn =  p.id,
                //    email = "student@gmail.com",
                //    pwd = p.password,
                //    role = model.Role,
                //    lang = "vi-VN",
                //    signature = signature,
                //    china = 0,
                //    apiKey = JWT.APIKey
                //});


            }
            return View("_MeetingError");
        }


        public ActionResult ScreenLesson()
        {
            return View();
        }
    }
}