using Core.Utility.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class KitStaffModel
    {
        public long UserId { get; set; }

        public string StaffId { get; set; }

        public string StaffName { get; set; }

        public int KitAllocated { get; set; }

        public int KitAssigned { get; set; }

        public int KitRemaining => KitAllocated - KitAssigned;
    }
}
