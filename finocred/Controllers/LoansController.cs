using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finocred.web.Controllers
{
    public class LoansController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Personal()
        {
            return View();
        }
        public IActionResult Business()
        {
            return View();
        }
        public IActionResult Home()
        {
            return View();
        }

    }
}
