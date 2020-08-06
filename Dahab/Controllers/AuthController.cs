using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Dahab.Models;

namespace Dahab.Controllers
{
    public class AuthController : Controller
    {
        //DB 
        DB db = new DB();

        // GET: Login
        public ActionResult Login()
        {
                return View();
        }

        //Login Post
        [HttpPost]
        public ActionResult Login(string UserName,string Password)
        {
            Admin admin = db.Admins.SingleOrDefault(x => x.UserName == UserName && x.Password == Password);
            if(admin==null)
            {
                ViewBag.error = "برجاء التأكد من بيانات الادمن !!!";
                return View();
            }
            FormsAuthentication.SetAuthCookie(admin.ID.ToString(), false);
            return RedirectToAction("Index", "Admins", null);
        }
        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}