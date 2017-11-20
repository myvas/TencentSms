using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.QcloudSms
{
    public static class QcloudSmsServiceCollectionExtensions
    {
        /// <summary>
        /// Using Sms Middleware
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> passed to the configuration method.</param>
        /// <param name="setupAction">Middleware configuration options.</param>
        /// <param name="debug">true will log only, that is, NO sms will send to Mobile Terminal.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddQcloudSms(this IServiceCollection services, Action<QcloudSmsOptions> setupAction)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction != null)
            {
                services.Configure(setupAction);//Register IOptions<QcloudSmsOptions>
            }

            services.AddSingleton<ISmsSender, QcloudSmsManager>();

            return services;
        }

        public static IServiceCollection AddQcloudSms(this IServiceCollection services)
        {
            return AddQcloudSms(services, null);
        }
    }
}
