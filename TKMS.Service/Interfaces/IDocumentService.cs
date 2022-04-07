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
    public interface IDocumentService
    {
        Task<ResponseModel> GetDocumentById(long id);
        Task<ResponseModel> GetAllDocument(bool? isActive = null);
        Task<ResponseModel> GetDocumentPaged(Pagination pagination);
        Task<ResponseModel> CreateDocument(Document entity);
        Task<ResponseModel> UpdateDocument(Document updateEntity);
        Task<ResponseModel> DeleteDocument(long id);
    }
}
