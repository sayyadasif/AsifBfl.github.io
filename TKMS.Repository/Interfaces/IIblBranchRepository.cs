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
    public interface IIblBranchRepository : IRepository<IblBranch>
    {
        Task<PagedList> GetIblBranchPaged(Pagination pagination);
        Task<PagedList> GetDropdwon(long? id = null, bool? isActive = null);
    }
}
