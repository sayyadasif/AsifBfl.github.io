using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class KitDisplayModel
    {
        public bool KitDetail { get; set; }
        public bool KitForAllocation   { get; set; }
        public bool KitAllocated { get; set; }
        public bool KitAllocatedStaffWise { get; set; }
        public bool KitDestruction { get; set; }
        public bool KitDestructionCpu { get; set; }
        public bool KitAssigned { get; set; }
        public bool KitDetailStaff { get; set; }
        public bool KitDestructionHo { get; set; }
        public bool KitDestructionApproved { get; set; }
        public bool KitDestructionRejected { get; set; }
    }
}
