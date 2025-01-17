﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Models.User;

namespace SmartSchool.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        public static string USER_SESSION = "USER_SESSION";

        // GET: Admin/Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var sess = (UserLogin)Session[USER_SESSION];
            if (sess == null)
            {
                filterContext.Result =
                    new RedirectToRouteResult(
                        new RouteValueDictionary(new
                            { controller = "Login", action = "Index", Area = "Admin" }));
            }

            base.OnActionExecuting(filterContext);


        }
    }
}