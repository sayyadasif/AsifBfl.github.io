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
    public class User : BaseClass
    {
        [Key]
        public long UserId { get; set; }

        [Required(ErrorMessage = "Please enter staff id")]
        public string StaffId { get; set; }

        [Required(ErrorMessage = "Please enter full name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please select branch")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select branch")]
        public long BranchId { get; set; }

        [Required(ErrorMessage = "Please select role type")]
        [RegularExpression("(.*[1-9].*)|(.*[.].*[1-9].*)", ErrorMessage = "Please select role type")]
        public long RoleTypeId { get; set; }

        [EmailAddress(ErrorMessage = "Please enter valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }

        public string Salt { get; set; }

        [Required(ErrorMessage = "Please enter mobile number")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Please enter valid mobile no")]
        [StringLength(10, ErrorMessage = "Please enter valid mobile no", MinimumLength = 10)]
        [MaxLength(10)]
        public string MobileNo { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [ForeignKey(nameof(BranchId))]
        public virtual Branch Branch { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        public bool IsUpdatePassword { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        public long[] SelectedRoles { get; set; } = Array.Empty<long>();

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [NotMapped]
        public string OldPassword { get; set; }

        [JsonIgnoreRequest]
        [NotMapped]
        public string BranchCode { get; set; }

    }
}
