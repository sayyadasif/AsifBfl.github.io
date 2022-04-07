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
    public class C5Code : BaseClass
    {
        [Key]
        public long C5CodeId { get; set; }

        [Required(ErrorMessage = "Please enter C5 code")]
        public string C5CodeName { get; set; }

        [Required(ErrorMessage = "Please select card type")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select card type")]
        public long CardTypeId { get; set; }

        public int SortOrder { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        [ForeignKey(nameof(CardTypeId))]
        public virtual CardType CardType { get; set; }
    }
}
