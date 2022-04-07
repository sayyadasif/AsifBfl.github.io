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
    public interface ISchemeCodeService
    {
        Task<ResponseModel> GetSchemeCodeById(long id);
        Task<ResponseModel> GetAllSchemeCode(bool? isActive = null);
        Task<ResponseModel> GetSchemeCodePaged(Pagination pagination);
        Task<ResponseModel> CreateSchemeCode(SchemeCode entity);
        Task<ResponseModel> UpdateSchemeCode(SchemeCode updateEntity);
        Task<ResponseModel> DeleteSchemeCode(long id);
        Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null);
    }
}
