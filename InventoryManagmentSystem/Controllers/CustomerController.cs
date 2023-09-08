using InventoryManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace InventoryManagmentSystem.Controllers
{

    public class CustomerController : Controller
    {
        private readonly InventorySystemEntities1 _DbContext;

        public CustomerController(InventorySystemEntities1 inventorySystemEntities)
        {
            this._DbContext = inventorySystemEntities;
        }

        // save new customer
        public ActionResult SaveCustomer(CustomerView model)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(400, "Bad Request");
            }

            //Check customer phone number 
            var isCustomer = _DbContext.CustomeDetails.FirstOrDefault(x => x.CustomerContactInfo == model.PhoneNumber);
            if (isCustomer == null)
            {
                var CustomerReg = new CustomeDetail
                {
                    CustomerName = model.CustomerName,
                    CustomerContactInfo = model.PhoneNumber,
                    CustomerAddress = model.CustomerAddress,
                    CreatDate = DateTime.Now
                };

                _DbContext.CustomeDetails.Add(CustomerReg);
                _DbContext.SaveChanges();
            }
            return Content("Already Exit");
        }

        // get customer details
        [HttpGet]
        public async Task<ActionResult> GetCustomerDetails()
        {
            var isCustomerDetails = await _DbContext.CustomeDetails.ToListAsync();
            if (isCustomerDetails != null)
            {
                return Json(isCustomerDetails);
            }
            return new HttpStatusCodeResult(400, "Bad Request");
        }

        // get customer details OrderBy Id
        [HttpGet]
        public ActionResult GetCustomerDetailsById(int Id)
        {
            var list = (from cus in _DbContext.CustomeDetails
                        where cus.Id == Id
                        select new CustomerView
                        {
                            CustomerId = cus.Id,
                            CustomerName = cus.CustomerName,
                            CustomerAddress = cus.CustomerAddress,
                            PhoneNumber = cus.CustomerContactInfo
                        }).FirstOrDefault();
            if (list != null)
            {
                return Json(list, JsonRequestBehavior.AllowGet);

            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditCustomerDetails(CustomerView model)
        {
            var isCustomer = _DbContext.CustomeDetails.FirstOrDefault(x => x.Id == model.CustomerId);

            if (isCustomer != null)
            {
                isCustomer.CustomerName = model.CustomerName;
                isCustomer.CustomerContactInfo = model.PhoneNumber;
                isCustomer.CustomerAddress = model.CustomerAddress;
                isCustomer.CreatDate = DateTime.Now;
                _DbContext.SaveChanges();
                return Content("Success");
            }
            return Content("Faild");
        }
    }

}
