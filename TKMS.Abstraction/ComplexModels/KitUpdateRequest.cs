using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class KitUpdateRequest
    {
        public long KitId { get; set; }
        public long KitStatusId { get; set; }
        public long? KitDamageReasonId { get; set; }
        public string DamageRemarks { get; set; }
    }

    public class KitDestructionRequest
    {
        public long KitId { get; set; }
        public long KitStatusId { get; set; }
        public bool DestructionApproved { get; set; }
        public string DestructionRemarks { get; set; }
    }
}
