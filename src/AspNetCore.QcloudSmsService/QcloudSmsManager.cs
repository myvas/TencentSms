using AspNetCore.QcloudSms.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.QcloudSms
{
    public class DebugQcloudSmsManager : IVerificationCodeSmsSender, ISmsSender
    {
        private readonly ILogger<DebugQcloudSmsManager> _logger;
        //private readonly string _nationCode = "86";
        private readonly QcloudSmsOptions _options;

        public DebugQcloudSmsManager(IOptions<QcloudSmsOptions> optionsAccessor, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
            _logger = loggerFactory.CreateLogger<DebugQcloudSmsManager>();

            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            _options = optionsAccessor.Value;

        }

        public async Task<bool> SendVerificationCodeAsync(string mobile, string verificationCode)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                throw new ArgumentException($"参数 {nameof(mobile)} 不能为空");
            }
            if (string.IsNullOrWhiteSpace(verificationCode))
            {
                throw new ArgumentException($"参数 {nameof(verificationCode)} 不能为空");
            }

            _logger.LogInformation($"发送一条短信给[{mobile}]: {verificationCode}");
            return await Task.FromResult(true);
        }

        public async Task<bool> SendSmsAsync(string mobile, string content)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                throw new ArgumentException($"参数 {nameof(mobile)} 不能为空");
            }
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException($"参数 {nameof(content)} 不能为空");
            }

            _logger.LogInformation($"发送一条短信给[{mobile}]: {content}");
            return await Task.FromResult(true);
        }
    }

    public class QcloudSmsManager : IVerificationCodeSmsSender, ISmsSender
    {
        private readonly ILogger<QcloudSmsManager> _logger;
        private readonly SmsSingleSender _smsSingleSender;
        private readonly SmsMultipleSender _smsMultipleSender;
        private readonly string _nationCode = "86";

        private readonly QcloudSmsOptions Options;

        public QcloudSmsManager(IOptions<QcloudSmsOptions> optionsAccessor, ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
            _logger = loggerFactory.CreateLogger<QcloudSmsManager>();

            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            Options = optionsAccessor.Value;

            _smsSingleSender = new SmsSingleSender(optionsAccessor, loggerFactory);
            _smsMultipleSender = new SmsMultipleSender(optionsAccessor, loggerFactory);
        }

        /// <summary>
        /// 发送验证码。用户应事先在腾讯云提交并审核通过一个验证码短信模版，并配置好<see cref="QcloudSmsOptions.VerificationCodeSmsTemplateId"/>和<see cref="QcloudSmsOptions.OrganizationSignature"/>。
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        public async Task<bool> SendVerificationCodeAsync(string mobile, string verificationCode)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                throw new ArgumentException($"参数 {nameof(mobile)} 不能为空");
            }
            if (string.IsNullOrWhiteSpace(verificationCode))
            {
                throw new ArgumentException($"参数 {nameof(verificationCode)} 不能为空");
            }

            var templateId = Options.VerificationCodeSmsTemplateId;
            var templateParams = new List<string>() { verificationCode };
            var sign = Options.OrganizationSignature;
            var extend = "";
            var ext = "";

            var oResult = await _smsSingleSender.SendWithParam(_nationCode, mobile, templateId, templateParams, sign, extend, ext);
            var result = oResult.result ?? -1;
            _logger.LogInformation(result == 0 ? $"成功发送一条验证码短信给[{mobile}]，验证码长度：{verificationCode.Length}" : $"未能成功发送一条验证码短信给[{mobile}]，错误代码：{result}");
            return result == 0;
        }

        /// <summary>
        /// 发送普通短信，但短信内容必须与事先在腾讯云提交审核通过的模版匹配。
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content">腾讯云规定：短信内容必须匹配事先提交并通过审核的模版。例如：【大乾入户】{1}为您的验证码。如非本人操作，请忽略本短信。</param>
        /// <returns></returns>
        public async Task<bool> SendSmsAsync(string mobile, string content)
        {
            if (string.IsNullOrWhiteSpace(mobile))
            {
                throw new ArgumentException($"参数 {nameof(mobile)} 不能为空");
            }
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException($"参数 {nameof(content)} 不能为空");
            }

            int type = 0;//0普通短信，1营销短信
            var extend = "";
            var ext = "";

            var oResult = await _smsSingleSender.Send(type, _nationCode, mobile, content, extend, ext);
            var result = oResult.result ?? -1;
            _logger.LogInformation(result == 0 ? $"成功发送一条{(type==0?"普通":"营销")}短信给[{mobile}]，内容长度：{content.Length}" : $"未能成功发送一条{(type == 0 ? "普通" : "营销")}短信给[{mobile}]，错误代码：{result}");
            return result == 0;
        }
    }
}
