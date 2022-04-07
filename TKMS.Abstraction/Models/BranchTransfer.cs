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
    public class BranchTransfer : BaseClass
    {
        [Key]
        public long BranchTransferId { get; set; }

        public long KitId { get; set; }

        public long FromBranchId { get; set; }

        public long ToBranchId { get; set; }

        public long TransferById { get; set; }

        public DateTime TransferDate { get; set; }

        public long? ReceivedById { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public string Remarks { get; set; }
    }
}
