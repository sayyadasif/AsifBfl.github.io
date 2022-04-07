using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class BfilAuthSettings
    {
        public string LoginUrl { get; set; }
        public string SecretKey { get; set; }
        public string ReqHeaderKey { get; set; }
        public string ReqHeaderValue { get; set; }
    }
}
