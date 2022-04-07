using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class IndentDisplayModel
    {
        public bool AllowIndentCreate { get; set; }
        public bool IndentRaised { get; set; }
        public bool IndentRejected { get; set; }
        public bool IndentRejectedDetail { get; set; }
        public bool IndentDispatched { get; set; }
        public bool IndentCpuDispatched { get; set; }
        public bool IndentApproved { get; set; }
        public bool IndentReceived { get; set; }
        public bool IndentCpuReceived { get; set; }
        public bool IndentCancelled { get; set; }
        public bool IndentReceivedAtBranch { get; set; }
    }
}
