using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AspNetCore.QcloudSms.Internal
{
    class SmsSenderUtil
    {
        Random random = new Random();

        //public HttpWebRequest GetPostHttpConn(string url)
        //{
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    request.Method = "POST";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    return request;
        //}

        public long GetRandom()
        {
            return random.Next(999999) % 900000 + 100000;
        }

        public long GetCurTime()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp;
        }

        // 将二进制的数值转换为 16 进制字符串，如 "abc" => "616263"
        private static string ByteArrayToHex(byte[] byteArray)
        {
            string returnStr = "";
            if (byteArray != null)
            {
                for (int i = 0; i < byteArray.Length; i++)
                {
                    returnStr += byteArray[i].ToString("x2");
                }
            }
            return returnStr;
        }

        public string StrToHash(string str)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] resultByteArray = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            return ByteArrayToHex(resultByteArray);
        }

        // 将单发回包解析成结果对象
        public SmsSingleSenderResult ResponseStrToSingleSenderResult(string str)
        {
            SmsSingleSenderResult result = JsonConvert.DeserializeObject<SmsSingleSenderResult>(str);
            return result;
        }

        // 将群发回包解析成结果对象
        public SmsMultipleSenderResult ResponseStrToMultiSenderResult(string str)
        {
            SmsMultipleSenderResult result = JsonConvert.DeserializeObject<SmsMultipleSenderResult>(str);
            return result;
        }

        public JArray SmsParamsToJSONArray(List<string> templParams)
        {
            JArray smsParams = new JArray();
            foreach (string templParamsElement in templParams)
            {
                smsParams.Add(templParamsElement);
            }
            return smsParams;
        }

        public JArray PhoneNumbersToJSONArray(string nationCode, List<string> phoneNumbers)
        {
            JArray tel = new JArray();
            int i = 0;
            do
            {
                JObject telElement = new JObject();
                telElement.Add("nationcode", nationCode);
                telElement.Add("mobile", phoneNumbers.ElementAt(i));
                tel.Add(telElement);
            } while (++i < phoneNumbers.Count);

            return tel;
        }

        public string CalculateSigForTempl(
            string appkey,
            long random,
            long curTime,
            List<string> phoneNumbers)
        {
            string phoneNumbersString = phoneNumbers.ElementAt(0);
            for (int i = 1; i < phoneNumbers.Count; i++)
            {
                phoneNumbersString += "," + phoneNumbers.ElementAt(i);
            }
            return StrToHash(String.Format(
                "appkey={0}&random={1}&time={2}&mobile={3}",
                appkey, random, curTime, phoneNumbersString));
        }

        public string CalculateSigForTempl(
            string appkey,
            long random,
            long curTime,
            string phoneNumber)
        {
            List<string> phoneNumbers = new List<string>();
            phoneNumbers.Add(phoneNumber);
            return CalculateSigForTempl(appkey, random, curTime, phoneNumbers);
        }

        public string CalculateSig(
            string appkey,
            long random,
            long curTime,
            List<string> phoneNumbers)
        {
            string phoneNumbersString = phoneNumbers.ElementAt(0);
            for (int i = 1; i < phoneNumbers.Count; i++)
            {
                phoneNumbersString += "," + phoneNumbers.ElementAt(i);
            }
            return StrToHash(String.Format(
                    "appkey={0}&random={1}&time={2}&mobile={3}",
                    appkey, random, curTime, phoneNumbersString));
        }
    }

}
