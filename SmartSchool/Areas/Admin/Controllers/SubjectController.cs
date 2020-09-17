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
        private static RoomMeetingBus _roomMeetingBus = new RoomMeetingBus();
        private static ZoomInfoBus _zoomInfoBus = new ZoomInfoBus();
        // GET: Admin/Subject
        public ActionResult Index()
        {
            var sesUser = (UserLogin)Session[Contans.USER_SESSION];
            var data = AuthorZoom.GetInfoAPI(sesUser.ID);
            if (data != null)
            {
                List<Subject> list = new List<Subject>();
                list = _subjectBus.GetById(sesUser.ID);
                ViewBag.Subject = list;
                return View();
            }
            else
            {
                return RedirectToAction("GetInfoZoom", "Zoom");
            }
        }

        public ActionResult StartLesson(CreateMeetingModel model, string status = "LIVE")
        {
            var sesUser = (UserLogin)Session[Contans.USER_SESSION];

            var info = _zoomInfoBus.GetZoomInfoByTearchId(sesUser.ID);
            
            if (info !=  null)
            {
                try
                {
                    var data = JsonConvert.DeserializeObject<ZoomInfo>(info.Info);

                    var response = AuthorZoom.CreateMeetingAPI(sesUser.ID, data.Id, model);

                    if (response.StatusCode == HttpStatusCode.Created)
                    {
                        var result = _subjectBus.Update(model.SubjectId, status);

                        var getMeeting = _roomMeetingBus.GetInfMeetingBySubjectId(model.SubjectId);

                        if (getMeeting == null)
                        {
                             _roomMeetingBus.Insert(model.SubjectId, response.Content);
                        }
                        else
                        {
                             _roomMeetingBus.Update(model.SubjectId, response.Content);
                        }

                        var p = JsonConvert.DeserializeObject<MeetingViewModel>(response.Content);
                        var signature = SignatureZoom.GenerateSignature(p.id, model.Role);

                        ViewBag.Name = sesUser.username;
                        ViewBag.Title = p.topic;
                        ViewBag.Id = p.id;
                        ViewBag.Pwd = p.password;
                        ViewBag.Role = model.Role;
                        ViewBag.Signature = signature;
                        ViewBag.ApiKey = JWT.APIKey;

                        return View("ScreenLesson");
                    }
                   
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

            }
            return View("_MeetingError");
        }


        public ActionResult ScreenLesson()
        {
            return View();
        }
    }
}