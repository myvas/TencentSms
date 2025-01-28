using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TencentSms.UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddTencentSms(options =>
            {
                options.AppId = 0;
                options.AppKey = "your-app-key-on-tencent-cloud";
                options.BrandName = "Myvas";
            });
        }
    }
}