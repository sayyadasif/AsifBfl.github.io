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
    public interface IBranchTypeRepository : IRepository<BranchType>
    {
        Task<PagedList> GetBranchTypePaged(Pagination pagination);
        Task<PagedList> GetDropdwon(bool? isAllowIndent = null, long? id = null, bool? isActive = null);
    }
}
