using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;

namespace SmartSchool.Controllers
{
    public class HomeController : Controller
    {
        private static SubjectBus _subjectBus = new SubjectBus();

        public ActionResult Index()
        {
            var lst = _subjectBus.GetById(0);
            ViewBag.Subject = lst;
            return View();
        }

       
    }
}