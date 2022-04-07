using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class SentSmsModel
    {
        public long SentSmsId { get; set; }

        public string MobileNumber { get; set; }

        public string Message { get; set; }

        public bool IsSuccess { get; set; }

        public string ResponseMessage { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
