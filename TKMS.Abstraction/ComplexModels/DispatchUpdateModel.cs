using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonIgnoreRequest = System.Text.Json.Serialization.JsonIgnoreAttribute;
using JsonIgnoreResponse = Newtonsoft.Json.JsonIgnoreAttribute;

namespace TKMS.Abstraction.ComplexModels
{
    public class DispatchUpdateModel
    {
        public long IndentId { get; set; }

        public long DispatchId { get; set; }

        public bool IsReceivedAtRo { get; set; }

        public bool IsDispatchToBranch { get; set; }

        public bool IsReceivedAtBranch { get; set; }
    }
}
