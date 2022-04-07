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
    public class IblBranch : BaseClass
    {
        [Key]
        public long IblBranchId { get; set; }

        [Required(ErrorMessage = "Please enter IBL branch name")]
        public string IblBranchName { get; set; }

        [Required(ErrorMessage = "Please enter IBL branch code")]
        public string IblBranchCode { get; set; }

        public int SortOrder { get; set; }
    }
}
