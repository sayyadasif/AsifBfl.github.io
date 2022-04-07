using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.Enums
{
    public enum Roles
    {
        [Description("HO Admin")]
        HOAdmin = 1,

        [Description("HO Indent Maker")]
        HOIndentMaker = 2,
        
        [Description("HO Approver")]
        HOApprover = 3,
        
        [Description("RO Indent Maker")]
        ROIndentMaker = 4,

        [Description("RO Kit Management")]
        ROKitManagement = 5,

        [Description("BM")]
        BM = 6,

        [Description("BCM")]
        BCM = 7,
        
        [Description("IBL CPU")]
        IBLCPU = 8,

        [Description("Staff")]
        Staff = 9,

        [Description("System Admin")]
        SystemAdmin = 10,
    }
}
