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
    public class SettingRepository : Repository<Setting>, ISettingRepository
    {
        public SettingRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetSettingPaged(Pagination pagination)
        {
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;

            IRepository<SettingModel> repositorySettingModel = new Repository<SettingModel>(TkmsDbContext);
            var query = (from us in TkmsDbContext.Settings
                         where us.IsDeleted == false &&
                         (!isActive.HasValue || isActive.Value == us.IsActive)
                         select new SettingModel()
                         {
                             IsActive = us.IsActive,
                         });

            return await repositorySettingModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<Setting> GetSettingsByKey(string key)
        {
            return await (from s in TkmsDbContext.Settings
                          where s.SettingKey == key
                          select s)
                         .FirstOrDefaultAsync();
        }
    }
}
