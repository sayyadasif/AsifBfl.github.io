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
    public interface IDispatchService
    {
        Task<ResponseModel> GetDispatchById(long id);
        Task<ResponseModel> GetAllDispatch(bool? isActive = null);
        Task<ResponseModel> GetDispatchPaged(Pagination pagination);
        Task<ResponseModel> CreateDispatch(Dispatch entity);
        Task<ResponseModel> CreateDispatches(List<Dispatch> dispatches);
        Task<ResponseModel> UpdateDispatch(Dispatch updateEntity);
        Task<ResponseModel> UpdateDispatchStatus(Dispatch updateEntity);
        Task<ResponseModel> DeleteDispatch(long id);
        Task<bool> IsAllDispatchReceived(long indentId);
    }
}
