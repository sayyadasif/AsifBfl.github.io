using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class DispatchModel
    {
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

        public string DispatchStatus { get; set; }

        public string Remarks { get; set; }

        public ICollection<DispatchWayBillModel> DispatchWayBills { get; set; } = new List<DispatchWayBillModel>();

    }
}
