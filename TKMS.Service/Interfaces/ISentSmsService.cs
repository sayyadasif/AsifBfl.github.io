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
    public interface ISentSmsService
    {
        Task<ResponseModel> GetSentSmsById(long id);
        Task<ResponseModel> GetAllSentSms(bool? isActive = null);
        Task<ResponseModel> GetSentSmsPaged(Pagination pagination);
        Task<ResponseModel> CreateSentSms(SentSms entity);
        Task<ResponseModel> CreateSentSmses(List<SentSms> entities);
        Task<ResponseModel> UpdateSentSms(SentSms updateEntity);
        Task<ResponseModel> DeleteSentSms(long id);
    }
}
