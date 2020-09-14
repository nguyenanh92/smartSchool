using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Common;
using Models;
using Models.User;

namespace SmartSchool.Controllers
{
    public class LearingController : Controller
    {
        private static SubjectBus _subjectBus = new SubjectBus();

        // GET: Learing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StartLearning(int subjectId)
        {
            //get thông tin  của lesson ở session
            List<SubjectItem> list = Session["Subjects"] as List<SubjectItem>;

            SubjectItem item = list.FirstOrDefault(a => a.Id == subjectId);


            return Redirect(item.Join_url);

        }
    }
}