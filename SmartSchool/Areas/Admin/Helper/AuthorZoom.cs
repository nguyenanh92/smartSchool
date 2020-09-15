using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BLL;
using Common.Config;
using Models.ZoomConnect;
using Newtonsoft.Json;
using RestSharp;

namespace SmartSchool.Areas.Admin.Helper
{
    public static class AuthorZoom
    {
        private static ZoomConnectBus _connectBus = new ZoomConnectBus();


        //public static bool GetAuthenZoom(int userId, string url = "")
        //{

        //    return false;
        //}
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
