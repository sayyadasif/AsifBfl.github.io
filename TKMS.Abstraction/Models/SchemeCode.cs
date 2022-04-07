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
    public class SchemeCode : BaseClass
    {
        [Key]
        public long SchemeCodeId { get; set; }

        public string SchemeCodeName { get; set; }

        public int SortOrder { get; set; }

        public virtual ICollection<SchemeC5Code> SchemeC5Codes { get; set; } = new List<SchemeC5Code>();

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        public long[] SelectedC5Codes { get; set; } = Array.Empty<long>();

    }
}
