using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class IblBranchModel
    {
        public long IblBranchId { get; set; }
        public string IblBranchName { get; set; }
        public string IblBranchCode { get; set; }
        public IEnumerable<string> Branches { get; set; } = new List<string>();
    }
}
