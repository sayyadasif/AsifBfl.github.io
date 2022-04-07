using Core.Utility.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class KitModel
    {
        public long KitId { get; set; }

        public long KitStatusId { get; set; }

        public string KitStatus { get; set; }

        public string IndentNo { get; set; }

        public string AccountNo { get; set; }

        public string CifNo { get; set; }

        public string IblBranchCode { get; set; }

        public string IblBranchName { get; set; }

        public string BfilBranchCode { get; set; }

        public string CardType { get; set; }

        public string StaffId { get; set; }

        public string StaffName { get; set; }

        public DateTime? AllocatedDate { get; set; }

        public long? KitDamageReasonId { get; set; }

        public string KitDamageReason { get; set; }

        public string DamageRemarks { get; set; }

        public bool? IsDestructionApproved { get; set; }

        public string DestructionRemarks { get; set; }

        public string CustomerName { get; set; }

        public string Remarks { get; set; }

        public DateTime KitDate { get; set; }

        public int Age => AllocatedDate.HasValue ? (CommonUtils.GetDefaultDateTime().Date - AllocatedDate.Value.Date).Days : 0;

        public string AgeDetail => Age > 1 ? $"{Age} Days" : $"{Age} Day";

        public bool KitAlert { get; set; }

        public bool AllowReAllocate { get; set; }
    }
}
