using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.Models;

namespace TKMS.Abstraction.ComplexModels
{
    public class IndentDetailModel
    {
        public long IndentId { get; set; }
        public long IndentStatusId { get; set; }
        public long BfilBranchTypeId { get; set; }
        public long BfilBranchId { get; set; }
        public DateTime IndentDate { get; set; }
        public string IndentStatus { get; set; }
        public string IndentNo { get; set; }
        public string IblBranchCode { get; set; }
        public string IblBranchName { get; set; }
        public string SchemeCode { get; set; }
        public string C5Code { get; set; }
        public string CardType { get; set; }
        public int NoOfKit { get; set; }
        public int NoOfKitDispatched { get; set; }
        public string BfilBranchType { get; set; }
        public string BfilBranchCode { get; set; }
        public string ContactName { get; set; }
        public string ContactNo { get; set; }
        public string IfscCode { get; set; }
        public string RejectedReason { get; set; }
        public string Remarks { get; set; }
        public Address DispatchAddress { get; set; }
        public List<DispatchModel> DispatchModels { get; set; } = new List<DispatchModel>();
    }
}
