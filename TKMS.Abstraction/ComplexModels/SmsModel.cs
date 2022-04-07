using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class SentSmsRequest
    {
        public string MobileNumbers { get; set; }
        public string Otp { get; set; }
        public bool IsAllocate { get; set; }
    }

    public class SmsRequest
    {
        public string apikey { get; set; }
        public string senderid { get; set; }
        public string number { get; set; }
        public string message { get; set; }
        public string format { get; set; }
    }

    public class SmsNumber
    {
        public string id { get; set; }
        public string mobile { get; set; }
        public string status { get; set; }
    }

    public class SmsResult
    {
        public string status { get; set; }
        public List<SmsNumber> data { get; set; }
        public string msgid { get; set; }
        public string message { get; set; }
    }
}
