using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Myvas.AspNetCore.TencentSms;
using System.Reflection;

namespace TencentSms.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddTencentSms(options =>
            {
                options.AppId = 0;
                options.AppKey = "your-app-key-on-tencent-cloud";
                options.BrandName = "Myvas";
            });
            var app = builder.Build();

            var smsSender = app.Services.GetRequiredService<ISmsSender>();

            var mobile = "13800138000";
            var content = "��Myvas��������֤���ǣ�1234����Ǳ��˲���������Ա����š�";
            var result = await smsSender.SendSmsAsync(mobile, content);
            Assert.False(result);
        }
    }
}