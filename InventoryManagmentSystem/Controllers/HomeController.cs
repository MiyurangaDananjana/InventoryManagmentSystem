using InventoryManagmentSystem.BLL;
using InventoryManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagmentSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly InventorySystemEntities1 _DbContext;
        public HomeController(InventorySystemEntities1 inventorySystemEntities)
        {
            this._DbContext = inventorySystemEntities;
        }

        public ActionResult Index()
        {
            string Key = "Session";
            var Cookie = Request.Cookies[Key];
            UserBLL user = new UserBLL(_DbContext);
            string session = Cookie?.Value;
            if (user.CheckSession(session))
            {
                return RedirectToAction("Main", "Main");
            }
            else
            {
                return View();
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}