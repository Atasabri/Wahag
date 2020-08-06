using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dahab.Models;

namespace Dahab.Controllers
{
    [Authorize]
    public class CodesController : Controller
    {
        DB db = new DB();
        // GET: Codes
        public ActionResult Index()
        {
            int ID = int.Parse(User.Identity.Name);
            bool access_codes = db.Admins.Find(ID).Access_Codes;
            if (!access_codes)
            {
                return RedirectToAction("Dashboard","Admins");
            }
            return View(db.Codes.ToList());
        }

        [HttpPost]
        public JsonResult GenerateCode(double discount)
        {
            if(discount>100)
            {
                return Json("قم بادخال قيمة صحيحة للخصم");
            }
            Code code = new Code
            {
                Discount=discount,
                Code1= Methods.GetRandom()
            };
            db.Codes.Add(code);
            db.SaveChanges();
            return Json(code.Code1);
        }
        [HttpPost]
        public JsonResult Delete(int id)
        {
            try
            {
                Code code = db.Codes.Find(id);
                db.Codes.Remove(code);
                db.SaveChanges();
                return Json(id);
            }
            catch
            {
                return Json(0);
            }
        }
    }
}