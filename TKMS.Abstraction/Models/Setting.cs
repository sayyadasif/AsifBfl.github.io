using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonIgnoreRequest = System.Text.Json.Serialization.JsonIgnoreAttribute;
using JsonIgnoreResponse = Newtonsoft.Json.JsonIgnoreAttribute;

namespace TKMS.Abstraction.Models
{
    public class Setting : BaseClass
    {
        [Key]
        public long SettingId { get; set; }

        public string SettingName { get; set; }

        public string SettingKey { get; set; }

        [Required(ErrorMessage = "Please enter setting value")]
        public string SettingValue { get; set; }

        public string SettingDesc { get; set; }

        public bool IsEditable { get; set; }
    }
}
