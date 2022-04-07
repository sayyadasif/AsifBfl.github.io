using Core.Utility.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class KitReturnModel
    {
        public long KitReturnId { get; set; }

        public long KitId { get; set; }

        public string ReturnBy { get; set; }

        public DateTime ReturnDate { get; set; }

        public string ReturnAcceptedBy { get; set; }
    }
}
