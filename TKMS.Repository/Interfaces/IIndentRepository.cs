using Core.Repository;
using Core.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Models;

namespace TKMS.Repository.Interfaces
{
    public interface IIndentRepository : IRepository<Indent>
    {
        Task<PagedList> GetIndentPaged(Pagination pagination);
        Task<long> GetIndentCount(dynamic filters = null);
        Task<long> GetIndentKitTotal(dynamic filters = null);
        Task<IndentDetailModel> GetIndentDetailById(long id);
        Task<List<IndentDetailModel>> GetIndentDetailByIndentNos(List<string> indentNos);
    }
}
