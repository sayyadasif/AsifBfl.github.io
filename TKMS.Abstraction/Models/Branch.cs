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
    public class Branch : BaseClass
    {
        [Key]
        public long BranchId { get; set; }

        [Required(ErrorMessage = "Please enter branch name")]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Please enter branch code")]
        public string BranchCode { get; set; }

        [Required(ErrorMessage = "Please select region")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select region")]
        public long RegionId { get; set; }

        [Required(ErrorMessage = "Please select IBL branch")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select IBL branch")]
        public long IblBranchId { get; set; }

        [Required(ErrorMessage = "Please select branch type")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select branch type")]
        public long BranchTypeId { get; set; }

        [Required(ErrorMessage = "Please select scheme code")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select scheme code")]
        public long SchemeCodeId { get; set; }

        [Required(ErrorMessage = "Please select c5 code")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select c5 code")]
        public long C5CodeId { get; set; }

        [Required(ErrorMessage = "Please select card type")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select card type")]
        public long CardTypeId { get; set; }

        public long AddressId { get; set; }

        [Required(ErrorMessage = "Please enter branch ifsc code")]
        [StringLength(11, ErrorMessage = "Please enter valid ifsc code", MinimumLength = 11)]
        [MaxLength(11)]
        public string IfscCode { get; set; }

        public int SortOrder { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [ForeignKey(nameof(AddressId))]
        public virtual Address Address { get; set; }

    }
}
