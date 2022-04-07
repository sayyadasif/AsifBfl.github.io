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
    public class DispatchWayBill : BaseClass
    {
        [Key]
        public long DispatchWayBillId { get; set; }

        public long DispatchId { get; set; }

        public string WayBillNo { get; set; }

        public DateTime DispatchDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public long CourierStatusId { get; set; }

        public string  ReceiveBy { get; set; }

        public long? ReceiveStatusId { get; set; }

        public long? ReportUploadBy { get; set; }

        public DateTime? ReportUploadDate { get; set; }

        public long DispatchStatusId { get; set; }

        public string CourierPartner { get; set; }

        public string Remarks { get; set; }


        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        [ForeignKey(nameof(DispatchId))]
        public virtual Dispatch Dispatch { get; set; }
    }
}
