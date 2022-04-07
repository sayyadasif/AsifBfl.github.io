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
    public class KitAssignedCustomer
    {
        [Key]
        public long KitAssignedCustomerId { get; set; }

        public string AccountNumber { get; set; }

        public DateTime AssignedDate { get; set; }

        public string CustomerName { get; set; }

        public DateTime CreatedDate { get; set; } 

        public DateTime? ProcessedDate { get; set; } 
    }
}
