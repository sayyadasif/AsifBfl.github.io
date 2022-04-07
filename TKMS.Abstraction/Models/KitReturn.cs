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
    public class KitReturn : BaseClass
    {
        [Key]
        public long KitReturnId { get; set; }

        public long KitId { get; set; }

        public long ReturnBy { get; set; }

        public DateTime ReturnDate { get; set; }

        public long ReturnAcceptedBy { get; set; }
    }
}
