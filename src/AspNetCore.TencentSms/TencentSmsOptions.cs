using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetCore.TencentSms
{
    public class TencentSmsOptions
    {
        /// <summary>
        /// 腾讯云短信sdkappid（整数）
        /// </summary>
        public string SdkAppId { get; set; }

        /// <summary>
        /// 腾讯云短信sdkappid（字符串）
        /// </summary>
        public int AppId
        {
            get { return int.TryParse(SdkAppId, out int result) ? result : 0; }
            set { SdkAppId = value.ToString(); }
        }

        /// <summary>
        /// 腾讯云短信sdkappid对应的appkey
        /// </summary>
        public string AppKey { get; set; }

        public string BrandName { get; set; }

        /// <summary>
        /// Used to communicate with the remote sms server.
        /// </summary>
        public HttpClient Backchannel { get; set; }

        /// <summary>
        /// Gets or sets timeout value in milliseconds for back channel communications with the remote sms server.
        /// </summary>
        public TimeSpan BackchannelTimeout { get; set; }

        /// <summary>
        /// Gets or sets the time limit for completing the sms operation/flow (15 minutes by default).
        /// </summary>
        public TimeSpan RemoteSmsTimeout { get; set; } = TimeSpan.FromMinutes(15);

        public virtual void Validate()
        {
            if (string.IsNullOrEmpty(SdkAppId))
                throw new ArgumentNullException(nameof(SdkAppId));
            if (string.IsNullOrEmpty(AppKey))
                throw new ArgumentNullException(nameof(AppKey));
        }
    }
}
