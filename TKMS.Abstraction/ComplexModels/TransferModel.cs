using Core.Utility.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class TransferModel
    {

        public string KitIds { get; set; }

        [Required(ErrorMessage = "Please select branch")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select branch")]
        public long ToBranchId { get; set; }

        public string ToBranchName { get; set; }

        public string Remarks { get; set; }

        public List<long> Kits => KitIds.Split(',').Select(long.Parse).ToList();
        public List<KitModel> KitDetails { get; set; } = new List<KitModel>();
    }
}
