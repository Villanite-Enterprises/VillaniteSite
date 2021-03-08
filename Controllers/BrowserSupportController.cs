using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VillaniteSite.Models;

namespace VillaniteSite.Controllers
{
    public class BrowserSupportController : Controller
    {
        readonly HttpClient httpclient = new();

        private readonly ILogger<BrowserSupportController> _logger;
        private readonly IConfiguration _config;

        public BrowserSupportController(ILogger<BrowserSupportController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            BrowserSupportViewModel viewModel = new();

            if (Request.Headers["User-Agent"].ToString().Contains("Googlebot"))
            {
                viewModel.IsRobot = true;
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SubmitNewRequest()
        {
            try
            {
                StringBuilder allHeaders = new();

                foreach (var header in Request.Headers)
                {
                    allHeaders.Append("--- " + HttpUtility.HtmlEncode(header.ToString()) + "\\n");
                }

                string gitHubApiToken = _config.GetSection("APITokens").GetSection("GitHub").Value;

                httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("token", gitHubApiToken);
                httpclient.DefaultRequestHeaders.Add("User-Agent", "C# Application");

                string jsonBody = "{\"title\":\"New Browser Support Request\", \"body\":\"" + allHeaders.ToString() + "\"}";

                HttpContent bodyContent = new StringContent(jsonBody);

                var response = httpclient.PostAsync("https://api.github.com/repos/Order-of-the-Nesur/VillaniteSite/issues", bodyContent).Result;

                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException("Something went wrong with the GitHub call: " + response.ReasonPhrase);
                }
            }
            catch
            {
                return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError);
            }
            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status201Created);
        }
    }
}
