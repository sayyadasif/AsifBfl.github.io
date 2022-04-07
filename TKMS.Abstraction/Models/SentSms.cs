using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonIgnoreRequest = System.Text.Json.Serialization.JsonIgnoreAttribute;
using JsonIgnoreResponse = Newtonsoft.Json.JsonIgnoreAttribute;

namespace TKMS.Abstraction.Models
{
    public class SentSms : BaseClass
    {
        [Key]
        public long SentSmsId { get; set; }

        public string MobileNumber { get; set; }

        public string Otp { get; set; }

        public string Message { get; set; }

        public bool IsSuccess { get; set; }

        public string ResponseMessage { get; set; }

    }
}
