using InventoryManagmentSystem.BLL;
using InventoryManagmentSystem.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static InventoryManagmentSystem.BLL.AuthBLL;

namespace InventoryManagmentSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly InventorySystemEntities1 _DbContext;

        private const string PATH_PROFILE_PHOTO_UPLOAD = "C:\\Users\\dananjanaA\\Desktop\\InventoryManagmentSystem\\InventoryManagmentSystem\\Img\\ProfileImg";

        public AuthController(InventorySystemEntities1 inventorySystemEntities)
        {
            this._DbContext = inventorySystemEntities;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserRegister()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(400, "Bad Request");
            }

            // Check if the user with the provided EPFNumber exists in the database.
            var isUser = await _DbContext.UserRegisters.FirstOrDefaultAsync(x => x.EPFNumber == loginViewModel.EpfNumber);
            if (isUser == null) // If the user is not found in the database.
            {
                TempData["Message"] = "EPF Number is not registered!";
                return RedirectToAction("Index", "Home");
            }
            if (!string.IsNullOrEmpty(loginViewModel.Password))
            {
                var salt = isUser.SaltPass;
                var password = BLL.AuthBLL.RandomStringGenerator.ComputeSHA256Hash(loginViewModel.Password);
                var saltPass = password + salt;
                if (isUser.Password == saltPass)
                {
                    DateTime currentDate = DateTime.Now;
                    string Value = RandomStringGenerator.GenerateRandomString(20);

                    var isSession = await _DbContext.UserSessions.FirstOrDefaultAsync(x => x.UserId == isUser.Id);
                    if (isSession != null)
                    {
                        isSession.SessionKey = Value;
                        await _DbContext.SaveChangesAsync(); // Update the existing session key. 
                    }
                    else
                    {
                        UserSession sesstion = new UserSession
                        {
                            UserId = isUser.Id,
                            SessionKey = Value,
                            SessionDate = currentDate,
                            expireDate = currentDate.AddDays(1)
                        };
                        _DbContext.UserSessions.Add(sesstion);
                        await _DbContext.SaveChangesAsync(); // Create a new session record.
                    }

                    HttpCookie cookie = new HttpCookie("Session", Value)
                    {
                        Expires = DateTime.Now.AddDays(1)
                    };
                    Response.Cookies.Add(cookie);
                    return RedirectToAction("Main", "Main");
                }
                else
                {
                    TempData["Message"] = "Password is not correct!";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                TempData["Message"] = "Check Password!";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<ActionResult> RegisterNewUser(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                return new HttpStatusCodeResult(400, "Bad Request");
            }
            var isEpfNumber = await _DbContext.UserRegisters.FirstOrDefaultAsync(x => x.EPFNumber == userViewModel.EPFNumber);
            if (isEpfNumber != null)
            {
                TempData["Message"] = "User already exists";
                return RedirectToAction("UserRegister", "Auth");
            }

            // Upload User Profile      
            string NewFileNameWithType = UserBLL.RenameImage(userViewModel.Profile, PATH_PROFILE_PHOTO_UPLOAD);
            if (NewFileNameWithType == null)
            {
                TempData["Message"] = "Set the image";
                return RedirectToAction("UserRegister", "Auth");
            }

            if (!string.IsNullOrEmpty(userViewModel.Password) && userViewModel.Password == userViewModel.ConfirmPassword)
            {
                string salt = RandomStringGenerator.ComputeSHA256Hash(RandomStringGenerator.GenerateRandomString(5));
                string modifyPass = RandomStringGenerator.ComputeSHA256Hash(userViewModel.Password);
                var userDetails = new UserRegister
                {
                    UserName = userViewModel.UserName,
                    EPFNumber = userViewModel.EPFNumber,
                    Nic = userViewModel.Nic,
                    Address = userViewModel.Address,
                    RegisterDate = DateTime.Now,
                    UserStatus = 1,
                    SaltPass = salt,
                    Password = modifyPass + salt,
                    ProfileImgName = NewFileNameWithType
                };

                _DbContext.UserRegisters.Add(userDetails);
                await _DbContext.SaveChangesAsync();
                TempData["Message"] = "New User Saved Success!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Message"] = "Password is not Metch!";
                return RedirectToAction("UserRegister", "Auth");
            }
        }

        public ActionResult RemoveCookie()
        {
            string value = string.Empty;
            HttpCookie cookie = new HttpCookie("Session", value)
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            Response.Cookies.Add(cookie);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult GetTheUserDetailsBySession(string session)
        {
            if (session != null)
            {
                var isSession = _DbContext.UserSessions.FirstOrDefault(x => x.SessionKey == session);
                var isUserDetails = _DbContext.UserRegisters.FirstOrDefault(x => x.Id == isSession.UserId);
                var userDetails = new
                {
                    UserName = isUserDetails.UserName,
                    ImageName = isUserDetails.ProfileImgName
                };
                return Json(userDetails, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        //http://localhost:55811/Auth/GetImage?imageName=profile%20(1)_dd9e4fde-e176-4ecd-a38c-825199bcc2bb.png example url
        //<img src = "@Url.Action("GetImage", "YourController", new { imageName = "example.jpg" })" alt="Image">

        [HttpGet]
        public ActionResult GetImage(string imageName)
        {
            string imagePath = Path.Combine(PATH_PROFILE_PHOTO_UPLOAD, imageName);
            if (System.IO.File.Exists(imagePath))
            {
                return File(imagePath, "image/jpeg");
            }
            else
            {
                return Content("Image not found");
            }
        }
    }
}