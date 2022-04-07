using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class BfilAuthRequest
    {
        public string Request { get; set; }
    }

    public class BfilAuthResponse
    {
        public string Response { get; set; }
    }

    public class BfilAuthResult
    {
        public string RESPONSE_CODE { get; set; }
        public string RESPONSE_MESSAGE { get; set; }
        public string USERNAME { get; set; }
        public string EMPNAME { get; set; }
        public string EMP_ISACTIVE { get; set; }
        public string DEPT { get; set; }
        public string MobileNumber { get; set; }
        public string BranchCode { get; set; }
        public string LoginType { get; set; }
        public string RegionName { get; set; }
        public string ZoneName { get; set; }
    }
}
