using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Abstraction.ComplexModels
{
    public class DisplayCard
    {
        public string CardTitle { get; set; }
        public string CardDetail { get; set; }
        public string CardLinkText { get; set; } = "View All";
        public string CardUrl { get; set; }
    }
}
