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
    public interface IRoleTypeService
    {
        Task<ResponseModel> GetRoleTypeById(long id);
        Task<ResponseModel> GetAllRoleType(bool? isActive = null);
        Task<ResponseModel> GetRoleTypePaged(Pagination pagination);
        Task<ResponseModel> CreateRoleType(RoleType entity);
        Task<ResponseModel> UpdateRoleType(RoleType updateEntity);
        Task<ResponseModel> DeleteRoleType(long id);
        Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null);
    }
}
