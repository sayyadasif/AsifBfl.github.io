using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class KitAssignedCustomerModel
    {
        public long KitAssignedCustomerId { get; set; }

        public string AccountNumber { get; set; }

        public DateTime AssignedDate { get; set; }

        public string CustomerName { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ProcessedDate { get; set; }
    }
}
