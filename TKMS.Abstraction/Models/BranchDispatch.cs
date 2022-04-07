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
    public class BranchDispatch : BaseClass
    {
        [Key]
        public long BranchDispatchId { get; set; }

        public long DispatchId { get; set; }

        public string DispatchMode { get; set; }

        [Required(ErrorMessage = "Please enter staff Id")]
        public string StaffId { get; set; }

        [Required(ErrorMessage = "Please enter staff Name")]
        public string StaffName { get; set; }

        [Required(ErrorMessage = "Please enter staff contact number")]
        public string StaffContactNo { get; set; }

        [Required(ErrorMessage = "Please enter courier name")]
        public string CourierName { get; set; }

        [Required(ErrorMessage = "Please enter waybill number")]
        public string WayBillNo { get; set; }

        public string Remarks { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        public long IndentId { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [ForeignKey(nameof(DispatchId))]
        public virtual Dispatch Dispatch { get; set; }
    }
}
