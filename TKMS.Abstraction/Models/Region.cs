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
    public class Region : BaseClass
    {
        [Key]
        public long RegionId { get; set; }

        [Required(ErrorMessage = "Please enter system ro id")]
        public string SystemRoId { get; set; }

        [Required(ErrorMessage = "Please enter region name")]
        public string RegionName { get; set; }

        public int SortOrder { get; set; }

        public long AddressId { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [ForeignKey(nameof(AddressId))]
        public virtual Address Address { get; set; } = new Address();
    }
}
