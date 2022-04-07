using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class IWorksSettings
    {
        public string BaseUrl { get; set; }
        public string BaseMethod { get; set; }
    }

    public class IWorksResult
    {
        public string successMessage { get; set; }
        public string errorMessage { get; set; }
        public string Message { get; set; }
    }
}
