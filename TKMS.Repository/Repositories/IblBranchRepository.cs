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
    public class IblBranchRepository : Repository<IblBranch>, IIblBranchRepository
    {
        public IblBranchRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetIblBranchPaged(Pagination pagination)
        {
            string iblBranchName = IsPropertyExist(pagination.Filters, "iblBranchName") ? pagination.Filters?.iblBranchName : null;
            string iblBranchCode = IsPropertyExist(pagination.Filters, "iblBranchCode") ? pagination.Filters?.iblBranchCode : null;

            IRepository<IblBranchModel> repositoryIblBranchModel = new Repository<IblBranchModel>(TkmsDbContext);
            var query = (from ib in TkmsDbContext.IblBranches
                         where !ib.IsDeleted
                         && ((string.IsNullOrEmpty(iblBranchName) || ib.IblBranchName.Contains(iblBranchName))
                         || (string.IsNullOrEmpty(iblBranchCode) || ib.IblBranchCode.Contains(iblBranchCode)))
                         select new IblBranchModel
                         {
                             IblBranchId = ib.IblBranchId,
                             IblBranchCode = ib.IblBranchCode,
                             IblBranchName = ib.IblBranchName,
                             Branches = (from b in TkmsDbContext.Branches
                                         where b.IblBranchId == ib.IblBranchId
                                         select $"{b.BranchCode} - {b.BranchName}").AsEnumerable(),
                         });

            return await repositoryIblBranchModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetDropdwon(long? id = null, bool? isActive = null)
        {
            Repository<IblBranchDropdownModel> repositoryDropdownModel = new(TkmsDbContext);
            var query = (from ib in TkmsDbContext.IblBranches
                         where !ib.IsDeleted &&
                         (!id.HasValue || ib.IblBranchId == id) &&
                         (!isActive.HasValue || ib.IsActive == isActive)
                         select new IblBranchDropdownModel
                         {
                             Id = ib.IblBranchId,
                             Text = ib.IblBranchName,
                             IblBranchCode = ib.IblBranchCode,
                             IsActive = ib.IsActive
                         });

            return await repositoryDropdownModel.GetPagedReponseAsync(new Pagination { SortOrderColumn = "Text" }, null, query);
        }
    }
}
