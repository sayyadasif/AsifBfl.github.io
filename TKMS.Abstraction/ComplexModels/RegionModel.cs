using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.Models;
using JsonIgnoreRequest = System.Text.Json.Serialization.JsonIgnoreAttribute;
using JsonIgnoreResponse = Newtonsoft.Json.JsonIgnoreAttribute;

namespace TKMS.Abstraction.ComplexModels
{
    public class RegionModel
    {
        public long RegionId { get; set; }
        public string SystemRoId { get; set; }
        public string RegionName { get; set; }
        public bool IsActive { get; set; }

        [JsonIgnoreResponse]
        [JsonIgnoreRequest]
        public Address Address { get; set; }

        public string AddressDetail => Address.AddressDetail;

        public string Region => Address.Region;

        public string Zone => Address.Zone;

        public string District => Address.District;

        public string State => Address.State;

        public string PinCode => Address.PinCode;
    }
}
