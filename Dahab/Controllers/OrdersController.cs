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
    public class OrdersController : Controller
    {
        private DB db = new DB();

        // GET: Orders
        public ActionResult Index()
        {
            int ID = int.Parse(User.Identity.Name);
            bool access_orders = db.Admins.Find(ID).Access_Orders;
            if (!access_orders)
            {
                return RedirectToAction("Dashboard", "Admins");
            }
            var orders = db.Orders.Include(o => o.User);
            return View(orders.ToList());
        }

        // POST: Orders/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                Order order = db.Orders.Find(id);
                db.Orders.Remove(order);
                db.SaveChanges();
                return Json(id);
            }
            catch
            {
                return Json(0);
            }
        }
        //Arrive Order
        [HttpPost]
        public JsonResult Arrive(int id)
        {
            try
            {
                Order order = db.Orders.Find(id);
                order.IsArriveToUser = true;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return Json(id);
            }
            catch
            {
                return Json(0);
            }
        }

        public ActionResult Order(int id)
        {
            var order = db.Orders.Where(x => x.ID == id).AsEnumerable().Select(x => new
            {
                x.ID,
                Date=x.Date.ToString("dd-MM-yyy | hh:mm"),
                x.Name,
                x.User_ID,
                x.UseVisa,
                x.Place,
                x.City,
                x.TotalPrice,
                x.Discount,
                x.DiscountType,
                x.FinalPrice,
                x.IsArriveToUser,
                x.Lat,
                x.Log,
                x.Phone,
                UserName= x.User.Name,
                Details= x.Order_Details.Select(o =>new
                {
                    o.ID,
                    o.Product.Name,
                    o.Product_ID,
                    o.Price,
                    o.Count,
                    o.Message
                })
            }).FirstOrDefault();
            return Json(order);
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
