using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.QcloudSms
{
    public interface IVerificationCodeSmsSender
    {
        Task<bool> SendVerificationCodeAsync(string mobile, string verificationCode);
    }

    public interface ISmsSender
    {
        Task<bool> SendSmsAsync(string mobile, string content);
    }
}
