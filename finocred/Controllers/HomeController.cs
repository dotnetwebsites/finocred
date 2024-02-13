using finocred.BusinessLayer.Interfaces;
using finocred.Models;
using finocred.web.BusinessLayer.DTOs;
using finocred.web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace finocred.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _emailSender;
        private IConfiguration _configuration { get; }

        public HomeController(ILogger<HomeController> logger, IEmailSender emailSender,
            IConfiguration configuration)
        {
            _logger = logger;
            _emailSender = emailSender;
            _configuration = configuration;
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

        [HttpGet]
        [Route("applynow")]
        public IActionResult ApplyNow()
        {
            return View();
        }

        [HttpPost]
        [Route("applynow")]
        public async Task<IActionResult> ApplyNow(ApplyDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var jsonData = JsonConvert.SerializeObject(model);

                    var client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Post, _configuration["baseUrl"] + "api/basicdetails/");

                    request.Content = new StringContent(jsonData, null, "application/json");
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    var result = await response.Content.ReadAsStringAsync();

                    TempData["Msg"] = "Thank you for submitting the details. Our representative will contact you shortly.";
                    return RedirectToAction("applynow");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                model.FullName = ex.Message;
                return View(model);
            }            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string[] blockedcontent = _configuration["blocked_keyword_for_mail"].ToString().ToLower().Split(",");
                    bool blocked = false;

                    foreach (var item in blockedcontent)
                    {
                        if (contactUs.FirstName.ToLower().Split(" ").Contains(item))
                            blocked = true;

                        if (contactUs.LastName.ToLower().Split(" ").Contains(item))
                            blocked = true;

                        if (contactUs.PhoneNumber.ToLower().Split(" ").Contains(item))
                            blocked = true;

                        if (contactUs.Subject.ToLower().Split(" ").Contains(item))
                            blocked = true;

                        if (contactUs.Message.ToLower().Split(" ").Contains(item))
                            blocked = true;
                    }

                    if (blocked)
                    {
                        ModelState.AddModelError(string.Empty, "The content your are trying to send has been blocked due to some suspicious activities found. If you still want to connect then try to send a valid email.");
                        return View(contactUs);
                    }

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

                    CaptchaResponse response = ValidateCaptcha(Request.Form["g-recaptcha-response"].ToString());
                    if (response.Success && ModelState.IsValid)
                    {
                        var mailsts = _emailSender.SendEmail("contact@finocred.com", "finocred", contactUs.Subject, htmlContect);

                        if (mailsts == MailStatus.NotConfigure)
                        {
                            ModelState.AddModelError(string.Empty, "Mail not configure");
                            return View(contactUs);
                        }
                        else if (mailsts == MailStatus.Failed)
                        {
                            ModelState.AddModelError(string.Empty, "Mail not sent. Something went wrong.");
                            return View(contactUs);
                        }

                        TempData["msg"] = "Thank you for sending an email. Our representative will connect you in 24 hrs.";
                        return RedirectToAction("ContactUs");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Captcha");
                        return View(contactUs);
                    }
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

            return View(contactUs);
        }

        /// <summary>  
        /// Validate Captcha  
        /// </summary>  
        /// <param name="response"></param>  
        /// <returns></returns>  
        public CaptchaResponse ValidateCaptcha(string response)
        {
            string secret = _configuration["captcha:secret"];
            var client = new WebClient();
            var jsonResult = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            return JsonConvert.DeserializeObject<CaptchaResponse>(jsonResult.ToString());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
