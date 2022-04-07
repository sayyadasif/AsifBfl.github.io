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
    public interface IKitStatusService
    {
        Task<ResponseModel> GetKitStatusById(long id);
        Task<ResponseModel> GetAllKitStatus(bool? isActive = null);
        Task<ResponseModel> GetKitStatusPaged(Pagination pagination);
        Task<ResponseModel> CreateKitStatus(KitStatus entity);
        Task<ResponseModel> UpdateKitStatus(KitStatus updateEntity);
        Task<ResponseModel> DeleteKitStatus(long id);
        Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null);
    }
}
