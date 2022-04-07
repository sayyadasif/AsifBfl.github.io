using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.Enums;

namespace TKMS.Abstraction.ComplexModels
{
    public class IndentModel
    {
        public long IndentId { get; set; }
        public string IndentNo { get; set; }
        public DateTime IndentDate { get; set; }
        public string IblBranchCode { get; set; }
        public string IblBranchName { get; set; }
        public string SchemeCode { get; set; }
        public string C5Code { get; set; }
        public string CardType { get; set; }
        public int NoOfKit { get; set; }
        public int StockPresent { get; set; }
        public int NoOfKitDispatched { get; set; }
        public int NoOfKitCancelled => IndentStatusId == IndentStatuses.Cancelled.GetHashCode() ? NoOfKit - NoOfKitDispatched : 0;
        public string BfilBranchType { get; set; }
        public string BfilBranchCode { get; set; }
        public string IfscCode { get; set; }
        public string RejectedReason { get; set; }
        public string IndentStatus { get; set; }
        public long IndentStatusId { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
    }
}
