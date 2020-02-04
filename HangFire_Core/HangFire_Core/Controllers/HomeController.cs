using Hangfire;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HangFire_Core.Controllers
{
    public class HomeController : Controller
    {
        private IHostingEnvironment _env { get; set; }
        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }
        [Authorize]
        public IActionResult Index()
        {
            RecurringStartAndEndAtSpectificTime();
            return View();
        }

        public IActionResult Privacy()
        {
            //RecurringJob.AddOrUpdate(() => ReadWriteFile(), Cron.Minutely);
            RecurringStartAndEndAtSpectificTime();

            return View();
        }

        public void ReadWriteFile()
        {
            new ProcessFile(_env).ReadWriteFile();
        }

        public void RecurringStartAndEndAtSpectificTime()
        {
            RecurringJob.AddOrUpdate("Craw Result", () => CrawResult(), "*/10 * * * * *", TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        }

        [DisplayName("Craw Result")]
        public void CrawResult()
        {
            int i = 1;
            Console.WriteLine($"i = {i + 1}");
        }
    }
}
