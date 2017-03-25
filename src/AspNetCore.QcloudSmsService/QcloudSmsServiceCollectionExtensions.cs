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
        public static IServiceCollection AddQcloudSms(this IServiceCollection services, Action<QcloudSmsOptions> setupAction, bool debug)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (setupAction != null)
            {
                services.Configure(setupAction);//Register IOptions<QcloudSmsOptions>
            }

            if (debug)
            {
                //services.AddSingleton<DebugQcloudSmsManager>(); //Register DebugQcloudSmsManager
                services.AddSingleton<IVerificationCodeSmsSender, DebugQcloudSmsManager>();
                services.AddSingleton<ISmsSender, DebugQcloudSmsManager>();
            }
            else
            {
                //services.AddSingleton<QcloudSmsManager>(); //Register QcloudSmsManager
                services.AddSingleton<IVerificationCodeSmsSender, QcloudSmsManager>();
                services.AddSingleton<ISmsSender, QcloudSmsManager>();
            }

            return services;
        }

        /// <summary>
        /// Using Sms Middleware
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> passed to the configuration method.</param>
        /// <param name="setupAction">Middleware configuration options.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddQcloudSms(this IServiceCollection services, bool debug)
        {
            return services.AddQcloudSms(null, debug);
        }
        
        /// <summary>
        /// Using Sms Middleware
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> passed to the configuration method.</param>
        /// <param name="setupAction">Middleware configuration options.</param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddQcloudSms(this IServiceCollection services, Action<QcloudSmsOptions> setupAction)
        {
            return services.AddQcloudSms(setupAction, false);
        }
    }
}
