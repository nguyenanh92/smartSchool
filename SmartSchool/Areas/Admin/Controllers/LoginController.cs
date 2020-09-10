using Models.User;
using SmartSchool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SmartSchool.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        public string USER_SESSION = "USER_SESSION";

        private CheckLogin checkLogin = new CheckLogin();
        // GET: Admin/Login
        public ActionResult Index(LoginViewModel model)
        {
            if (Request.Cookies["client_cookie"] != null)
            {
                return RedirectToAction("Index", "DashBoard");
            }

            return View(new LoginViewModel { });
        }

        [HttpPost]
        [ValidateInput(true)]
        public ActionResult LoginSubmit(LoginViewModel model)
        {
            Random random;
            
            int checklogin = checkLogin.CheckUserLogin(model);
            switch (checklogin)
            {
                case 1:

                    //Đăng nhập thành công
                    var user = checkLogin.GetUser(model.username);
                    var userSession = new UserLogin();

                    userSession.Username = user.username;
                    userSession.Id = user.id;
                    Session.Add(USER_SESSION, userSession);

                    TempData["Count"] = 0;
                    TempData["Messages"] = "Login Success";
                    return RedirectToAction("Index", "DashBoard");
                case 2:
                    ViewBag.Messages = "Tài khoản đã bị khóa";
                    return View("Index");
                case -1:
                    if (TempData["Count"] == null)
                    {
                        TempData["Count"] = 1;
                        TempData.Keep("Count");
                    }
                    else
                    {
                        TempData["Count"] = int.Parse(TempData["Count"].ToString()) + 1;
                        TempData.Keep("Count");
                    }
                    if (int.Parse(TempData["Count"].ToString()) >= 5)
                    {
                        random = new Random();
                        int iNumber = random.Next(10000, 99999);

                        ViewBag.Message = "Nhập sai quá nhiều ! Vui lòng đợi";
                        return View("Index");

                    }

                    ViewBag.Messages = "Tên đăng nhập hoặc mật khẩu không đúng";
                    return View("Index", model);

            }
            return View("Index");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Remove("USER_SESSION");

            return RedirectToAction("Index", "Login");
        }
    }
}