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
    public class Dispatch : BaseClass
    {
        [Key]
        public long DispatchId { get; set; }

        public long IndentId { get; set; }

        public string AccountStart { get; set; }

        public string AccountEnd { get; set; }

        public string Vendor { get; set; }

        public int DispatchQty { get; set; }

        public DateTime DispatchDate { get; set; }

        public string SchemeType { get; set; }

        public string ReferenceNo { get; set; }

        public long DispatchStatusId { get; set; }

        public DateTime? BranchDispatchDate { get; set; }

        public DateTime? BranchReceiveDate { get; set; }

        public string Remarks { get; set; }

        public virtual ICollection<DispatchWayBill> DispatchWayBills { get; set; } = new List<DispatchWayBill>();

        public virtual ICollection<Kit> Kits { get; set; } = new List<Kit>();

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        public string IndentNo { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        public DateTime IndentDate { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        public string IblBranchCode { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        public string SchemeCode { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        public string CardType { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        public string BfilBranchCode { get; set; }
    }
}
