using InventoryManagmentSystem.BLL;
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
            var isCustomer = _DbContext.CustomerDetails.FirstOrDefault(x => x.CustomerPhoneNumber == model.PhoneNumber);
            if (isCustomer == null)
            {
                var CustomerReg = new CustomerDetail
                {
                    CustomerName = model.CustomerName,
                    CustomerPhoneNumber = model.PhoneNumber,
                    CustomerAddress = model.CustomerAddress,
                    CrateDate = DateTime.Now
                };
                _DbContext.CustomerDetails.Add(CustomerReg);
                _DbContext.SaveChanges();
                return Json(new { success = true, message = "Successfully added a new customer" });
            }
            return Json(new { success = false, message = "Customer already exists" });
        }

        // get customer details
        [HttpGet]
        public ActionResult GetCustomerDetails()
        {
            var userDetails = _DbContext.CustomerDetails.ToList();
            // Map the data to UserDetailsModel objects
            var userDetailModels = userDetails.Select(u => new CustomerDetailsModel
            {
                CustomerId = u.CustomerId,
                CustomerName = u.CustomerName,
                CustomerPhoneNumber = u.CustomerPhoneNumber,
                CustomerAddress = u.CustomerAddress
            }).ToList();
            return Json(userDetailModels, JsonRequestBehavior.AllowGet);
        }

        // get customer details OrderBy Id
        [HttpGet]
        public ActionResult GetCustomerDetailsById(int Id)
        {
            var list = (from cus in _DbContext.CustomerDetails
                        where cus.CustomerId == Id
                        select new CustomerView
                        {
                            CustomerId = cus.CustomerId,
                            CustomerName = cus.CustomerName,
                            CustomerAddress = cus.CustomerAddress,
                            PhoneNumber = cus.CustomerPhoneNumber
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
            var isCustomer = _DbContext.CustomerDetails.FirstOrDefault(x => x.CustomerId == model.CustomerId); 
            if (isCustomer != null)
            {
                isCustomer.CustomerName = model.CustomerName;
                isCustomer.CustomerPhoneNumber = model.PhoneNumber;
                isCustomer.CustomerAddress = model.CustomerAddress;
                isCustomer.CrateDate = DateTime.Now;
                _DbContext.SaveChanges();
                return Content("Success");
            }
            return Content("Faild");
        }

        [HttpPost]
        public ActionResult DeleteCustomer(int CustomerId)
        {
            var isCustomer = _DbContext.CustomerDetails.FirstOrDefault(x => x.CustomerId == CustomerId);
            if(isCustomer != null)
            {
                _DbContext.CustomerDetails.Remove(isCustomer);
                _DbContext.SaveChanges();
                return Content("Delete");
            }
            return Content("fail");
        }
    }

}
