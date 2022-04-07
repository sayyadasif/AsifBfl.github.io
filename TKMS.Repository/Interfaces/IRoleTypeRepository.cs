using Core.Repository;
using Core.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.Models;

namespace TKMS.Repository.Interfaces
{
    public interface IRoleTypeRepository : IRepository<RoleType>
    {
        Task<PagedList> GetRoleTypePaged(Pagination pagination);
        Task<PagedList> GetDropdwon(long? id = null, bool? isActive = null);
    }
}
