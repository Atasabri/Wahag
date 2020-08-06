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
    public class ProductsController : Controller
    {
        private DB db = new DB();

        // GET: Products
        public ActionResult Index()
        {
            int ID = int.Parse(User.Identity.Name);
            bool access_products = db.Admins.Find(ID).Access_Products;
            if (!access_products)
            {
                return RedirectToAction("Dashboard","Admins");
            }
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Name_EN,Model,Description,Description_EN,Discount,Price,TotalPrice,Cat_ID")] Product product,HttpPostedFileBase Photo)
        {
            try
            {
                if (ModelState.IsValid && Photo != null)
                {
                    product.TotalPrice = Methods.GetPriceAfterDiscount(product.Price, product.Discount);
                    db.Products.Add(product);
                    db.SaveChanges();
                    Photo.SaveAs(Server.MapPath("~/Uploads/Products/" + product.ID + ".jpg"));
                    TempData["Done"] = "تم اضافة المنتج بنجاح .. شكرا لك";
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                TempData["error"] = "حدث خطأ اثناء اضافة المنتج .. برجاء المحاولة مرة اخري";
            }
            return RedirectToAction("Index");
        }

        // POST: Products/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Name,Name_EN,Model,Description,Description_EN,Discount,Price,TotalPrice,Cat_ID")] Product product,HttpPostedFileBase Photo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    product.TotalPrice = Methods.GetPriceAfterDiscount(product.Price, product.Discount);
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    if (Photo != null)
                    {
                        Photo.SaveAs(Server.MapPath("~/Uploads/Products/" + product.ID + ".jpg"));
                    }
                    TempData["Done"] = "تم تعديل المنتج بنجاح .. شكرا لك";
                }
                else
                {
                    throw new Exception();
                }
            }catch
            {
                TempData["error"] = "حدث خطأ اثناء تعديل المنتج .. برجاء المحاولة مرة اخري";
            }
            return RedirectToAction("Index");
        }

        // POST: Products/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
                FileInfo F = new FileInfo(Server.MapPath("~/Uploads/Products/" + id + ".jpg"));
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

        //Get Product Details
        public JsonResult Product(int id)
        {
            var product = db.Products.Where(x => x.ID == id).Select(x => new
            {
                x.ID,
                x.Name,
                x.Name_EN,
                x.Cat_ID,
                x.Price,
                x.Discount,
                x.TotalPrice,
                x.Model,
                x.Description,
                x.Description_EN,
                OrdersCount = x.Order_Details.Count,
                FavoritesCount=x.Favorites.Count,
                CatName= x.Category.Name,
            }).FirstOrDefault();
            return Json(product);
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
