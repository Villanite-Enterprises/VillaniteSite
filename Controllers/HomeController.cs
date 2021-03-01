using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Web;
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
        public IActionResult BrowserSupportSubmission()
        {
            StringBuilder allHeaders = new StringBuilder();

            foreach (var header in Request.Headers)
            {                
                allHeaders.Append("--- " + HttpUtility.HtmlEncode(header.ToString()) + "\\n");
            }

            string gitHubApiToken = _config.GetSection("APITokens").GetSection("GitHub").Value;

            httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("token", gitHubApiToken);
            httpclient.DefaultRequestHeaders.Add("User-Agent", "C# Application");

            string jsonBody = "{\"title\":\"New Browser Support Request\", \"body\":\"" + allHeaders.ToString() + "\"}";

            HttpContent bodyContent = new StringContent(jsonBody);

            httpclient.PostAsync("https://api.github.com/repos/Villanite-Enterprises/VillaniteSite/issues", bodyContent);

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
