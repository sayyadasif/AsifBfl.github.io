using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class UserModel
    {
        public long UserId { get; set; }
        public string StaffId { get; set; }
        public string FullName { get; set; }
        public string BranchName { get; set; }
        public string MobileNo { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
