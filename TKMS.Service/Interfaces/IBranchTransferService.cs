using Core.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Models;

namespace TKMS.Service.Interfaces
{
    public interface IBranchTransferService
    {
        Task<ResponseModel> GetBranchTransferById(long id);
        Task<BranchTransferModel> GetTransferDetailById(long id);
        Task<ResponseModel> GetAllBranchTransfer(bool? isActive = null);
        Task<ResponseModel> GetBranchTransferPaged(Pagination pagination);
        Task<ResponseModel> CreateBranchTransfer(BranchTransfer entity);
        Task<ResponseModel> CreateBranchTransfers(TransferModel model);
        Task<ResponseModel> UpdateBranchTransfer(BranchTransferUpdateRequest updateEntity);
        Task<ResponseModel> UpdateBranchTransfers(List<long> ids);
        Task<ResponseModel> DeleteBranchTransfer(long id);
        Task<ResponseModel> GetTransferKitByAccountNo(string accountNo);
    }
}
