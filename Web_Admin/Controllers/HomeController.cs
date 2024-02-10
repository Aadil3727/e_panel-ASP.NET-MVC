using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using Web_Admin.Data;
using Web_Admin.Models;

namespace Web_Admin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _context;


        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _context = context;

            _logger = logger;
        }

        public IActionResult Index()
        {
            //var userId = Request.Cookies["userid"];

            // Use the user ID to fetch and display user data (replace this with your logic)
            return View();
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
