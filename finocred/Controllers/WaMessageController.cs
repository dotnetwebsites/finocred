using finocred.web.BusinessLayer.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace finocred.web.Controllers
{
    public class WaMessageController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly IHttpClientFactory _httpClientFactory;
        public WaMessageController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            Configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index(string Password)
        {
            if (Password != null)
            {
                string source1 = Password;
                string source2 = Configuration["pass"].ToString();
                using (SHA512 sha512Hash = SHA512.Create())
                {
                    byte[] sourceBytes1 = Encoding.UTF8.GetBytes(source1);
                    byte[] sourceBytes2 = Encoding.UTF8.GetBytes(source2);
                    byte[] hashBytes1 = sha512Hash.ComputeHash(sourceBytes1);
                    byte[] hashBytes2 = sha512Hash.ComputeHash(sourceBytes2);
                    string hash1 = BitConverter.ToString(hashBytes1).Replace("-", String.Empty);
                    string hash2 = BitConverter.ToString(hashBytes2).Replace("-", String.Empty);

                    if (Request.Cookies["pass"] != null && hash1 == Request.Cookies["pass"].ToString())
                    {
                        return RedirectToAction("index");
                    }
                    else if (hash1 == hash2)
                    {
                        CookieOptions option = new CookieOptions();
                        Response.Cookies.Append("pass", hash2, option);
                    }
                    else
                        TempData["msg"] = "Invalid password";
                }

                return RedirectToAction("index");
            }

            return View();
        }

        [HttpGet]
        public IActionResult DeleteWaCookie()
        {
            if (Request.Cookies["pass"] != null)
                Response.Cookies.Delete("pass");

            return RedirectToAction("index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(WaModel model)
        {
            if (model.Mobiles == null)
                return NotFound();

            if (model.Templates == null)
                return NotFound();

            if (model.ImageUrl == null)
                return NotFound();

            List<string> mobiles = model.Mobiles.ToString().Replace(" ", "").Replace("+", "").Split(",").ToList();

            List<string> exeededNo = new List<string>();
            foreach (var m in mobiles)
            {
                if (m.Length > 10)
                    exeededNo.Add(m);
            }

            if (exeededNo != null && exeededNo.Count > 0)
            {
                TempData["msg"] = "Please correct these nos: " + string.Join(",", exeededNo);
                return RedirectToAction("index");
            }

            int cnt = 0;
            List<string> sentSms = new List<string>();
            foreach (var m in mobiles)
            {
                var wa = JsonSerializer.Serialize<WhatsappDTO>(content(m, model.Templates, model.ImageUrl, "en"));

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, Configuration["Whatsapp:url"]);
                httpRequestMessage.Content = new StringContent(wa, Encoding.UTF8, "application/json");
                httpRequestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", Configuration["Whatsapp:auth"]);

                var httpClient = _httpClientFactory.CreateClient();
                var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                    var WhatsappResponseDTO = await JsonSerializer.DeserializeAsync<WhatsappResponseDTO>(contentStream);

                    TempData["msg"] = "Invalid token, please contact administrator.";
                    return RedirectToAction("index");
                }

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    cnt++;
                    sentSms.Add(m);
                    using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                    var WhatsappResponseDTO = await JsonSerializer.DeserializeAsync<WhatsappResponseDTO>(contentStream);
                }
            }

            if (cnt > 0)
            {
                TempData["msg"] = cnt + " messages sent see contact list: " + string.Join(",", sentSms);
                return RedirectToAction("index");
            }

            return View(model);
        }

        private WhatsappDTO content(string mob, string template, string imageUrl, string locale)
        {
            var res = new WhatsappDTO();

            res.messaging_product = "whatsapp";
            res.recipient_type = "individual";
            res.to = "91" + mob;
            res.type = "template";


            res.template = new template
            {
                name = template,
                language = new language
                {
                    code = locale
                },
                components = new components[] {
                    new components
                    {
                        type="header",
                        parameters=new parameters[]
                        {
                            new parameters
                            {
                                type="image",
                                image=new image
                                {
                                    link=imageUrl
                                }
                            }
                        }
                    }
                }
            };

            return res;
        }
    }
}
