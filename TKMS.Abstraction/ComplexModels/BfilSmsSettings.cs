using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class BfilSmsSettings
    {
        public string GatewayUrl { get; set; }
        public string Apikey { get; set; }
        public string SenderId { get; set; }
        public string AllocateTemplate { get; set; }
        public string ReturnTemplate { get; set; }
        public string Format { get; set; }
    }
}
