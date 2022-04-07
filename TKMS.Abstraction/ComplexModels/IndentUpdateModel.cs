using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JsonIgnoreRequest = System.Text.Json.Serialization.JsonIgnoreAttribute;
using JsonIgnoreResponse = Newtonsoft.Json.JsonIgnoreAttribute;

namespace TKMS.Abstraction.ComplexModels
{
    public class IndentUpdateModel
    {
        public long IndentId { get; set; }

        public long IndentStatusId { get; set; }

        public long? RejectionReasonId { get; set; }

        public bool IsApproved { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsRejected { get; set; }

        public string Remarks { get; set; }
    }
}
