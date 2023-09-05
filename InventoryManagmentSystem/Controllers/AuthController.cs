using InventoryManagmentSystem.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static InventoryManagmentSystem.BLL.AuthBLL;

namespace InventoryManagmentSystem.Controllers
{
    public class AuthController : Controller
    {
        private readonly InventorySystemEntities _DbContext;
        public AuthController(InventorySystemEntities inventorySystemEntities)
        {
            this._DbContext = inventorySystemEntities;
        }

        // GET: Auth
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
            var isUser = await _DbContext.UserRegisters.FirstOrDefaultAsync(x => x.EPFNumber == loginViewModel.EpfNumber);
            if (isUser == null)
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
                    CookieOptions option = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(1)
                    };

                    var isSession = await _DbContext.UserSessions.FirstOrDefaultAsync(x => x.UserId == isUser.Id);
                    if (isSession != null)
                    {
                        isSession.SessionKey = Value;
                        await _DbContext.SaveChangesAsync();
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
                        await _DbContext.SaveChangesAsync();
                    }
                    Response.Cookies.Append(Key, Value, option);

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
                    Password = modifyPass + salt
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
    }
}