using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.QcloudSms.Internal
{
    public class SmsMultipleSender
    {
        private readonly ILogger _logger;
        protected QcloudSmsOptions Options { get; private set; }
        protected HttpClient _backchannel { get; private set; }

        SmsSenderUtil _util = new SmsSenderUtil();

        public SmsMultipleSender(IOptions<QcloudSmsOptions> optionsAccessor, ILoggerFactory loggerFactory)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            Options = optionsAccessor.Value;

            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
            _logger = loggerFactory.CreateLogger<SmsSingleSender>();


            _backchannel = new HttpClient(new HttpClientHandler());
            _backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("MyvasAgent/1.0");
            _backchannel.Timeout = TimeSpan.FromSeconds(60);
            _backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
        }

        /*
        请求包体
        {
            "tel": [
                {
                    "nationcode": "86", 
                    "mobile": "13788888888"
                }, 
                {
                    "nationcode": "86", 
                    "mobile": "13788888889"
                }
            ], 
            "type": 0, 
            "msg": "你的验证码是1234", 
            "sig": "fdba654e05bc0d15796713a1a1a2318c",
            "time": 1479888540,
            "extend": "", 
            "ext": ""
        }
        应答包体
        {
            "result": 0, 
            "errmsg": "OK", 
            "ext": "", 
            "detail": [
                {
                    "result": 0, 
                    "errmsg": "OK", 
                    "mobile": "13788888888", 
                    "nationcode": "86", 
                    "sid": "xxxxxxx", 
                    "fee": 1
                }, 
                {
                    "result": 0, 
                    "errmsg": "OK", 
                    "mobile": "13788888889", 
                    "nationcode": "86", 
                    "sid": "xxxxxxx", 
                    "fee": 1
                }
            ]
        }
        */
        /**
         * 普通群发短信接口，明确指定内容，如果有多个签名，请在内容中以【】的方式添加到信息内容中，否则系统将使用默认签名
         * 【注意】海外短信无群发功能
         * @param type 短信类型，0 为普通短信，1 营销短信
         * @param nationCode 国家码，如 86 为中国
         * @param phoneNumbers 不带国家码的手机号列表
         * @param msg 信息内容，必须与申请的模板格式一致，否则将返回错误
         * @param extend 扩展码，可填空
         * @param ext 服务端原样返回的参数，可填空
         * @return SmsMultiSenderResult
         */
        public async Task<SmsMultipleSenderResult> Send(
            int type,
            string nationCode,
            List<string> phoneNumbers,
            string msg,
            string extend,
            string ext)
        {
            var sdkappid = Options.SdkAppId;
            var appkey = Options.AppKey;
            var url = Options.AdvancedServiceUrl;

            if (0 != type && 1 != type)
            {
                throw new Exception("type " + type + " error");
            }
            if (null == extend)
            {
                extend = "";
            }
            if (null == ext)
            {
                ext = "";
            }

            long random = _util.GetRandom();
            long curTime = _util.GetCurTime();

            // 按照协议组织 post 请求包体
            JObject data = new JObject();
            data.Add("tel", _util.PhoneNumbersToJSONArray(nationCode, phoneNumbers));
            data.Add("type", type);
            data.Add("msg", msg);
            data.Add("sig", _util.CalculateSig(appkey, random, curTime, phoneNumbers));
            data.Add("time", curTime);
            data.Add("extend", extend);
            data.Add("ext", ext);

            string wholeUrl = url + "?sdkappid=" + sdkappid + "&random=" + random;

            var content = JsonConvert.SerializeObject(data);
            var requestContent = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, wholeUrl);
            requestMessage.Content = requestContent;

            var responseMessage = await _backchannel.SendAsync(requestMessage);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var result = _util.ResponseStrToMultiSenderResult(responseContent);
                return result;
            }
            else
            {

                var result = new SmsMultipleSenderResult()
                {
                    result = -1,
                    errmsg = "http error " + responseMessage.StatusCode
                };
                return result;
            }
        }

        /*
        请求包体
        {
            "tel": [
                {
                    "nationcode": "86", 
                    "mobile": "13788888888"
                }, 
                {
                    "nationcode": "86", 
                    "mobile": "13788888889"
                }
            ], 
            "type": 0, 
            "msg": "你的验证码是1234", 
            "sig": "fdba654e05bc0d15796713a1a1a2318c",
            "time": 1479888540,
            "extend": "", 
            "ext": ""
        }
        应答包体
        {
            "result": 0, 
            "errmsg": "OK", 
            "ext": "", 
            "detail": [
                {
                    "result": 0, 
                    "errmsg": "OK", 
                    "mobile": "13788888888", 
                    "nationcode": "86", 
                    "sid": "xxxxxxx", 
                    "fee": 1
                }, 
                {
                    "result": 0, 
                    "errmsg": "OK", 
                    "mobile": "13788888889", 
                    "nationcode": "86", 
                    "sid": "xxxxxxx", 
                    "fee": 1
                }
            ]
        }
        */

        /**
         * 指定模板群发
         * 【注意】海外短信无群发功能
         * @param nationCode 国家码，如 86 为中国
         * @param phoneNumbers 不带国家码的手机号列表
         * @param templId 模板 id
         * @param params 模板参数列表
         * @param sign 签名，如果填空，系统会使用默认签名
         * @param extend 扩展码，可以填空
         * @param ext 服务端原样返回的参数，可以填空
         * @return SmsMultiSenderResult
         */
        public async Task<SmsMultipleSenderResult> SendWithParam(
            String nationCode,
            List<string> phoneNumbers,
            int templId,
            List<string> templParams,
            string sign,
            string extend,
            string ext)
        {
            var sdkappid = Options.SdkAppId;
            var appkey = Options.AppKey;
            var url = Options.AdvancedServiceUrl;

            if (null == sign)
            {
                sign = "";
            }
            if (null == extend)
            {
                extend = "";
            }
            if (null == ext)
            {
                ext = "";
            }

            long random = _util.GetRandom();
            long curTime = _util.GetCurTime();

            // 按照协议组织 post 请求包体
            JObject data = new JObject();
            data.Add("tel", _util.PhoneNumbersToJSONArray(nationCode, phoneNumbers));
            data.Add("sig", _util.CalculateSigForTempl(appkey, random, curTime, phoneNumbers));
            data.Add("tpl_id", templId);
            data.Add("params", _util.SmsParamsToJSONArray(templParams));
            data.Add("sign", sign);
            data.Add("time", curTime);
            data.Add("extend", extend);
            data.Add("ext", ext);

            string wholeUrl = url + "?sdkappid=" + sdkappid + "&random=" + random;

            var content = JsonConvert.SerializeObject(data);
            var requestContent = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, wholeUrl);
            requestMessage.Content = requestContent;

            var responseMessage = await _backchannel.SendAsync(requestMessage);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                var result = _util.ResponseStrToMultiSenderResult(responseContent);
                return result;
            }
            else
            {

                var result = new SmsMultipleSenderResult()
                {
                    result = -1,
                    errmsg = "http error " + responseMessage.StatusCode
                };
                return result;
            }
        }
    }
}
