using Core.Utility.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class BranchTransferModel
    {
        public long BranchTransferId { get; set; }

        public long KitId { get; set; }

        public string IndentNo { get; set; }

        public string AccountNo { get; set; }

        public string CifNo { get; set; }

        public string FromBranchCode { get; set; }

        public string FromBranchName { get; set; }

        public string ToBranchCode { get; set; }

        public string CardType { get; set; }

        public string TransferByStaffId { get; set; }

        public string TransferByName { get; set; }

        public DateTime TransferDate { get; set; }

        public string ReceivedByStaffId { get; set; }

        public string ReceivedByName { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public string Remarks { get; set; }
    }
}
