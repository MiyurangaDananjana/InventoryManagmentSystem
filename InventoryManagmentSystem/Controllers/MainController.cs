using InventoryManagmentSystem.DAL;
using InventoryManagmentSystem.Models;
using System.Linq;
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
            return View();
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
        public ActionResult InvoiceView()
        {
            return View();
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
                // return a 404 Not Found response or another appropriate response.
            }
        }

        //User Details Delete
        [HttpPost]
        public ActionResult DeleteUser(int userId)
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