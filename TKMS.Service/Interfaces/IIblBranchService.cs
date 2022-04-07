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
    public interface IIblBranchService
    {
        Task<ResponseModel> GetIblBranchById(long id);
        Task<ResponseModel> GetAllIblBranch(bool? isActive = null);
        Task<ResponseModel> GetIblBranchPaged(Pagination pagination);
        Task<ResponseModel> CreateIblBranch(IblBranch entity);
        Task<ResponseModel> CreateIblBranches(List<IblBranch> entities);
        Task<ResponseModel> UpdateIblBranch(IblBranch updateEntity);
        Task<ResponseModel> DeleteIblBranch(long id);
        Task<List<IblBranchDropdownModel>> GetDropdwon(long? id = null, bool? isActive = null);
    }
}
