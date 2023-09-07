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

        [HttpGet]
        public ActionResult UserDetails()
        {
            var userDetails = _DbContext.UserDetailsSp().ToList();
            return Json(userDetails, JsonRequestBehavior.AllowGet);
        }

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

        [HttpPost]
        public ActionResult DeleteUser(int userId)
        {
            var userDetails = _DbContext.UserRegisters.FirstOrDefault(x => x.Id == userId);
            if (userDetails != null)
            {
                _DbContext.UserRegisters.Remove(userDetails);
                _DbContext.SaveChanges();
                return Content("Delete");
            }
            return Content("Fail");
        }

        [HttpPost]
        public ActionResult EditUserDetails(UserDetailsView model)
        {
            var userDetails = _DbContext.UserRegisters.FirstOrDefault(x => x.Id == model.Id);

            if (userDetails != null)
            {
                userDetails.UserName = model.UserName;
                userDetails.EPFNumber = model.EPFNumber;
                userDetails.Nic = model.Nic;
                userDetails.Address = model.Address;
                _DbContext.SaveChanges();
            }
            return Content("Fail");
        }



    }
}