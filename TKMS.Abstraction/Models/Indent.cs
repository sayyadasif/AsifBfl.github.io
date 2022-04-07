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
    public class Indent : BaseClass
    {
        [Key]
        public long IndentId { get; set; }

        [Required(ErrorMessage = "Please select scheme code")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select scheme code")]
        public long SchemeCodeId { get; set; }

        [Required(ErrorMessage = "Please select c5 code")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select c5 code")]
        public long C5CodeId { get; set; }

        [Required(ErrorMessage = "Please select card type")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select card type")]
        public long CardTypeId { get; set; }

        [Required(ErrorMessage = "Please select address type")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select address type")]
        public long BfilBranchTypeId { get; set; }

        [Required(ErrorMessage = "Please select bfil region")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select bfil region")]
        public long BfilRegionId { get; set; }

        [Required(ErrorMessage = "Please select bfil branch")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select bfil branch")]
        public long BfilBranchId { get; set; }

        [Required(ErrorMessage = "Please select ibl branch")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select ibl branch")]
        public long IblBranchId { get; set; }

        public long DispatchAddressId { get; set; }

        public long IndentStatusId { get; set; }

        [Required]
        public string IndentNo { get; set; }

        [Required(ErrorMessage = "Please enter date")]
        public DateTime IndentDate { get; set; }

        [Required(ErrorMessage = "Please enter number of kits")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please enter number of kits")]
        public int NoOfKit { get; set; }

        [Required(ErrorMessage = "Please enter contact name")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Please enter contact number")]
        [RegularExpression(@"^(?:\s*\d{10}\s*(?:,|$))+$", ErrorMessage = "Please enter valid contact number(s)")]
        public string ContactNo { get; set; }

        public long? RejectionReasonId { get; set; }

        public long? HoApproveStatusId { get; set; }

        public long? HoApproveBy { get; set; }

        public DateTime? HoApproveDate { get; set; }

        public long? CpuApproveStatusId { get; set; }

        public long? CpuApproveBy { get; set; }

        public DateTime? CpuApproveDate { get; set; }

        public string Remarks { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        public bool IsAddressUpdated { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        public string IfscCode { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        public string RejectionReason { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        [ForeignKey(nameof(DispatchAddressId))]
        public virtual Address DispatchAddress { get; set; } = new Address();

    }
}
