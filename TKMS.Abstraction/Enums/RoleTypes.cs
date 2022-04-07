using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.Enums
{
    public enum RoleTypes
    {
        [Description("Head Office")]
        HO = 1,
        
        [Description("Region Office")]
        RO = 2,

        [Description("Branch")]
        BRANCH = 3,

        [Description("IBL CPU")]
        CPU = 4,

        [Description("Staff")]
        STAFF = 5,
        
        [Description("System Admin")]
        SA = 6
    }
}
