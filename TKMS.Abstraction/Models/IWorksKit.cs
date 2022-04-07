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
    public class IWorksKit : BaseClass
    {
        [Key]
        public long IWorksKitId { get; set; }

        public long KitId { get; set; }

        public string AccountNo { get; set; }

        public string IWorksStatus { get; set; }

        public bool IsSuccess { get; set; }

        public string ResponseMessage { get; set; }

        public int RetryCount { get; set; }
    }
}
