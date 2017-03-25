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
        public int SdkAppId { get; set; }

        /// <summary>
        /// 腾讯云sdkappid对应的appkey
        /// </summary>
        public string AppKey { get; set; }

        public string ServiceUrl { get; set; } = QcloudSmsDefaults.ServiceUrl;

        public string AdvancedServiceUrl { get; set; } = QcloudSmsDefaults.AdvancedServiceUrl;

        /// <summary>
        /// 短信签名。在腾讯云中提交并审核通过的短信签名。
        /// </summary>
        public string OrganizationSignature { get; set; } = "";

        /// <summary>
        /// 验证码模版ID。在腾讯云中提交并审核通过的验证码“短信正文内容”模版ID
        /// </summary>
        public int VerificationCodeSmsTemplateId { get; set; }
    }
}
