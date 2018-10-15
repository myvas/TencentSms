using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Myvas.AspNetCore.TencentSms
{
    public static class TencentSmsDefaults
    {
        public const string DefaultNationCode = "86";

        /// <summary>
        /// 单发服务地址：https://yun.tim.qq.com/v5/tlssmssvr/sendsms
        /// </summary>
        public const string ServiceUrl = "https://yun.tim.qq.com/v5/tlssmssvr/sendsms";

        /// <summary>
        /// 群发服务地址：https://yun.tim.qq.com/v5/tlssmssvr/sendmultisms2
        /// </summary>
        public const string AdvancedServiceUrl = "https://yun.tim.qq.com/v5/tlssmssvr/sendmultisms2";

    }
}
