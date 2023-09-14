using InventoryManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagmentSystem.Controllers
{
    public class DashboardController : Controller
    {
        private readonly InventorySystemEntities1 _DbContext;
        public DashboardController(InventorySystemEntities1 inventorySystemEntities)
        {
            this._DbContext = inventorySystemEntities;
        }

        //[HttpGet]
        //public ActionResult GetTotal()
        //{
           

        //}
    }
}