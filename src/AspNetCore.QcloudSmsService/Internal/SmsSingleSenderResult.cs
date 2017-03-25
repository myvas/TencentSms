using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.QcloudSms.Internal
{
    /*
    {
        "result": 0,
        "errmsg": "OK", 
        "ext": "", 
        "sid": "xxxxxxx", 
        "fee": 1
    }
     */
    public class SmsSingleSenderResult
    {
        public int? result { set; get; }
        public string errmsg { set; get; }
        public string ext { set; get; }
        public string sid { set; get; }
        public int? fee { set; get; }

        public override string ToString()
        {
            return string.Format(
                "SmsSingleSenderResult\nresult {0}\nerrMsg {1}\next {2}\nsid {3}\nfee {4}",
                result, errmsg, ext, sid, fee);
        }
    }
}
