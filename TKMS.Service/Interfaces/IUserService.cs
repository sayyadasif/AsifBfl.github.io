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
    public interface IUserService
    {
        Task<ResponseModel> GetUserById(long id);
        Task<ResponseModel> GetUserByStaffId(string shortId);
        Task<ResponseModel> GetAllUser(bool? isActive = null);
        Task<ResponseModel> GetUserPaged(Pagination pagination);
        Task<ResponseModel> CreateUser(User entity);
        Task<ResponseModel> CreateUsers(List<User> entities);
        Task<ResponseModel> UpdateUser(User updateEntity);
        Task<ResponseModel> DeleteUser(long id);
        Task<ResponseModel> AuthenticateUser(LoginModel model);
        Task<ResponseModel> AuthenticateBfilUser(LoginModel model);
        Task<List<DropdownModel>> GetDropdwon(long? id = null, long? branchId = null, long? roleTypeId = null, bool? isActive = null);
    }
}
