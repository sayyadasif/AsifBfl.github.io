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
    public class SchemeC5Code
    {
        [Key]
        public long SchemeC5CodeId { get; set; }
        public long SchemeCodeId { get; set; }
        public long C5CodeId { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        [ForeignKey(nameof(SchemeCodeId))]
        public virtual SchemeCode SchemeCode { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        [ForeignKey(nameof(C5CodeId))]
        public virtual C5Code C5Code { get; set; }
    }
}
