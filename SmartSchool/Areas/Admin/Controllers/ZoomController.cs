using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common.Config;
using Common.Helper;
using Common;
using Common.ZoomConfiguration;
using Models.User;
using Newtonsoft.Json;
using RestSharp;
using Models.ZoomConnect;

namespace SmartSchool.Areas.Admin.Controllers
{
    public class ZoomController : Controller
    {
        private static ZoomConnectBus _connectBus = new ZoomConnectBus();

        // GET: Admin/Zoom
        public ActionResult Index()
        {
            var session = (UserLogin)Session[Contans.USER_SESSION];
            var reuslt = _connectBus.GetById(session.ID);

            if (reuslt != null)
            {
                ViewBag.CheckConnect = reuslt;
            }
            return View();
        }

        //Chuyển hướng sang APP cấp quyền Zoom.
        public ActionResult Authorization()
        {
            var uri = OAuth.Uri;
            return Redirect(uri);
        }

        //CallBack trả về mã code , POST mã code + params # sang API # để get về Access_token và Scope....
        public ActionResult CallBack()
        {
            try
            {
                var rp_code = Request.QueryString["code"];

                var client = new RestSharp.RestClient("https://zoom.us/oauth/token");

                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "Basic " + OAuth.TOKEN);

                request.RequestFormat = DataFormat.Json;
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", rp_code);
                request.AddParameter("redirect_uri", OAuth.redirectURL);

                IRestResponse response = client.Execute(request);

                var data = JsonConvert.DeserializeObject<ZoomConnect>(response.Content);

                try
                {
                    //Lấy ra ID của user hiện tại.
                    var session = (UserLogin)Session[Contans.USER_SESSION];

                    var result = new ZoomConnect()
                    {
                        UserId = session.ID,
                        Access_token = data.Access_token,
                        Token_type = data.Token_type,
                        Refresh_token = data.Refresh_token,
                        Expires_in = data.Expires_in,
                        Scope = data.Scope,
                        Status = data.Status,
                    };

                    //Lưu respone vào database
                    var push = _connectBus.Insert(result);

                    if (push != -1)
                    {
                        ViewBag.Messages = "Authorization Success";

                    }
                    return View("Index");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    
        public JsonResult GetInfoZoomAPI()
        {
            var session = (UserLogin)Session[Contans.USER_SESSION];

            var reuslt = _connectBus.GetById(session.ID);

            var url = "v2/users/me";

            var client = new RestSharp.RestClient(Api.BASE_URL + url);

            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", reuslt.Token_type + reuslt.Access_token);

            IRestResponse response = client.Execute(request);

            var data = JsonConvert.DeserializeObject<ZoomConnect>(response.Content);

            return Json(new { data = data, status = true }, JsonRequestBehavior.AllowGet);
        }

    }
}