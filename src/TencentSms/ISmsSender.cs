using System.Collections.Generic;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.TencentSms
{
    public interface ISmsSender
    {
        /// <summary>
        /// 发送短信（单发短信）
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content">短信正文（带签名）。由发送者主动附加发送者签名。例如："【短信签名】短信正文"</param>
        /// <remarks>腾讯规定：短信内容必须匹配事先提交并通过审核的模版。</remarks>
        /// <returns>是否发送成功</returns>
        Task<bool> SendSmsAsync(string mobile, string content);

        /// <summary>
        /// 发送短信（单发短信）
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content">短信正文（不带签名）。由系统自动附加发送者签名。</param>
        /// <param name="brandName">发送者签名。若不指定，则由程序从配置参数中读取。</param>
        /// <returns>是否发送成功</returns>
        Task<bool> SendSmsWithBrandNameAsync(string mobile, string content, string brandName = null);


        /// <summary>
        /// 群发短信，发送相同短信内容给多人。
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content">短信正文（带签名）。由发送者主动附加发送者签名。例如："【短信签名】短信正文"</param>
        /// <remarks>腾讯规定：短信内容必须匹配事先提交并通过审核的模版。</remarks>
        /// <returns>成功提交的手机号码</returns>
        Task<List<string>> SendSmsAsync(List<string> mobiles, string content);

        /// <summary>
        /// 群发短信，发送相同短信内容给多人。
        /// </summary>
        /// <param name="mobile"></param>
        /// <param name="content">短信正文（不带签名）。由系统自动附加发送者签名。</param>
        /// <param name="brandName">发送者签名。若不指定，则由程序从配置参数中读取。</param>
        /// <returns>成功提交的手机号码</returns>
        Task<List<string>> SendSmsWithBrandNameAsync(List<string> mobiles, string content, string brandName = null);
    }
}
