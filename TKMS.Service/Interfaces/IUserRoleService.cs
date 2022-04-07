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
    public interface IUserRoleService
    {
        Task<ResponseModel> GetUserRoleById(long id);
        Task<ResponseModel> GetAllUserRole(long userId);
        Task<ResponseModel> GetUserRolePaged(Pagination pagination);
        Task<ResponseModel> CreateUserRole(UserRole entity);
        Task<ResponseModel> UpdateUserRole(UserRole updateEntity);
        Task<ResponseModel> DeleteUserRole(long id);
    }
}
