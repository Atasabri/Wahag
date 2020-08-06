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
    public class SubscribersController : Controller
    {
        private DB db = new DB();

        // GET: Subscribers
        public ActionResult Index()
        {
            return View(db.Subscribers.ToList());
        }

        // POST: Subscribers/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                Subscriber subscriber = db.Subscribers.Find(id);
                db.Subscribers.Remove(subscriber);
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
