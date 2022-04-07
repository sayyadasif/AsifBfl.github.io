using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class DispatchWayBillModel
    {
        public long DispatchWayBillId { get; set; }

        public long DispatchId { get; set; }

        public string WayBillNo { get; set; }

        public DateTime DispatchDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string CourierStatus { get; set; }

        public string ReceiveBy { get; set; }

        public string ReceiveStatus { get; set; }

        public string DispatchStatus { get; set; }

        public string CourierPartner { get; set; }

        public string Remarks { get; set; }
    }
}
