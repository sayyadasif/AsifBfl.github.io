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
    public interface IDispatchStatusService
    {
        Task<ResponseModel> GetDispatchStatusById(long id);
        Task<ResponseModel> GetAllDispatchStatus(bool? isActive = null);
        Task<ResponseModel> GetDispatchStatusPaged(Pagination pagination);
        Task<ResponseModel> CreateDispatchStatus(DispatchStatus entity);
        Task<ResponseModel> UpdateDispatchStatus(DispatchStatus updateEntity);
        Task<ResponseModel> DeleteDispatchStatus(long id);
        Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null);
    }
}
