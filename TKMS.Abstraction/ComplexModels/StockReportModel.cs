using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class StockReportModel
    {
        public string BfilBranchCode { get; set; }
        public string BfilBranchName { get; set; }
        public int TotalIndented { get; set; }
        public int TotalDispatched { get; set; }
        public int TotalReceived { get; set; }
        public int TotalAllocated { get; set; }
        public int TotalDestructed { get; set; }
        public int TotalAssigned { get; set; }
    }
}
