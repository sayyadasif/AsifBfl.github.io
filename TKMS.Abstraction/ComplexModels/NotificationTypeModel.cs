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
    public class NotificationTypeModel
    {
        [Key]
        public long NotificationTypeId { get; set; }

        public string NotificationTypeName { get; set; }

        public string NotificationTemplate { get; set; }

        public string UrlTemplate { get; set; }

        public string Description { get; set; }

        public string VisibleToRoles { get; set; }

    }
}
