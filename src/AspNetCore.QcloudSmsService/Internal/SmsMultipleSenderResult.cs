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
    public class SmsMultipleSenderResult
    {
        public int? result;
        public string errmsg = "";
        public string ext = "";
        public IList<Detail> detail;

        public override string ToString()
        {
            if (null != detail)
            {
                return String.Format(
                        "SmsMultipleSenderResult\nresult {0}\nerrmsg {1}\next {2}\ndetail:\n{3}",
                        result, errmsg, ext, String.Join("\n", detail));
            }
            else
            {
                return String.Format(
                     "SmsMultipleSenderResult\nresult {0}\nerrmsg {1}\next {2}\n",
                     result, errmsg, ext);
            }
        }
    }

    public class Detail
    {
        public int result { get; set; }
        public string errmsg { get; set; }
        public string mobile { get; set; }
        public string nationcode { get; set; }
        public string sid { get; set; }
        public int fee { get; set; }

        public override string ToString()
        {
            return string.Format(
                    "\tDetail result {0} errmsg {1} mobile {2} nationcode {3} sid {4} fee {5}",
                    result, errmsg, mobile, nationcode, sid, fee);
        }
    }

}
