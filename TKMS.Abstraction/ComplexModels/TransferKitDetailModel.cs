using Core.Utility.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class TransferKitDetailModel
    {
        public long BranchTransferId { get; set; }

        public DateTime TransferDate { get; set; }

        public string TransferDateFormatted => CommonUtils.GetFormatedDate(TransferDate);

        public string IndentNo { get; set; }

        public string BranchName { get; set; }

        public string BranchCode { get; set; }

        public string BfilBranchCode { get; set; }

        public string CifNo { get; set; }

        public string AccountNo { get; set; }
    }
}
