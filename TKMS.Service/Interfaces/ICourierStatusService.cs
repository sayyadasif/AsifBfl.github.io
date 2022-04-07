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
    public interface ICourierStatusService
    {
        Task<ResponseModel> GetCourierStatusById(long id);
        Task<ResponseModel> GetAllCourierStatus(bool? isActive = null);
        Task<ResponseModel> GetCourierStatusPaged(Pagination pagination);
        Task<ResponseModel> CreateCourierStatus(CourierStatus entity);
        Task<ResponseModel> UpdateCourierStatus(CourierStatus updateEntity);
        Task<ResponseModel> DeleteCourierStatus(long id);
        Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null);
    }
}
