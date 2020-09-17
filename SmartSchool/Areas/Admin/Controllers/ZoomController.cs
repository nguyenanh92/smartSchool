using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common.Config;
using Common.Helper;
using Common;
using Common.ZoomConfiguration;
using Models;
using Models.User;
using Newtonsoft.Json;
using RestSharp;
using Models.ZoomConnect;
using SmartSchool.Areas.Admin.Helper;

namespace SmartSchool.Areas.Admin.Controllers
{
    public class ZoomController : BaseController
    {
        private static ZoomConnectBus _connectBus = new ZoomConnectBus();
        private static ZoomInfoBus _zoomInfoBus = new ZoomInfoBus();

        // GET: Admin/Zoom
        public ActionResult Index()
        {
            var session = (UserLogin)Session[Contans.USER_SESSION];
            var reuslt = _connectBus.GetZoomConnectByUserId(session.ID);

            if (reuslt != null)
            {
                ViewBag.CheckConnect = reuslt;
            }

            return View();
        }

        //Chuyển hướng sang APP cấp quyền Zoom.
        public ActionResult Authentication()
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

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.Messages = "Authorization Error";
                    return PartialView("_ConnectError");
                }
                else
                {
                    var data = JsonConvert.DeserializeObject<ZoomConnect>(response.Content);

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

                    decimal push = 0;
                    var reuslt = _connectBus.GetZoomConnectByUserId(session.ID);
                    if (reuslt == null)
                    {
                        //Lưu respone vào database
                        push = _connectBus.Insert(result);
                    }
                    else
                    {
                        push = _connectBus.Update(result);
                    }

                    if (push != -1)
                    {
                        ViewBag.Messages = "Authorization Success";
                    }

                    return RedirectToAction("Index", "Zoom");

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //Get info của zoom account bằng access_token và trả xuống PartialView
        [HttpGet]
        public ActionResult GetInfoZoom()
        {
            try
            {
                var session = (UserLogin)Session[Contans.USER_SESSION];
                var data = AuthorZoom.GetInfoAPI(session.ID);
                 
                if (data != null)
                {
                    var get = _zoomInfoBus.GetZoomInfoByTearchId(session.ID);
                    var convert = JsonConvert.SerializeObject(data);

                    if (get == null)
                    {
                        _zoomInfoBus.Insert(session.ID, convert);
                    }
                }
                ViewBag.Obj = data;
                return PartialView("_ZoomInfo");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return PartialView("_ConnectError");

            }
        }
  
    }
}