using InventoryManagmentSystem.BLL;
using InventoryManagmentSystem.DAL;
using InventoryManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;

namespace InventoryManagmentSystem.Controllers
{
    public class MainController : Controller
    {
        private readonly InventorySystemEntities1 _DbContext;

        public MainController(InventorySystemEntities1 inventorySystemEntities)
        {
            this._DbContext = inventorySystemEntities;
        }

        public ActionResult Main()
        {
            string Key = "Session";
            var Cookie = Request.Cookies[Key];
            UserBLL user = new UserBLL(_DbContext);
            string session = Cookie?.Value;
            if (user.CheckSession(session))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }

        }

        public ActionResult Dashbord()
        {
            var viewModel = new DashboardViewModel
            {
                TotalCustomers = _DbContext.CustomerDetails.Count(),
                TotalProductVariants = _DbContext.ProductVariantes.Count(),
                TotalProducts = _DbContext.Products.Count(),
                TotalSystemUsers = _DbContext.UserRegisters.Count()
            };
            return View(viewModel);
        }

        public ActionResult SystemUsersView()
        {
            return View();
        }

        public ActionResult Customer()
        {
            return View();
        }

        public ActionResult Supplier()
        {
            return View();
        }

        public ActionResult Brand()
        {
            return View();
        }

        public ActionResult Product()
        {
            return View();
        }

        public ActionResult ProductVariant()
        {
            return View();
        }
        public ActionResult OrderItem()
        {
            return View();
        }
        public ActionResult InvoiceView(int invoiceIds)
        {
            if (invoiceIds > 0)
            {
                var query = from invoice in _DbContext.Invoices
                            join customer in _DbContext.CustomerDetails on invoice.CustomerId equals customer.CustomerId
                            where invoice.InvoiceId == invoiceIds
                            select new CustomerDetailsModel
                            {
                                CustomerName = customer.CustomerName,
                                CustomerPhoneNumber = customer.CustomerPhoneNumber,
                                CustomerAddress = customer.CustomerAddress,
                            };
                var result = query.SingleOrDefault();

                var invoiceIdToSearch = invoiceIds;

                var list = from i in _DbContext.Invoices
                           from o in _DbContext.OrderItems
                           where o.CustomerId == i.CustomerId && o.CreateDate == i.Date
                           join b in _DbContext.Brands on o.BrandId equals b.BrandId
                           join p in _DbContext.Products on o.ProductId equals p.ProductId
                           join pv in _DbContext.ProductVariantes on o.ProductVariantId equals pv.ProductVariantId
                           where i.InvoiceId == invoiceIdToSearch
                           select new InvoiceItemModel
                           {
                               InvoiceId = i.InvoiceId,
                               BrandName = b.BrandName,
                               ProductName = p.ProductName,
                               Description = pv.Description,
                               Price = (decimal)p.Price,
                               TotalQuantity = i.TotalQuantity,
                               TotalPrice = (decimal)i.TotalPrice
                           };
                var Listresult = list.ToList();
                var viewModel = new InvoiceViewModel
                {
                    CustomerDetails = result,
                    InvoiceItems = Listresult
                };
                return View(viewModel);
            }
            return Content("tets");
        }
        //get user details 
        [HttpGet]
        public ActionResult UserDetails()
        {
            var userDetails = _DbContext.UserDetailsSp().ToList();
            return Json(userDetails, JsonRequestBehavior.AllowGet);
        }

        //Get user details order by the User-Id
        [HttpGet]
        public ActionResult GetUserDetailsById(int Id)
        {
            var query = (from user in _DbContext.UserRegisters
                         where user.Id == Id
                         select new UserDetailsView
                         {
                             Id = user.Id,
                             UserName = user.UserName,
                             EPFNumber = user.EPFNumber,
                             Nic = user.Nic,
                             Address = user.Address
                         }).FirstOrDefault();
            if (query != null)
            {
                // Return the userRegister object in JSON format
                return Json(query, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return HttpNotFound();
            }
        }

        //User Details Delete
        [HttpPost]
        public ActionResult DeleteUser(int userId)
        {
            try
            {
                var isSession = _DbContext.UserSessions.FirstOrDefault(x => x.UserId == userId);
                if (isSession != null)
                {
                    _DbContext.UserSessions.Remove(isSession);
                    _DbContext.SaveChanges();
                    var userDetails = _DbContext.UserRegisters.FirstOrDefault(x => x.Id == userId);
                    if (userDetails != null)
                    {
                        _DbContext.UserRegisters.Remove(userDetails);
                        _DbContext.SaveChanges();
                        return Content("Delete");
                    }
                    return Content("Fail");
                }
                else
                {
                    var userDetails = _DbContext.UserRegisters.FirstOrDefault(x => x.Id == userId);
                    if (userDetails != null)
                    {
                        _DbContext.UserRegisters.Remove(userDetails);
                        _DbContext.SaveChanges();
                        return Content("Delete");
                    }
                }
                return Content("fail");
            }
            catch (DbUpdateException ex)
            {

                string errorMessage = "An error occurred while updating the entries.";
                if (ex.InnerException != null)
                {
                    errorMessage += " Inner Exception: " + ex.InnerException.Message;
                    
                }
                return Content(errorMessage);

            }
           
        }

        //User Details Edit
        [HttpPost]
        public ActionResult EditUserDetails(UserDetailsView model)
        {
            UserDAL user = new UserDAL(_DbContext);
            int userId = user.GetUserIdBySessionKey(model.SessionKey);
            if (userId > 0)
            {
                var userDetails = _DbContext.UserRegisters.FirstOrDefault(x => x.Id == model.Id);
                if (userDetails != null)
                {
                    userDetails.UserName = model.UserName;
                    userDetails.EPFNumber = model.EPFNumber;
                    userDetails.Nic = model.Nic;
                    userDetails.Address = model.Address;
                    userDetails.LastUpdateUserId = userId;
                    _DbContext.SaveChanges();
                    return Content("success");
                }
                return Content("Fail");
            }
            return Content("Fail");
        }
    }
}