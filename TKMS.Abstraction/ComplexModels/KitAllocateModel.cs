using Core.Utility.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.Models;

namespace TKMS.Abstraction.ComplexModels
{
    public class KitAllocateModel
    {

        public string KitIds { get; set; }

        [Required(ErrorMessage = "Please select staff Id")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select staff Id")]
        public long StaffId { get; set; }

        public string StaffName { get; set; }

        public string SelectedStaffId { get; set; }

        public bool AllowOTPAuth { get; set; }

        public List<KitModel> KitDetails { get; set; } = new List<KitModel>();

        public List<long> Kits => KitIds.Split(',').Select(long.Parse).ToList();
    }

    public class KitDestructModel
    {
        public string KitIds { get; set; }

        [Required(ErrorMessage = "Please select destruction reason")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select destruction reason")]
        public long KitDamageReasonId { get; set; }

        public string Remarks { get; set; }

        public List<KitModel> KitDetails { get; set; } = new List<KitModel>();

        public List<long> Kits => KitIds.Split(',').Select(long.Parse).ToList();
    }
}
