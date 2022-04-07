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
    public class IndentAudit : BaseClass
    {
        [Key]
        public long IndentAuditId { get; set; }

        public long IndentId { get; set; }

        public long SchemeCodeId { get; set; }

        public long C5CodeId { get; set; }

        public long CardTypeId { get; set; }

        public long BfilBranchTypeId { get; set; }

        public long BfilRegionId { get; set; }

        public long BfilBranchId { get; set; }

        public long IblBranchId { get; set; }

        public long DispatchAddressId { get; set; }

        public long IndentStatusId { get; set; }

        public string IndentNo { get; set; }

        public DateTime IndentDate { get; set; }

        public int NoOfKit { get; set; }

        public string ContactName { get; set; }

        public string ContactNo { get; set; }

        public long? RejectionReasonId { get; set; }

        public long? HoApproveStatus { get; set; }

        public long? HoApproveBy { get; set; }

        public DateTime? HoApproveDate { get; set; }

        public long? CpuApproveStatus { get; set; }

        public long? CpuApproveBy { get; set; }

        public DateTime? CpuApproveDate { get; set; }

        public string Remarks { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
