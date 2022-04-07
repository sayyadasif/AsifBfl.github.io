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
    public interface IIndentService
    {
        Task<ResponseModel> GetIndentById(long id);
        Task<ResponseModel> GetAllIndent(bool? isActive = null);
        Task<ResponseModel> GetIndentPaged(Pagination pagination);
        Task<ResponseModel> CreateIndent(Indent entity);
        Task<ResponseModel> CreateIndents(List<Indent> entities, bool validateIndent = true);
        Task<ResponseModel> UpdateIndent(Indent updateEntity);
        Task<ResponseModel> UpdateIndentStatus(Indent updateEntity);
        Task<ResponseModel> DeleteIndent(long id);
        Task<ResponseModel> DeleteIndents(List<Indent> entities);
        Task<string> GetNewIndentNo();
        Task<long> GetIndentCount(dynamic filters);
        Task<long> GetIndentKitTotal(dynamic filters);
        Task<IndentDetailModel> GetIndentDetailById(long id);
        Task<List<IndentDetailModel>> GetIndentDetailByIndentNos(List<string> indentNos);
    }
}
