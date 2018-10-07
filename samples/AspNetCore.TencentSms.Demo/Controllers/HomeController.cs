using AspNetCore.TencentSms.Demo.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AspNetCore.TencentSms;

namespace AspNetCore.TencentSms.Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISmsSender _smsSender;

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