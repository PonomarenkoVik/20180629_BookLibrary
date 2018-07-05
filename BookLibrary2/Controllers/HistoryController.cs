using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using BookLibrary2.Controllers;
using BookLibrary2.Models;

namespace BookLibrary2.Controllers
{
    [Authorize]
    public class HistoryController : Controller
    {
        [HttpGet]
        public async Task<ActionResult> Index(long idUser)
        {
            ViewData["isAdmin"] = await AdminLogin.CheckLoginAdminAccess(User.Identity.Name);
            return View(await (new History()).GetByUser(idUser));
        }

       [HttpGet]
        public async Task<ActionResult> BookHistory(int idBook)
        {
            return View(await (new History()).GetByBook(idBook));
        }

        //
        // GET: /History/GetAbsentBooksAuthors
        [HttpGet]
        public async Task<ActionResult> AllHistory()
        {
            return View(await (new History()).GetAll());
        }
    }
}
