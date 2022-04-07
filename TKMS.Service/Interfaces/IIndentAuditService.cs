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
    public interface IIndentAuditService
    {
        Task<ResponseModel> GetIndentAuditById(long id);
        Task<ResponseModel> GetAllIndentAudit(bool? isActive = null);
        Task<ResponseModel> GetIndentAuditPaged(Pagination pagination);
        Task<ResponseModel> CreateIndentAudit(IndentAudit entity);
        Task<ResponseModel> UpdateIndentAudit(IndentAudit updateEntity);
        Task<ResponseModel> DeleteIndentAudit(long id);
    }
}
