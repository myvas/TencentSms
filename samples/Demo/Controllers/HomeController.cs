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
        private readonly ILogger<HomeController> _logger;
        private readonly IVerificationCodeSmsSender _smsSender;

        public HomeController(
            ILoggerFactory loggerFactory,
            IVerificationCodeSmsSender smsSender)
        {
            _logger = loggerFactory?.CreateLogger<HomeController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _smsSender = smsSender ?? throw new ArgumentNullException(nameof(smsSender));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendSms(SendSmsViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return Ok(false);
            }

            var result = await _smsSender.SendVerificationCodeAsync(vm.Mobile, vm.Code);

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