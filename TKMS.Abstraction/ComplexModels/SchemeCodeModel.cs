using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class SchemeCodeModel
    {
        public long SchemeCodeId { get; set; }
        public string SchemeCodeName { get; set; }
        public IEnumerable<string> SchemeC5Codes { get; set; }
    }
}
