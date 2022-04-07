using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class BranchModel
    {
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string BranchType { get; set; }
        public string RegionName { get; set; }
        public string IblBranchName { get; set; }
        public string IblBranchCode { get; set; }
        public string IfscCode { get; set; }
        public bool IsActive { get; set; }
    }
}
