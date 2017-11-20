using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.QcloudSms
{
    public class QcloudSmsOptions
    {
        /// <summary>
        /// 腾讯云sdkappid
        /// </summary>
        public string SdkAppId { get; set; }

        /// <summary>
        /// 腾讯云sdkappid对应的appkey
        /// </summary>
        public string AppKey { get; set; }

        public string ServiceUrl { get; set; } = QcloudSmsDefaults.ServiceUrl;

        public string AdvancedServiceUrl { get; set; } = QcloudSmsDefaults.AdvancedServiceUrl;
    }
}
