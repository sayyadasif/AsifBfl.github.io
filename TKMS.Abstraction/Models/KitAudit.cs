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
    public class KitAudit : BaseClass
    {
        [Key]
        public long KitAuditId { get; set; }

        public long KitId { get; set; }

        public long IndentId { get; set; }

        public string CifNo { get; set; }

        public string AccountNo { get; set; }

        public long KitStatusId { get; set; }

        public long? AllocatedToId { get; set; }

        public DateTime? AllocatedDate { get; set; }

        public long? AllocatedById { get; set; }

        public long? KitDamageReasonId { get; set; }

        public string DamageRemarks { get; set; }

        public bool? IsDestructionApproved { get; set; }

        public long? DestructionById { get; set; }

        public string DestructionRemarks { get; set; }

        public string CustomerName { get; set; }

        public string Remarks { get; set; }
    }
}
