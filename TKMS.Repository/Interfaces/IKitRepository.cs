using Core.Repository;
using Core.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Models;

namespace TKMS.Repository.Interfaces
{
    public interface IKitRepository : IRepository<Kit>
    {
        Task<PagedList> GetKitPaged(Pagination pagination);
        Task<PagedList> GetKitStaffPaged(Pagination pagination);
        Task<KitModel> GetKitDetailById(long id);
        Task<KitDetailModel> GetKitByAccountNo(dynamic filters = null);
        Task<int> GetAllocatedKitCount(long staffId, long kitStatusId);
        Task<KitStaffDetailModel> KitStaffDetailByAllocatedId(long userId);
        Task<long> GetKitCount(dynamic filters = null);
        Task<bool> IsSingleStaff(List<long> kitIds);
        Task<List<KitModel>> GetStaffKits(List<long> kitIds);
        Task<List<string>> ValidateKitAccountNo(List<string> accountNos);
    }
}
