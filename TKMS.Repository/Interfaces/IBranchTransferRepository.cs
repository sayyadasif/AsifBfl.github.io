using Core.Repository;
using Core.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Models;

namespace TKMS.Repository.Interfaces
{
    public interface IBranchTransferRepository : IRepository<BranchTransfer>
    {
        Task<PagedList> GetBranchTransferPaged(Pagination pagination);
        Task<BranchTransferModel> GetTransferDetailById(long id);
        Task<TransferKitDetailModel> GetTransferKitByAccountNo(dynamic filters);
    }
}
