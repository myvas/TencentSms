using Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Myvas.AspNetCore.TencentSms;
using System;
using System.Threading.Tasks;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;

        public HomeController(ISmsSender smsSender)
        {
            _smsSender = smsSender ?? throw new ArgumentNullException(nameof(smsSender));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendSms(SmsSendViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _smsSender.SendSmsAsync(vm.Mobile, vm.Content);
            
            return Ok(result);
        }
    }
}