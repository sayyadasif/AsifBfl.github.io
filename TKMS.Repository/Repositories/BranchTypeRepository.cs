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
    public class BranchTypeRepository : Repository<BranchType>, IBranchTypeRepository
    {
        public BranchTypeRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetBranchTypePaged(Pagination pagination)
        {
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;

            IRepository<BranchTypeModel> repositoryBranchTypeModel = new Repository<BranchTypeModel>(TkmsDbContext);
            var query = (from us in TkmsDbContext.BranchTypes
                         where us.IsDeleted == false &&
                         (!isActive.HasValue || isActive.Value == us.IsActive)
                         select new BranchTypeModel()
                         {
                             IsActive = us.IsActive,
                         });

            return await repositoryBranchTypeModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetDropdwon(bool? isAllowIndent = null, long? id = null, bool? isActive = null)
        {
            Repository<DropdownModel> repositoryDropdownModel = new(TkmsDbContext);
            var query = (from r in TkmsDbContext.BranchTypes
                         where !r.IsDeleted &&
                         (!id.HasValue || r.BranchTypeId == id) &&
                         (!isAllowIndent.HasValue || r.IsAllowIndent == isAllowIndent) &&
                         (!isActive.HasValue || r.IsActive == isActive)
                         select new DropdownModel()
                         {
                             Id = r.BranchTypeId,
                             Text = r.BranchTypeName,
                             IsActive = r.IsActive
                         });

            return await repositoryDropdownModel.GetPagedReponseAsync(new Pagination { SortOrderColumn = "Text" }, null, query);
        }
    }
}
