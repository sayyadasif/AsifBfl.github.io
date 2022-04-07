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
    public interface IBranchDispatchService
    {
        Task<ResponseModel> GetBranchDispatchById(long id);
        Task<ResponseModel> GetAllBranchDispatch(bool? isActive = null);
        Task<ResponseModel> GetBranchDispatchPaged(Pagination pagination);
        Task<ResponseModel> CreateBranchDispatch(BranchDispatch entity);
        Task<ResponseModel> UpdateBranchDispatch(BranchDispatch updateEntity);
        Task<ResponseModel> DeleteBranchDispatch(long id);
    }
}
