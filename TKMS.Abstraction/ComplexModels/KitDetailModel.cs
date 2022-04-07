using Core.Utility.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class KitDetailModel
    {
        public long KitId { get; set; }

        public DateTime KitDate { get; set; }

        public string KitDateFormatted => CommonUtils.GetFormatedDate(KitDate);

        public string IndentNo { get; set; }

        public string BranchName { get; set; }

        public string BranchCode { get; set; }

        public string BfilBranchCode { get; set; }

        public string CifNo { get; set; }

        public string AccountNo { get; set; }
    }
}
