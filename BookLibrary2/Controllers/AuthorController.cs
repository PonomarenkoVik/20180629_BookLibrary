using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using BookLibrary2.Models;

namespace BookLibrary2.Controllers
{
   
    public class AuthorController : Controller
    {

        [HttpGet]
        public async Task<ActionResult> Index(int pageNumber = 1, string searchString = null)
        {

            await CheckSetRights();
            DataTable table;
            do
            {
                table = await new Author().GetByPageAsync(pageNumber, searchString);
                if (table.Rows.Count == 0 && pageNumber > 1)
                {
                    pageNumber -= 1;
                    table = await (new Author()).GetByPageAsync(pageNumber, searchString);
                }

            } while (pageNumber > 1 && table.Rows.Count == 0);

            if (!string.IsNullOrEmpty(searchString))
            {
                ViewData["searchString"] = searchString;
            }
            else
            {
                ViewData["searchString"] = null;
            }

            ViewData["pageNumber"] = pageNumber;
            ViewData["pageSize"] = Book.PageSize;
            return View(table);           
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ActionResult result = await GetUserRout();
            if (result == null)
            {
                result = View(new Author());
            }
            return result;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(Author author)
        {
            ActionResult result;
            if (ModelState.IsValid)
            {
                await author.CreateAsync();
                result = RedirectToAction("Index");
            }
            else
            {
                result = RedirectToAction("Create");
            }
            return result;
          
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Edit(int idAuthor)
        {
            ActionResult result = await GetUserRout();
            if (result == null)
            {
                Author author = await (new Author()).GetByIdAsync(idAuthor);
                if (author != null)
                {
                    result = View(author);
                }
                else
                {
                    result = RedirectToAction("Index");
                }
            }
            return result;                   
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Edit(Author author)
        {
            ActionResult result;
            if (ModelState.IsValid)
            {
                await author.UpdateAsync();
                result = RedirectToAction("Index");
            }
            else
            {
                result = RedirectToAction("Edit", new RouteValueDictionary { { "idAuthor", author.IdAuthor } });
            }
            return result;       
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
    
        private async Task CheckSetRights()
        {
            if (User.Identity.IsAuthenticated)
            {
                string login = User.Identity.Name;
                bool isAdmin = await AdminLogin.CheckLoginAdminAccess(login);
                if (isAdmin)
                {
                    ViewData["isAdmin"] = true;
                }
                else
                {
                    User us = await new User().GetByEmailAsync(login);
                    ViewData["idUser"] = us.IdUser;
                }
            }
        }
    }
}
