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
        // GET: Admin/Subject

        public ActionResult Index()
        {
            List<Subject> list = new List<Subject>();
            var session = (ZoomInfo)Session[Contans.INFO_ZOOM_SESSION];
            if (session == null)
            {
                var sesUser = (UserLogin)Session[Contans.USER_SESSION];
                var data = AuthorZoom.GetInfoAPI(sesUser.ID);
                var ses = new ZoomInfo();
                ses.Personal_meeting_url = data.Personal_meeting_url;
                ses.Pmi = data.Pmi;
                ses.UserId = sesUser.ID;
                Session.Add(Contans.INFO_ZOOM_SESSION, ses);

                list = _subjectBus.GetById(sesUser.ID);
                ViewBag.Subject = list;
                return View();
            }
            list = _subjectBus.GetById(session.UserId);
            ViewBag.Subject = list;
            return View();
        }

        public ActionResult StartLesson(int subjectId, string status = "LIVE")
        {
            var session = (ZoomInfo)Session[Contans.INFO_ZOOM_SESSION];
            var result = _subjectBus.Update(subjectId, status);
            if (result != -1)
            {
                var url = "v2/meetings/" + session.Pmi + "";

                var rq = _connectBus.GetById(session.UserId);

                var response = AuthorZoom.RequestApi(Api.BASE_URL + url, rq);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    try
                    {

                        var data = JsonConvert.DeserializeObject<SubjectItem>(response.Content);

                        if (Session["Subjects"] == null)
                        {
                            Session["Subjects"] = new List<SubjectItem>();
                        }
                        List<SubjectItem> list = Session["Subjects"] as List<SubjectItem>;

                        SubjectItem newItem = new SubjectItem()
                        {
                            Id = subjectId,
                            MeetingId = data.Id,
                            Password = data.Password,
                            Created_at = data.Created_at,
                            Start_url = data.Start_url,
                            Join_url = data.Join_url,
                        };

                        list.Add(newItem);

                        //ViewBag.Obj = data;
                        return Redirect(data.Start_url);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                }

            }
            return View("_MeetingError");
        }
    }
}