using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dahab.Models;

namespace Dahab.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private DB db = new DB();

        // GET: Users
        public ActionResult Index()
        {
            int ID = int.Parse(User.Identity.Name);
            bool access_users = db.Admins.Find(ID).Access_Users;
            if (!access_users)
            {
                return RedirectToAction("Dashboard", "Admins");
            }
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            int ID = int.Parse(User.Identity.Name);
            bool access_users = db.Admins.Find(ID).Access_Users;
            if (!access_users)
            {
                return RedirectToAction("Dashboard", "Admins");
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        // POST: Users/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                User user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return Json(id);
            }
            catch
            {
                return Json(0);
            }
        }
        [HttpPost]
        public ActionResult SendMail(string Subject, HttpPostedFileBase File, List<string> check)
        {
            if (check != null && check.Count() >= 1 && File != null)
            {
                List<string> Users = check.Distinct().ToList();
                Methods.Send_Mail(Subject, File, Users);
            }

            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
