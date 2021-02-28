using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using VillaniteSite.Models;

namespace VillaniteSite.Controllers
{
    public class HomeController : Controller
    {
        HttpClient httpclient = new HttpClient();

        private readonly ILogger<HomeController> _logger;
        private IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ToDo()
        {
            return View();
        }
        public IActionResult HardwareSoftware()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            string gitHubApiToken = _config.GetSection("APITokens").GetSection("GitHub").Value;

            httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("token", gitHubApiToken);
            httpclient.DefaultRequestHeaders.Add("User-Agent", "C# Application");

            HttpContent bodyContent = new StringContent("{\"title\":\"Testing\", \"body\":\"This is a test.\"}");

            var callResponse = httpclient.PostAsync("https://api.github.com/repos/Villanite-Enterprises/VillaniteSite/issues", bodyContent).Result;

            //var callResponse = httpclient.GetAsync("https://api.github.com/repos/Villanite-Enterprises/VillaniteSite/issues").Result;

            ViewBag.Temp = callResponse.Content.ReadAsStringAsync().Result;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
