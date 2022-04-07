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
    public interface IIWorksKitService
    {
        Task<ResponseModel> GetIWorksKitById(long id);
        Task<ResponseModel> GetAllIWorksKit(bool? isActive = null);
        Task<List<IWorksKit>> GetIWorksKitPending();
        Task<ResponseModel> CreateIWorksKit(IWorksKit entity);
        Task<ResponseModel> CreateIWorksKits(List<IWorksKit> entities);
        Task<ResponseModel> UpdateIWorksKit(IWorksKit updateEntity);
        Task<ResponseModel> DeleteIWorksKit(long id);
        Task UpdateIWorksStatus(List<Kit> kits, string status);
    }
}
