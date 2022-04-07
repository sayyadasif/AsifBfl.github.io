using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class DispatchNotificationModel
    {
        public long DispatchId { get; set; }

        public long BranchId { get; set; }

        public DateTime? DispatchDate { get; set; }

        public string ReferenceNo { get; set; }

        public long? KitId { get; set; }

        public DateTime? AllocatedDate { get; set; }

        public string AccountNo { get; set; }

        public string IndentNo { get; set; }

        public DateTime? BranchDispatchDate { get; set; }

        public DateTime? BranchReceiveDate { get; set; }

    }
}
