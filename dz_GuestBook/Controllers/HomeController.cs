using dz_GuestBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace dz_GuestBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly GuestBookContext _context;

        public HomeController(GuestBookContext context)
        {
            _context=context;
        }

        public async Task<IActionResult> Index()
        {
            var cont = _context.Messages.Include(x => x.User);
            return View(await cont.ToListAsync());
        }

        //public IActionResult Index()
        //{
        //    return View();
        //    //if(HttpContext.Session.GetString("Name") != null)
        //    //{
        //    //    return View();
        //    //}
        //    //else
        //    //{
        //    //    return RedirectToAction("Login", "Account");
        //    //}

        //}

        public ActionResult CreateMes()
        {
            return View();
        }

        public ActionResult Logout()
        {
            // очищается сессия
            HttpContext.Session.Clear();
            // переадресация на Login на контроллере Account
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
