using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonIgnoreRequest = System.Text.Json.Serialization.JsonIgnoreAttribute;
using JsonIgnoreResponse = Newtonsoft.Json.JsonIgnoreAttribute;

namespace TKMS.Abstraction.Models
{
    public class IndentStatus : BaseClass
    {
        [Key]
        public long IndentStatusId { get; set; }

        public string StatusName { get; set; }

        public string StatusKey { get; set; }

        public int SortOrder { get; set; }

    }
}
