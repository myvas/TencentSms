using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using qcloudsms_csharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetCore.TencentSms
{
    public class TencentSmsManager : ISmsSender
    {
        private readonly HttpClient _backchannel;

        private readonly ILogger _logger;
        private readonly TencentSmsOptions _options;

        public TencentSmsManager(IOptions<TencentSmsOptions> optionsAccessor, ILogger<TencentSmsManager> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = optionsAccessor?.Value ?? throw new ArgumentNullException(nameof(optionsAccessor));
            _backchannel = _options.Backchannel ?? new HttpClient();
        }

        /// <summary>
        /// 发送普通短信，但短信内容必须与事先在腾讯云提交审核通过的模版匹配。
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content">短信正文（带签名）。例如：【短信签名】{1}为您的验证码。如非本人操作，请忽略本短信。</param>
        /// <remarks>腾讯云规定：短信内容必须匹配事先提交并通过审核的模版。</remarks>
        /// <returns></returns>
        public async Task<bool> SendSmsAsync(string mobile, string content)
        {
            await Task.FromResult(0);

            var appid = _options.AppId;
            var appkey = _options.AppKey;
            var defaultNationCode = TencentSmsDefaults.DefaultNationCode;

            if (string.IsNullOrWhiteSpace(mobile))
            {
                throw new ArgumentNullException(nameof(mobile));
            }
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(nameof(content));
            }

            try
            {
                var sender = new SmsSingleSender(appid, appkey);
                var sendResult = sender.send(0, defaultNationCode, mobile, content, "", "");

                if (sendResult.result == 0)
                {
                    _logger.LogDebug($"成功发送短信给[{mobile}]，内容长度：{content.Length}");
                    return true;
                }
                else
                {
                    _logger.LogDebug($"未能成功发送短信给[{mobile}]，内容长度：{content.Length}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "发送短信时发生异常");
                return false;
            }
        }

        /// <summary>
        /// 发送短信（单发短信）
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content">短信正文（不带签名）。由系统自动附加发送者签名。</param>
        /// <param name="brandName">发送者签名。若不指定，则由程序从配置参数中读取。</param>
        /// <returns>是否发送成功</returns>
        public async Task<bool> SendSmsWithBrandNameAsync(string mobile, string content, string brandName = null)
        {
            if (string.IsNullOrWhiteSpace(brandName))
            {
                brandName = _options.BrandName;
            }
            content = $"【{brandName}】{content}";
            return await SendSmsAsync(mobile, content);
        }

        /// <summary>
        /// 群发短信，发送相同短信内容给多人。
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content">短信正文（带签名）。由发送者主动附加发送者签名。例如："【短信签名】短信正文"</param>
        /// <returns>成功提交的手机号码</returns>
        public async Task<List<string>> SendSmsAsync(List<string> mobiles, string content)
        {
            await Task.FromResult(0);

            var appid = _options.AppId;
            var appkey = _options.AppKey;
            var defaultNationCode = TencentSmsDefaults.DefaultNationCode;

            if (mobiles == null || !mobiles.Any())
            {
                throw new ArgumentNullException(nameof(mobiles));
            }
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentNullException(nameof(content));
            }

            try
            {
                var sender = new SmsMultiSender(appid, appkey);
                var sendResult = sender.send(0, defaultNationCode, mobiles, content, "", "");

                if (sendResult.result == 0)
                {
                    _logger.LogDebug($"成功提交群发短信给[{mobiles[0]}]等[{mobiles.Count}]人，内容长度：{content.Length}");
                    return sendResult.details.Where(x => x.result == 0).Select(x => x.mobile).ToList();
                }
                else
                {
                    _logger.LogDebug($"未能成功提交群发短信给[{mobiles[0]}]等[{mobiles.Count}]人，内容长度：{content.Length}");
                    return new List<string>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "提交群发短信时发生异常");
                return new List<string>();
            }
        }

        /// <summary>
        /// 群发短信，发送相同短信内容给多人。
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content">短信正文（不带签名）。由系统自动附加发送者签名。</param>
        /// <param name="brandName">发送者签名。若不指定，则由程序从配置参数中读取。</param>
        /// <returns>成功提交的手机号码</returns>
        public async Task<List<string>> SendSmsWithBrandNameAsync(List<string> mobiles, string content, string brandName = null)
        {
            if (string.IsNullOrWhiteSpace(brandName))
            {
                brandName = _options.BrandName;
            }
            content = $"【{brandName}】{content}";
            return await SendSmsAsync(mobiles, content);
        }
    }
}
