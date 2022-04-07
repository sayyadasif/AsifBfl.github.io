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
    public interface IRoleRepository : IRepository<Role>
    {
        Task<PagedList> GetRolePaged(Pagination pagination);
        Task<PagedList> GetDropdwon(long? id = null, long? roleTypeId = null);
    }
}
