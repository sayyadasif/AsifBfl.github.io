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
    public class Address : BaseClass
    {
        [Key]
        public long AddressId { get; set; }

        [Required(ErrorMessage = "Please enter address detail")]
        public string AddressDetail { get; set; }

        public string Region { get; set; }

        public string Zone { get; set; }

        public string District { get; set; }

        public string State { get; set; }


        [Required(ErrorMessage = "Please enter pincode")]
        [StringLength(6, ErrorMessage = "Please enter valid pincode", MinimumLength = 6)]
        [MaxLength(6)]
        public string PinCode { get; set; }

    }
}
