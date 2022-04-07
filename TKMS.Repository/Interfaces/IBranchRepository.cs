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
    public interface IBranchRepository : IRepository<Branch>
    {
        Task<PagedList> GetBranchPaged(Pagination pagination);
        Task<PagedList> GetDropdwon(long? regionId = null, long? branchId = null, long? id = null);
    }
}
