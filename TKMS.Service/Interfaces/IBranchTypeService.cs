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
    public interface IBranchTypeService
    {
        Task<ResponseModel> GetBranchTypeById(long id);
        Task<ResponseModel> GetAllBranchType(bool? isActive = null);
        Task<ResponseModel> GetBranchTypePaged(Pagination pagination);
        Task<ResponseModel> CreateBranchType(BranchType entity);
        Task<ResponseModel> UpdateBranchType(BranchType updateEntity);
        Task<ResponseModel> DeleteBranchType(long id);
        Task<List<DropdownModel>> GetDropdwon(bool? isAllowIndent = null, long? id = null, bool? isActive = null);
    }
}
