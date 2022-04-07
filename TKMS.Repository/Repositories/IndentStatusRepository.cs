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
    public class IndentStatusRepository : Repository<IndentStatus>, IIndentStatusRepository
    {
        public IndentStatusRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetIndentStatusPaged(Pagination pagination)
        {
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;

            IRepository<IndentStatusModel> repositoryIndentStatusModel = new Repository<IndentStatusModel>(TkmsDbContext);
            var query = (from us in TkmsDbContext.IndentStatuses
                         where us.IsDeleted == false &&
                         (!isActive.HasValue || isActive.Value == us.IsActive)
                         select new IndentStatusModel()
                         {
                             IsActive = us.IsActive,
                         });

            return await repositoryIndentStatusModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetDropdwon(long? id = null, bool? isActive = null)
        {
            Repository<DropdownModel> repositoryDropdownModel = new(TkmsDbContext);
            var query = (from r in TkmsDbContext.IndentStatuses
                         where !r.IsDeleted &&
                         (!id.HasValue || r.IndentStatusId == id) &&
                         (!isActive.HasValue || r.IsActive == isActive)
                         select new DropdownModel()
                         {
                             Id = r.IndentStatusId,
                             Text = r.StatusName,
                             IsActive = r.IsActive
                         });

            return await repositoryDropdownModel.GetPagedReponseAsync(new Pagination { SortOrderColumn = "Text" }, null, query);
        }
    }


}
