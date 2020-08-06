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
    public class CategoriesController : Controller
    {
        private DB db = new DB();

        // GET: Categories
        public ActionResult Index()
        {
            int ID = int.Parse(User.Identity.Name);
            bool access_categories = db.Admins.Find(ID).Access_Categories;
            if (!access_categories)
            {
                return RedirectToAction("Dashboard", "Admins");
            }
            return View(db.Categories.ToList());
        }

   

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Name_EN")] Category category, HttpPostedFileBase Photo, List<int> Sizes)
        {
            try
            {
                if (ModelState.IsValid && Photo != null)
                {
                    if(Sizes!=null)
                    {
                        foreach (var item in Sizes)
                        {
                            category.Sizes.Add(new Size { Size1 = item });
                        }
                    }                   
                    db.Categories.Add(category);
                    db.SaveChanges();
                    Photo.SaveAs(Server.MapPath("~/Uploads/Categories/" + category.ID + ".jpg"));
                    TempData["Done"] = "تم اضافة هذا النوع بنجاح .. شكرا لك";
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                TempData["error"] = "حدث خطأ اثناء الاضافة .. شكرا لك";
            }
            return RedirectToAction("Index");
        }
        // POST: Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Name_EN")] Category category,HttpPostedFileBase Photo,List<int> Sizesedit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(Sizesedit!=null)
                    {
                        foreach (var item in Sizesedit)
                        {
                            db.Sizes.Add(new Size { Size1 = item ,Cat_ID=category.ID});
                        }
                    }                   
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    if (Photo != null)
                    {
                        Photo.SaveAs(Server.MapPath("~/Uploads/Categories/" + category.ID + ".jpg"));
                    }
                    TempData["Done"] = "تم تعديل هذا النوع بنجاح .. شكرا لك";
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                TempData["error"] = "حدث خطأ اثناء التعديل .. شكرا لك";
            }
            return RedirectToAction("Index");
        }

        // POST: Categories/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Category category = db.Categories.Find(id);
                db.Categories.Remove(category);

                db.SaveChanges();
                FileInfo F = new FileInfo(Server.MapPath("~/Uploads/Categories/" + id + ".jpg"));
                if(F.Exists)
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
        [HttpPost]
        public JsonResult Category(int id)
        {
            var cat = db.Categories.Where(x => x.ID == id).Select(x => new
            {
                x.ID,
                x.Name,
                x.Name_EN,
                Sizes= x.Sizes.Select(s=>new
                {
                    s.ID,
                    s.Size1
                }),
                ProductCount=x.Products.Count
            }).FirstOrDefault();

            return Json(cat);
        }
        [HttpPost]
        public JsonResult DeleteSizeFromDB(int id)
        {
            try
            {
                Size s = db.Sizes.Find(id);
                db.Sizes.Remove(s);
                db.SaveChanges();
                return Json(id);
            }
            catch
            {
                return Json(0);
            }
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
