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

namespace SmartSchool.Areas.Admin.Controllers
{
    public class SubjectController : BaseController
    {
        private static SubjectBus _subjectBus = new SubjectBus();
        private static ZoomConnectBus _connectBus = new ZoomConnectBus();
        // GET: Admin/Subject

        public ActionResult Index()
        {
            var session = (ZoomInfo)Session[Contans.INFO_ZOOM_SESSION];
            var lst = _subjectBus.GetById(session.UserId);
            ViewBag.Subject = lst;
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

                var response = RequestApi(Api.BASE_URL + url, rq);

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

        public static IRestResponse RequestApi(string Uri, ZoomConnect reuslt)
        {
            try
            {
                var client = new RestSharp.RestClient(Uri);
                var request = new RestRequest(Method.GET);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", reuslt.Token_type + " " + reuslt.Access_token);

                IRestResponse response = client.Execute(request);

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}