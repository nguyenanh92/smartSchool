using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmartSchool
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "ZoomCallBack",
            //    url: "callback",
            //    defaults: new { controller = "Zoom", action = "CallBack" }
            //);
            //routes.MapRoute(
            //    name: "Test",
            //    url: "Test",
            //    defaults: new { controller = "Zoom", action = "Test" }
            //);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
