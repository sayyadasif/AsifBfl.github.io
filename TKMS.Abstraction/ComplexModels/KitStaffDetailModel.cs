using Core.Utility.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class KitStaffDetailModel
    {
        public string StaffId { get; set; }

        public string StaffName { get; set; }

        public IEnumerable<KitStaffDetail> KitAllocated { get; set; }

        public IEnumerable<KitStaffDetail> KitAssigned { get; set; }
    }

    public class KitStaffDetail
    {
        public string AccountNo { get; set; }

        public string CifNo { get; set; }

        public DateTime AllocatedDate { get; set; }

        public DateTime? AssignedDate { get; set; }

        public string CustomerName { get; set; }

        public int Age => (CommonUtils.GetDefaultDateTime().Date - AllocatedDate.Date).Days;

        public string AgeDetail => Age > 1 ? $"{Age} Days" : $"{Age} Day";

        public bool KitAlert { get; set; }

        public bool ReturnAlert { get; set; }
    }
}
