using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
namespace Dahab.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public string GetCity(string Location)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load("https://maps.googleapis.com/maps/api/geocode/xml?latlng=" + Location + "&key=AIzaSyAk8yVESKLL_UqDwN1hMQ7_G_hZ-vGdC0M");

            XmlNodeList xNodelst = xDoc.GetElementsByTagName("result");
            XmlNode xNode = xNodelst.Item(0);
            string adress = xNode.SelectSingleNode("formatted_address").InnerText;
            string[] Words = adress.Split(',');
            string city =string.Join("", Words[Words.Length-2].Where(x=>char.IsLetter(x)).ToArray()) + "," + Words[Words.Length-1];
            return city;
        }
    }
}
