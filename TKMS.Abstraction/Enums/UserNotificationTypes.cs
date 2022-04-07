using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.Enums
{
    public enum UserNotificationTypes
    {
        [Description("Receipt Confirmation")]
        ReceiptConfirmation = 1,
        
        [Description("Scan Kit Confirmation ")]
        ScanKitConfirmation = 2,

        [Description("Kit Return After Allocation")]
        KitRetunAfterAllocation = 3,
    }
}
