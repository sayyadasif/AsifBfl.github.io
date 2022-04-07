using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonIgnoreRequest = System.Text.Json.Serialization.JsonIgnoreAttribute;
using JsonIgnoreResponse = Newtonsoft.Json.JsonIgnoreAttribute;

namespace TKMS.Abstraction.ComplexModels
{
    public class NotificationModel
    {
        [Key]
        public long NotificationId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public long? DispatchId { get; set; }

        public long? KitId { get; set; }

        public string RedirectUrl { get; set; }

        public DateTime NotificationDate { get; set; }

        public string VisibleToRoles { get; set; }

        public bool IsActive { get; set; }

        [JsonIgnoreRequest]
        [JsonIgnoreResponse]
        public List<long> VisibleRoles => VisibleToRoles.Split(',').Select(long.Parse).ToList();

    }
}
