using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class BranchTransferUpdateRequest
    {
        public long BranchTransferId { get; set; }
        public long KitStatusId { get; set; }
        public long? KitDamageReasonId { get; set; }
        public string DamageRemarks { get; set; }
    }
}
