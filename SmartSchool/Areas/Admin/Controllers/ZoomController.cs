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

namespace SmartSchool.Areas.Admin.Controllers
{
    public class ZoomController : BaseController
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

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    ViewBag.Messages = "Authorization Success";
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
                    var reuslt = _connectBus.GetById(session.ID);
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
        public ActionResult GetInfoZoomAPI()
        {
            try
            {
                var url = "v2/users/me";

                var session = (UserLogin)Session[Contans.USER_SESSION];
                var reuslt = _connectBus.GetById(session.ID);

                var response = RequestApi(Api.BASE_URL + url, reuslt);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    try
                    {
                        var data = JsonConvert.DeserializeObject<ZoomInfo>(response.Content);

                        ViewBag.Obj = data;

                        return PartialView("_ZoomInfo", data);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var refresh_token = RefreshToken(reuslt);

                    if (refresh_token == true)
                    {
                        var reuslt_2 = _connectBus.GetById(session.ID);

                        var response_2 = RequestApi(Api.BASE_URL + url, reuslt_2);
                        try
                        {
                            var data = JsonConvert.DeserializeObject<ZoomInfo>(response_2.Content);

                            ViewBag.Obj = data;

                            return PartialView("_ZoomInfo", data);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }
                    
                }
                else
                {
                    return PartialView("_ConnectError");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return PartialView("_ConnectError");
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

        public static bool RefreshToken(ZoomConnect reuslt)
        {
            try
            {
                var client = new RestSharp.RestClient("https://zoom.us/oauth/token");

                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "Basic " + OAuth.TOKEN);

                request.RequestFormat = DataFormat.Json;
                request.AddParameter("grant_type", "refresh_token");
                request.AddParameter("refresh_token", reuslt.Refresh_token);

                IRestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return false ;
                }
                else
                {
                    var data = JsonConvert.DeserializeObject<ZoomConnect>(response.Content);

                    var result = new ZoomConnect()
                    {
                        UserId = reuslt.UserId,
                        Access_token = data.Access_token,
                        Token_type = data.Token_type,
                        Refresh_token = data.Refresh_token,
                        Expires_in = data.Expires_in,
                        Scope = data.Scope,
                        Status = data.Status,
                    };
                    decimal push = 0;
                    try
                    {
                        push = _connectBus.Update(result);
                        if (push != -1) return true;
                        return false;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}