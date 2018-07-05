using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BookLibrary2.Models;
using System.Web.Security;

namespace BookLibrary2.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            ActionResult result = View();
            if (User.Identity.IsAuthenticated)
            {
                result = RedirectToAction( "Login", new RouteValueDictionary { { "logModel", new LoginModel() { Email = User.Identity.Name } } });
            }
            return result;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel logModel)
        {
            ActionResult result = RedirectToAction("Login");
            
            bool isAdmin = await AdminLogin.CheckLoginAdminAccess(logModel.Email);
            User us = null;
            if (!isAdmin)
            {
                us = await (new User()).GetByEmailAsync(logModel.Email);                    
            }
            if (us != null || isAdmin)
            {
                FormsAuthentication.SetAuthCookie(logModel.Email, true);
                result = RedirectToAction("Index", "Book");
            }          
            return result;
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","Book");
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            ActionResult result = RedirectToAction("Register");
            if (ModelState.IsValid)
            {
                bool emailIsExists = await (new RegisterModel()).CheckEmail(model.Email);
                if (!emailIsExists)
                {
                    User user = new User
                    {
                        FirstName = model.FirstName,
                        SecondName = model.SecondName,
                        Email = model.Email
                    };
                    await user.CreateAsync();
                    User us = await (new User()).GetByEmailAsync(model.Email);
                    if (us != null)
                    {
                        FormsAuthentication.SetAuthCookie(us.Email, true);
                        result = RedirectToAction("Index", "Book", new RouteValueDictionary { { "idUser", us.IdUser } });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User with such an email already exists");
                }
            }           
            return result;
        }

    }
}