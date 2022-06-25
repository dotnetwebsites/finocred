using finocred.BusinessLayer.Interfaces;
using finocred.Models;
using finocred.web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace finocred.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Disclaimer()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TermsAndConditions()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Partner()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Career()
        {
            return View();
        }

        [HttpGet]
        public IActionResult EmiCalculator()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(ContactUs contactUs)
        {
            try
            {
                string htmlContect = @"<p>Hi,</p><p>This mail is triggered by the user who visited on finocred contact us page.</p>
                                        <p>Customer details:</p>
                                        <table border='1' cellpadding='1' cellspacing='1' style='width:500px'>
	                                        <tbody>
		                                        <tr>
			                                        <td>First name</td>
			                                        <td>" + contactUs.FirstName + @"</td>
		                                        </tr>
		                                        <tr>
			                                        <td>Last name</td>
			                                        <td>" + contactUs.LastName + @"</td>
		                                        </tr>
		                                        <tr>
			                                        <td>Email</td>
			                                        <td>" + contactUs.Email + @"</td>
		                                        </tr>
		                                        <tr>
			                                        <td>Phone no</td>
			                                        <td>" + contactUs.PhoneNumber + @"</td>
		                                        </tr>
		                                        <tr>
			                                        <td>Subject</td>
			                                        <td>" + contactUs.Subject + @"</td>
		                                        </tr>
		                                        <tr>
			                                        <td>Message</td>
			                                        <td>" + contactUs.Message + @"</td>
		                                        </tr>
	                                        </tbody>
                                        </table>
                                        <p>&nbsp;</p>";

                var mailsts = _emailSender.SendEmail("contact@finocred.com", "finocred", contactUs.Subject, htmlContect);

                if (mailsts == MailStatus.NotConfigure)
                {
                    return NoContent();
                }
                else if (mailsts == MailStatus.Failed)
                {
                    return NoContent();
                }

                return RedirectToAction("ContactUs");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("The server response was: 5.7.0 Authentication"))
                    ModelState.AddModelError(string.Empty, "The server response was: 5.7.0 Authentication Required.");
                else
                    ModelState.AddModelError(string.Empty, ex.Message);

                string str = ex.Message;
                return NoContent();
            }            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
