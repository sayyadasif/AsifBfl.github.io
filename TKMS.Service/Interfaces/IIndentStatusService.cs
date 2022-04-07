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
    public interface IIndentStatusService
    {
        Task<ResponseModel> GetIndentStatusById(long id);
        Task<ResponseModel> GetAllIndentStatus(bool? isActive = null);
        Task<ResponseModel> GetIndentStatusPaged(Pagination pagination);
        Task<ResponseModel> CreateIndentStatus(IndentStatus entity);
        Task<ResponseModel> UpdateIndentStatus(IndentStatus updateEntity);
        Task<ResponseModel> DeleteIndentStatus(long id);
        Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null);
    }
}
