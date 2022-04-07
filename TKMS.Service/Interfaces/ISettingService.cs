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
    public interface ISettingService
    {
        Task<ResponseModel> GetSettingById(long id);
        Task<ResponseModel> GetAllSetting(bool? isActive = null);
        Task<ResponseModel> GetSettingPaged(Pagination pagination);
        Task<ResponseModel> CreateSetting(Setting entity);
        Task<ResponseModel> UpdateSetting(Setting updateEntity);
        Task<ResponseModel> UpdateSettings(List<Setting> updateEntities);
        Task<ResponseModel> DeleteSetting(long id);
        Task<Setting> GetSettingsByKey(string key);
    }
}
