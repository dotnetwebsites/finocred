using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finocred.web.Controllers
{
    public class HomeLoanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Eligibility()
        {
            return View();
        }

        public IActionResult EmiCalculator()
        {
            return View();
        }

        public IActionResult Documents()
        {
            return View();
        }

        public IActionResult Apply()
        {
            return View();
        }
    }
}
