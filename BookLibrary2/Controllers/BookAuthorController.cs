using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Routing;
using BookLibrary2.Models;

namespace BookLibrary2.Controllers
{
    
    public class BookAuthorController : Controller
    {
       [HttpGet]
        public async Task<ActionResult> Index(long  idBook)
       {
            await CheckSetRights();
            DataTable booksauthors = await (new BookAuthor()).GetBooksAuthors(idBook);           
            if (booksauthors.Rows.Count == 0)
            {
                booksauthors = new DataTable();
                booksauthors.Columns.Add("idBook");
                booksauthors.Rows.Add(idBook);
            }           
            return View(booksauthors);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAbsentBooksAuthors(long idBook)
        {
            ViewData["idBook"] = idBook;
            return View(await(new BookAuthor()).GetAbsentBooksAuthors(idBook));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Create(long idBook, long idAuthor)
        {
            await (new BookAuthor()).Add(idBook, idAuthor);
            return RedirectToAction("Index", new RouteValueDictionary { { "idBook", idBook } });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Delete(long idBook, long idAuthor)
        {
            await (new BookAuthor()).DeleteAsync(idBook, idAuthor);
            return RedirectToAction("Index", new RouteValueDictionary {{"idBook", idBook}});        
        }


        //private async Task<ActionResult> GetUserRout()
        //{
        //    ActionResult result = null;
        //    if (!(await AdminLogin.CheckLoginAdminAccess(User.Identity.Name)))
        //    {
        //        result = RedirectToAction("Index", "Book");
        //    }
        //    return result;
        //}

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
                    User us = await (new User()).GetByEmailAsync(login);
                    ViewData["idUser"] = us.IdUser;
                }
            }
        }
    }
}
