using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dahab.Models;
using System.IO;

namespace Dahab.Controllers
{
    [Authorize]
    public class AdminsController : Controller
    {
        private DB db = new DB();

        // GET: Admins
        public ActionResult Index()
        {
            int ID = int.Parse(User.Identity.Name);
            bool IsManager = db.Admins.Find(ID).IsManager;
            if(!IsManager)
            {
                return RedirectToAction("Dashboard");
            }
            return View(db.Admins.ToList());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserName,Password,IsManager,Access_Codes,Access_Categories,Access_Products,Access_Users,Access_Orders")] Admin admin,HttpPostedFileBase Photo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Admins.Add(admin);
                    db.SaveChanges();
                    if (Photo != null)
                    {
                        Photo.SaveAs(Server.MapPath("~/Uploads/Admins/" + admin.ID + ".jpg"));
                    }
                    TempData["Done"] = "تم الاضافة بنجاح .. شكرا لك";
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                TempData["error"] = "حدث خطأ في الاضافة .. برجاء المحاولة مرة اخري";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,UserName,Password,IsManager,Access_Codes,Access_Categories,Access_Products,Access_Users,Access_Orders")] Admin admin,HttpPostedFileBase Photo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                    if (Photo != null)
                    {
                        Photo.SaveAs(Server.MapPath("~/Uploads/Admins/" + admin.ID + ".jpg"));
                    }
                    if(admin.ID==int.Parse(User.Identity.Name))
                    {
                        return RedirectToAction("Login", "Auth");
                    }
                    TempData["Done"] = "تم التعديل بنجاح .. شكرا لك";
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                TempData["error"] = "حدث خطأ في التعديل .. برجاء المحاولة مرة اخري";
            }
            return RedirectToAction("Index");
        }
        // POST: Admins/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                Admin admin = db.Admins.Find(id);
                db.Admins.Remove(admin);
                db.SaveChanges();
                FileInfo F = new FileInfo(Server.MapPath("~/Uploads/Admins/" + id + ".jpg"));
                if (F.Exists)
                {
                    F.Delete();
                }
                return Json(id);
            }
            catch
            {
                return Json(0);
            }
        }
        //Get Admin Data For Edit And Details
        [HttpPost]
        public JsonResult Admin(int id)
        {
            var admin = db.Admins.Where(x => x.ID == id).Select(x =>new
            {
                x.ID,
                x.UserName,
                x.Password,
                x.IsManager,
                x.Access_Users,
                x.Access_Products,
                x.Access_Orders,
                x.Access_Codes,
                x.Access_Categories
            }).FirstOrDefault();
            return Json(admin);
        }
        //Check Is User Name Is Used (For Adding)
        public JsonResult CheckAdd(string username)
        {
            if(db.Admins.Any(x=>x.UserName==username))
            {
                return Json(0);
            }
            return Json(1);
        }
        //Check Is User Name Is Used (For Editing)
        public JsonResult CheckEdit(string username,int id)
        {
            if (db.Admins.Where(x=>x.ID!=id).Any(x => x.UserName == username))
            {
                return Json(0);
            }
            return Json(1);
        }

        //Dashboard
        public ActionResult Dashboard()
        {
            return View();
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
