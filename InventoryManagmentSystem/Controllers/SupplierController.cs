using InventoryManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagmentSystem.Controllers
{
    public class SupplierController : Controller
    {
        private readonly InventorySystemEntities1 _DbContext;
        public SupplierController(InventorySystemEntities1 DbContext)
        {
            this._DbContext = DbContext;
        }

        // new new Supplier // Test Pass
        [HttpPost]
        public ActionResult NewSupplier(SupplierDetailsModel model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(400, "Bad Request");
            }
            // is check supplier
            var isSupplier = _DbContext.Suppliers.FirstOrDefault(x => x.PhoneNumber == model.PhoneNumber);
            if (isSupplier == null)
            {
                var SupplierDetrails = new Supplier
                {
                    SupplierName = model.SupplierName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    SupplierDescription = model.Description
                };
                _DbContext.Suppliers.Add(SupplierDetrails);
                _DbContext.SaveChanges();
                return Json(new { success = false, message = "Successfully added a new Supplier" });
            }
            return Json(new { success = false, message = "Supplier already exists" });
        }

        // Get Supplier Details // Test Pass
        [HttpGet]
        public ActionResult GetSupplierDetails()
        {
            var supplierDetails = _DbContext.Suppliers.ToList();
            var supplierDetailsModel = supplierDetails.Select(u => new SupplierDetailsModel
            {
                SupplierID=u.SupplierID,
                SupplierName = u.SupplierName,
                PhoneNumber = u.PhoneNumber,
                Address= u.Address,
                Description=u.SupplierDescription
            }).ToList();
            return Json(supplierDetailsModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetSupplierDetailsById(int Id)
        {
            var list = (from supplier in _DbContext.Suppliers
                        where supplier.SupplierID == Id
                        select new SupplierDetailsModel
                        {
                            SupplierID = supplier.SupplierID,
                            SupplierName= supplier.SupplierName,
                            Address= supplier.Address,
                            PhoneNumber= supplier.PhoneNumber,
                            Description= supplier.SupplierDescription
                        }).FirstOrDefault();
            if(list != null)
            {
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return HttpNotFound();
        }

        // Update by supplier details 
        [HttpPost]
        public ActionResult EditSupplierDetails(SupplierDetailsModel model)
        {
            if(!ModelState.IsValid)
            {
                return HttpNotFound();
            }
            var isSupplier =_DbContext.Suppliers.FirstOrDefault(x=>x.SupplierID == model.SupplierID);
            if(isSupplier != null)
            {
                isSupplier.SupplierName = model.SupplierName;
                isSupplier.Address = model.Address;
                isSupplier.PhoneNumber = model.PhoneNumber;
                isSupplier.SupplierDescription = model.Description; 
                _DbContext.SaveChanges();
                return Content("Success");
            }
            return Content("Faild");
        }

        //Delete Supplier // Test Pass
        [HttpPost]
        public ActionResult DeleteSupplierDetails(int SupplierID)
        {
            if(SupplierID == 0)
            {
                return new HttpStatusCodeResult(400, "Bad Request");
            }
            var isSupplier = _DbContext.Suppliers.FirstOrDefault(x => x.SupplierID == SupplierID);
            if(isSupplier != null)
            {
                _DbContext.Suppliers.Remove(isSupplier);
                _DbContext.SaveChanges();
                return Content("Delete");
            }
            return Content("Fail");

        }
    }
}