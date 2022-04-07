using Core.Repository;
using Core.Repository.Models;
using Core.Utility.Utils;
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
    public class SentSmsRepository : Repository<SentSms>, ISentSmsRepository
    {
        public SentSmsRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetSentSmsPaged(Pagination pagination)
        {
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;

            IRepository<SentSmsModel> repositorySentSmsModel = new Repository<SentSmsModel>(TkmsDbContext);
            var query = (from us in TkmsDbContext.SentSmses
                         where us.IsDeleted == false &&
                         (!isActive.HasValue || isActive.Value == us.IsActive)
                         select new SentSmsModel
                         {
                         });

            return await repositorySentSmsModel.GetPagedReponseAsync(pagination, null, query);
        }
    }
}
