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
    public interface IRoleService
    {
        Task<ResponseModel> GetRoleById(long id);
        Task<ResponseModel> GetAllRole(bool? isActive = null);
        Task<ResponseModel> GetRolePaged(Pagination pagination);
        Task<ResponseModel> CreateRole(Role entity);
        Task<ResponseModel> UpdateRole(Role updateEntity);
        Task<ResponseModel> DeleteRole(long id);
        Task<List<DropdownModel>> GetDropdwon(long? id = null, long? roleTypeId = null);
    }
}
