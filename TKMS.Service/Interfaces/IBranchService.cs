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
    public interface IBranchService
    {
        Task<ResponseModel> GetBranchById(long id);
        Task<ResponseModel> GetBranchByCode(string branchCode);
        Task<ResponseModel> GetAllBranch(bool? isActive = null);
        Task<ResponseModel> GetBranchPaged(Pagination pagination);
        Task<ResponseModel> CreateBranch(Branch entity);
        Task<ResponseModel> CreateBranches(List<Branch> entities);
        Task<ResponseModel> IblBranchMapping(List<IblBranchMapping> entities);
        Task<ResponseModel> UpdateBranch(Branch updateEntity);
        Task<ResponseModel> UpdateBranches(List<Branch> updateEntities);
        Task<ResponseModel> DeleteBranch(long id);
        Task<List<BranchDropdownModel>> GetDropdwon(long? regionId = null, long? branchId = null, long? id = null);
    }
}
