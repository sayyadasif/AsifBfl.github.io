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
    public interface IRejectionReasonService
    {
        Task<ResponseModel> GetRejectionReasonById(long id);
        Task<ResponseModel> GetAllRejectionReason(bool? isActive = null);
        Task<ResponseModel> GetRejectionReasonPaged(Pagination pagination);
        Task<ResponseModel> CreateRejectionReason(RejectionReason entity);
        Task<ResponseModel> UpdateRejectionReason(RejectionReason updateEntity);
        Task<ResponseModel> DeleteRejectionReason(long id);
        Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null);
    }
}
