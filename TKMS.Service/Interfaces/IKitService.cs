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
    public interface IKitService
    {
        Task<ResponseModel> GetKitById(long id);
        Task<ResponseModel> GetAllKit(bool? isActive = null);
        Task<ResponseModel> GetKitPaged(Pagination pagination);
        Task<ResponseModel> GetKitStaffPaged(Pagination pagination);
        Task<ResponseModel> CreateKit(Kit entity);
        Task<ResponseModel> CreateKits(List<Kit> entities);
        Task<ResponseModel> UpdateKit(Kit updateEntity);
        Task<ResponseModel> UpdateKitsAllocated(KitAllocateModel updateEntity);
        Task<ResponseModel> UpdateKitsDestructed(KitDestructModel updateEntity);
        Task<ResponseModel> UpdateKitDetails(KitUpdateRequest updateEntity);
        Task<ResponseModel> UpdateKitDestruction(KitDestructionRequest updateEntity);
        Task<ResponseModel> UpdateKitReturned(KitAllocateModel updateEntity);
        Task<ResponseModel> UpdateKitAssigned();
        Task<ResponseModel> DeleteKit(long id);
        Task<KitModel> GetKitDetailById(long id);
        Task<ResponseModel> GetKitByAccountNo(string accountNo, long? kitStatusId = null);
        Task<int> GetAllocatedKitCount(long staffId, long kitStatusId);
        Task<KitStaffDetailModel> KitStaffDetailByAllocatedId(long userId);
        Task<long> GetKitCount(dynamic filters = null);
        Task<bool> IsSingleStaff(List<long> kitIds);
        Task<List<KitModel>> GetStaffKits(List<long> kitIds);
        Task<List<string>> ValidateKitAccountNo(List<string> accountNos);
    }
}
