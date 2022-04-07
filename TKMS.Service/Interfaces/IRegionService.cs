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
    public interface IRegionService
    {
        Task<ResponseModel> GetRegionById(long id);
        Task<ResponseModel> GetAllRegion(bool? isActive = null);
        Task<ResponseModel> GetRegionPaged(Pagination pagination);
        Task<ResponseModel> CreateRegion(Region entity);
        Task<ResponseModel> CreateRegions(List<Region> entities);
        Task<ResponseModel> UpdateRegion(Region updateEntity);
        Task<ResponseModel> DeleteRegion(long id);
        Task<List<DropdownModel>> GetDropdwon(long? id = null, bool? isActive = null);
    }
}
