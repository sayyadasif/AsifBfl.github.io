using System.Collections.Generic;

namespace Core.Repository.Models
{
    public class UserClaim
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public long BranchId { get; set; }
        public string BranchCode { get; set; }
        public long RoleTypeId { get; set; }
        public long RegionId { get; set; }
        public List<long> RoleIds { get; set; } = new List<long>();
    }
}
