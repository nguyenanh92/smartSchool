using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BLL;
using Common;
using Common.Config;
using Common.ZoomConfiguration;
using Models;
using Models.User;
using Models.ZoomConnect;
using Newtonsoft.Json;
using RestSharp;

namespace SmartSchool.Areas.Admin.Helper
{
    public static class AuthorZoom
    {
        private static ZoomConnectBus _connectBus = new ZoomConnectBus();

        public static ZoomInfo GetInfoAPI(int userId)
        {
            var url = "v2/users/me";

            var reuslt = _connectBus.GetZoomConnectByUserId(userId);

            var response = AuthorZoom.RequestApi(Api.BASE_URL + url, reuslt);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                try
                {
                    var data = JsonConvert.DeserializeObject<ZoomInfo>(response.Content);

                    return data;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refresh_token = AuthorZoom.RefreshToken(reuslt, userId);

                if (refresh_token == true)
                {
                    var reuslt_2 = _connectBus.GetZoomConnectByUserId(userId);

                    var response_2 = AuthorZoom.RequestApi(Api.BASE_URL + url, reuslt_2);
                    try
                    {
                        var data = JsonConvert.DeserializeObject<ZoomInfo>(response_2.Content);

                        return data;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
                return new ZoomInfo();
            }
            else
            {
                return new ZoomInfo();
            }
        }

        public static IRestResponse CreateMeetingAPI(int userId ,string userZoomId, CreateMeetingModel model)
        {
            var url = Api.BASE_URL + "v2/users/" + userZoomId + "/meetings";

            var reuslt = _connectBus.GetZoomConnectByUserId(userId);
            var client = new RestSharp.RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + reuslt.Access_token);

            request.AddParameter("application/json", "{\"type\": " + 2 + "," +
                                                     "\"duration\":\"" + model.Duration + "\"," +
                                                     "\"password\":\"" + model.Password + "\"," +
                                                     "\"status\":\"waiting\"," +
                                                     "\"timezone\":\"Asia/Bangkok\"," +
                                                     "\"topic\":\"" + model.Title + "\"," +
                                                     "\"settings\":" +
                                                     "{\"host_video\":" + model.Host_video + "," +
                                                     "\"participant_video\":" + model.Client_video + "}}",
                                                     ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
            return response;

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

        public static bool RefreshToken(ZoomConnect reuslt, int userId)
        {
            try
            {
                var client = new RestSharp.RestClient("https://zoom.us/oauth/token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/json");
                request.AddHeader("authorization", "Basic " + OAuth.TOKEN);
                request.AddParameter("grant_type", "refresh_token");
                request.AddParameter("refresh_token", reuslt.Refresh_token);

                IRestResponse response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return false;
                }
                else
                {
                    var data = JsonConvert.DeserializeObject<ZoomConnect>(response.Content);

                    var result = new ZoomConnect()
                    {
                        UserId = userId,
                        Access_token = data.Access_token,
                        Token_type = data.Token_type,
                        Refresh_token = data.Refresh_token,
                        Expires_in = data.Expires_in,
                        Scope = data.Scope,
                        Status = "Update Token",
                    };
                    try
                    {
                        var push = _connectBus.Update(result);
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

 