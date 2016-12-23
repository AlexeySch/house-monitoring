using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Monitoring.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Temperature()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Humidity()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Power()
        {
            return View();
        }
    }
}
