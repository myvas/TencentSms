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

        /// <summary>
        /// https://yun.tim.qq.com/v5/tlssmssvr/sendsms?sdkappid=xxxxx&random=xxxx
        /// </summary>
        public string ServiceUrl { get; set; } = QcloudSmsDefaults.ServiceUrl;

        /// <summary>
        /// https://yun.tim.qq.com/v5/tlssmssvr/sendmultisms2?sdkappid=xxxxx&random=xxxx
        /// </summary>
        public string AdvancedServiceUrl { get; set; } = QcloudSmsDefaults.AdvancedServiceUrl;

        public string BrandName { get; set; }
    }
}
