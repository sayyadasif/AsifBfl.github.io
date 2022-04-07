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
    public class DispatchAudit : BaseClass
    {
        [Key]
        public long DispatchAuditId { get; set; }

        public long DispatchId { get; set; }

        public long IndentId { get; set; }

        public string AccountStart { get; set; }

        public string AccountEnd { get; set; }

        public string Vendor { get; set; }

        public int DispatchQty { get; set; }

        public DateTime DispatchDate { get; set; }

        public string SchemeType { get; set; }

        public string ReferenceNo { get; set; }

        public long DispatchStatusId { get; set; }

        public DateTime? BranchDispatchDate { get; set; }

        public DateTime? BranchReceiveDate { get; set; }

        public string Remarks { get; set; }

    }
}
