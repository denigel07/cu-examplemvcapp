using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ExampleMvcApp.Models;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace ExampleMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            const string MAIL_HOST = "mail"; // "localhost" als mvc app uitgevoerd vanaf lokaal OS (niet in container)

            const int MAIL_PORT = 1025;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Mvc", "mvc@howestgp.be"));
            message.To.Add(new MailboxAddress("","test@fake.com"));
            message.Subject = "Examplemvc is alive.";
            message.Body = new TextPart("plain"){
                Text = "Hi there, testing examplemvc docker-email!"
            };
            using(var mailClient = new SmtpClient()){
                await mailClient.ConnectAsync(MAIL_HOST, MAIL_PORT, SecureSocketOptions.None);
                await mailClient.SendAsync(message);
                await mailClient.DisconnectAsync(true);
            }
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
