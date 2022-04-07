using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonIgnoreRequest = System.Text.Json.Serialization.JsonIgnoreAttribute;
using JsonIgnoreResponse = Newtonsoft.Json.JsonIgnoreAttribute;

namespace TKMS.Abstraction.Models
{
    public class Notification : BaseClass
    {
        [Key]
        public long NotificationId { get; set; }

        public long NotificationTypeId { get; set; }

        public long BranchId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string VisibleToRoles { get; set; }

        public long? DispatchId { get; set; }

        public long? KitId { get; set; }

        public string RedirectUrl { get; set; }
    }
}
