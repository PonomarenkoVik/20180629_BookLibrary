using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BookLibrary2.Models;

namespace BookLibrary2.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            ActionResult result = await GetUserRout();
            if (result == null)
            {
                result = View(await (new User()).GetAllAsync());
            }
            return result;
        }

       
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ActionResult result = await GetUserRout();
            if (result == null)
            {
                result = View(new User());
            }
            return result;
        }


        [HttpPost]
        public async Task<ActionResult> Create(User user)
        {
            ActionResult result = RedirectToAction("Create");
            if (ModelState.IsValid)
            {
                bool emailIsExists = await (new RegisterModel()).CheckEmail(user.Email);
                if (!emailIsExists)
                {
                    await user.CreateAsync();
                    result = RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Email", "User with such an email already exists");
                }
               
            }            
            return result;
        }

        [HttpGet]
        public async Task<ActionResult> Edit(long idUser)
        {
            ActionResult result = await GetUserRout();
            if (result == null)
            {
                User user = await (new User()).GetByIdAsync(idUser);
                if (user != null)
                {
                    result = View(user);
                }
                else
                {
                    result = RedirectToAction("Index");
                }
            }
            return result;
        }

    
        [HttpPost]
        public async Task<ActionResult> Edit(User user)
        {
            ActionResult result = RedirectToAction("Edit", new RouteValueDictionary { { "idUser", user.IdUser } }); ;
            if (ModelState.IsValid)
            {
                bool emailIsExists = await (new RegisterModel()).CheckEmail(user.Email);
                string oldEmail = (await (new User()).GetByIdAsync(user.IdUser)).Email;
                if (!emailIsExists)
                {
                    await user.UpdateAsync();
                    result = RedirectToAction("Index");
                }
                else
                {
                    if (user.Email == oldEmail)
                    {
                        result = RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("Email", "User with such an email already exists");
                    }
                    
                }
                
            }            
            return result;          
        }

        [HttpGet]
        public async Task<ActionResult> ReceiveAbsentBooks(long idUser)
        {
            string login = User.Identity.Name;
            ViewData["isAdmin"] = await AdminLogin.CheckLoginAdminAccess(login);
            ViewData["idUser"] = idUser;
            return View(await (new User()).GetAbsentBooksAsync(idUser));
        }

        [HttpGet]
        public async Task<ActionResult> Receive(long idUser, long idBook)
        {
            await (new User()).ReceiveTheBook(idUser, idBook);
            return RedirectToAction("ReceiveAbsentBooks", new RouteValueDictionary { { "idUser", idUser } });
        }

        [HttpGet]
        public async Task<ActionResult> GetUserBooks(long idUser)
        {
            string login = User.Identity.Name;
            ViewData["isAdmin"] = await AdminLogin.CheckLoginAdminAccess(login);
            ViewData["idUser"] = idUser;
            return View(await (new User()).GetUserBooks(idUser));
        }

        [HttpGet]
        public async Task<ActionResult> Return(long idUser, long idBook)
        {
            await (new User()).Return(idUser, idBook);
            return RedirectToAction("GetUserBooks", new RouteValueDictionary { { "idUser", idUser } });
        }


        private async Task<ActionResult> GetUserRout()
        {
            ActionResult result = null;
            if (!(await AdminLogin.CheckLoginAdminAccess(User.Identity.Name)))
            {
                result = RedirectToAction("Index", "Book");
            }
            return result;
        }
    }    
}
