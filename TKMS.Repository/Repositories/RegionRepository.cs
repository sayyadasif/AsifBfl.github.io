using Core.Repository;
using Core.Repository.Models;
using Core.Utility.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Models;
using TKMS.Repository.Contexts;
using TKMS.Repository.Interfaces;

namespace TKMS.Repository.Repositories
{
    public class RegionRepository : Repository<Region>, IRegionRepository
    {
        public RegionRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetRegionPaged(Pagination pagination)
        {
            string regionName = IsPropertyExist(pagination.Filters, "regionName") ? pagination.Filters?.regionName : null;
            string systemRoId = IsPropertyExist(pagination.Filters, "systemRoId") ? pagination.Filters?.systemRoId : null;

            IRepository<RegionModel> repositoryRegionModel = new Repository<RegionModel>(TkmsDbContext);
            var query = (from r in TkmsDbContext.Regions
                         .Include(a => a.Address)
                         where !r.IsDeleted
                         && ((string.IsNullOrEmpty(regionName) || r.RegionName.Contains(regionName))
                         || (string.IsNullOrEmpty(systemRoId) || r.SystemRoId.Contains(systemRoId)))
                         select new RegionModel
                         {
                             RegionId = r.RegionId,
                             SystemRoId = r.SystemRoId,
                             RegionName = r.RegionName,
                             IsActive = r.IsActive,
                             Address = r.Address
                         });

            return await repositoryRegionModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetDropdwon(long? id = null, bool? isActive = null)
        {
            Repository<DropdownModel> repositoryDropdownModel = new(TkmsDbContext);
            var query = (from r in TkmsDbContext.Regions
                         where !r.IsDeleted &&
                         (!id.HasValue || r.RegionId == id) &&
                         (!isActive.HasValue || r.IsActive == isActive)
                         select new DropdownModel()
                         {
                             Id = r.RegionId,
                             Text = r.RegionName,
                             IsActive = r.IsActive
                         });

            return await repositoryDropdownModel.GetPagedReponseAsync(new Pagination { SortOrderColumn = "Text" }, null, query);
        }
    }
}
