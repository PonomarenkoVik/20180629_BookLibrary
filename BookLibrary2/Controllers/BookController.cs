using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.UI.WebControls;
using BookLibrary2.Models;

namespace BookLibrary2.Controllers
{

    public class BookController : Controller
    {
        
        [HttpGet]
        public async Task<ActionResult> Index(int pageNumber = 1, string searchString = null)
        {          
            await CheckSetRights(); 
            DataTable table;
            do
            {
                table = await (new Book()).GetBooksByPage(pageNumber, searchString);
                if (table.Rows.Count == 0 && pageNumber > 1)
                {
                    pageNumber -= 1;
                    table = await (new Book()).GetBooksByPage(pageNumber, searchString);
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

       

        [HttpGet]
        public async Task<ActionResult> BooksByAuthor(long idAuthor, int pageNumber = 1)
        {
            await CheckSetRights();
            DataTable table;
            do
            {
                table = await (new Book()).GetBooksByAuthor(idAuthor, pageNumber);
                if (table.Rows.Count == 0 && pageNumber > 1)
                {
                    pageNumber -= 1;
                    table = await (new Book()).GetBooksByAuthor(idAuthor, pageNumber);
                }

            } while (pageNumber > 1 && table.Rows.Count == 0);
            ViewData["idAuthor"] = idAuthor;
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
               result = View(new Book());
            }
            return result;
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(Book book)
        {
            ActionResult result;
            if (ModelState.IsValid)
            {
                await book.CreateAsync();
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
        public async Task<ActionResult> Edit(long id)
        {
            ActionResult result = await GetUserRout();
            if (result == null)
            {
                Book book = await (new Book()).GetByIdAsync(id);
                if (book != null)
                {
                    result = View(book);
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
        public async Task<ActionResult> Edit(Book book)
        {
            ActionResult result;
            if (ModelState.IsValid)
            {
                await book.UpdateAsync();
                result = RedirectToAction("Index");
            }
            else
            {
                result = RedirectToAction("Edit", new RouteValueDictionary { { "idBook", book.IdBook } });
            }
            return result;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Delete(long id)
        {
            ActionResult result = await GetUserRout();
            if (result == null)
            {
                Book book = new Book();
                await book.DeleteAsync(id);
                result = RedirectToAction("Index");
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
