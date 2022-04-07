using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.Enums;

namespace TKMS.Abstraction.ComplexModels
{
    public class AlertNotificationModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationTypes Type { get; set; }
    }
}
