using AspNetCore.HechinaSmsService.Sample.Models;
using AspNetCore.QcloudSms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AspNetCore.HechinaSmsService.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISmsSender _smsSender;

        public HomeController(ISmsSender smsSender)
        {
            _smsSender = smsSender;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendSms(SendSmsViewModel vm)
        {
            if (!ModelState.IsValid) return Ok(false);

            var result = await _smsSender.SendSmsAsync(vm.Mobile, vm.Content);

            return Ok(result);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
    }
}