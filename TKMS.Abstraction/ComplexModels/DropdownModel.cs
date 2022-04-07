using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class DropdownModel
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public bool IsActive { get; set; }
    }

    public class BranchDropdownModel : DropdownModel
    {
        public long RegionId { get; set; }
        public string BranchCode { get; set; }
    }

    public class IblBranchDropdownModel : DropdownModel
    {
        public string IblBranchCode { get; set; }
    }

    public class C5CodeDropdownModel : DropdownModel
    {
        public long CardTypeId { get; set; }
        public string CardTypeName { get; set; }
    }
}
