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
    public interface IDispatchAuditService
    {
        Task<ResponseModel> GetDispatchAuditById(long id);
        Task<ResponseModel> GetAllDispatchAudit(bool? isActive = null);
        Task<ResponseModel> GetDispatchAuditPaged(Pagination pagination);
        Task<ResponseModel> CreateDispatchAudit(DispatchAudit entity);
        Task<ResponseModel> UpdateDispatchAudit(DispatchAudit updateEntity);
        Task<ResponseModel> DeleteDispatchAudit(long id);
    }
}
