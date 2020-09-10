using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Config;

namespace SmartSchool.Areas.Admin.Controllers
{
    public class ZoomController : Controller
    {

        // GET: Admin/Zoom
        public ActionResult Index()
        {
            return GetAccessToken();
        }


        public ActionResult GetAccessToken()
        {
            var uri = OAuthConfiguration.Uri;
            return Redirect(uri);
        }

    }
}