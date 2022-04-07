using Core.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class DashboardModel
    {
        public UserClaim UserClaim { get; set; }
        public bool AllowIndentCreate { get; set; }
        public bool DisplayRejected { get; set; }
        public bool DisplayDispatched { get; set; }
        public DisplayCard KitDestruction { get; set; } = new DisplayCard();
        public DisplayCard NoOfKitsCard { get; set; } = new DisplayCard();
        public DisplayCard KitsAllocationCard { get; set; } = new DisplayCard();
        public List<StatusCount> KitAllocationAge { get; set; } = new List<StatusCount>();
        public List<DisplayCard> DisplayCards { get; set; } = new List<DisplayCard>();
        public List<DisplayCard> DispatchCards { get; set; } = new List<DisplayCard>();
        public List<DisplayCard> KitCards { get; set; } = new List<DisplayCard>();
        public List<NotificationModel> Notifications { get; set; } = new List<NotificationModel>();
    }

    public class StatusCount
    {
        public string StatusName { get; set; }
        public string Color { get; set; }
        public long Count { get; set; }
    }

    public class Reminder
    {
        public string ReminderTime { get; set; }
        public string ReminderDetail { get; set; }
        public string RedirectUrl { get; set; }
    }
}
