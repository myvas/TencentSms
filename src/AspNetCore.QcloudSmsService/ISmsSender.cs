using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.QcloudSms
{
    public interface ISmsSender
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content">【短信签名】短信正文</param>
        /// <returns></returns>
        Task<bool> SendSmsAsync(string mobile, string content);
    }
}
