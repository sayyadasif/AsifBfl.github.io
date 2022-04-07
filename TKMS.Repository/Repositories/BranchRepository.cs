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
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        public BranchRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetBranchPaged(Pagination pagination)
        {
            string branchName = IsPropertyExist(pagination.Filters, "branchName") ? pagination.Filters?.branchName : null;
            string branchCode = IsPropertyExist(pagination.Filters, "branchCode") ? pagination.Filters?.branchCode : null;
            long? branchTypeId = IsPropertyExist(pagination.Filters, "branchTypeId") ? pagination.Filters?.branchTypeId : null;
            long? regionId = IsPropertyExist(pagination.Filters, "regionId") ? pagination.Filters?.regionId : null;
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;

            IRepository<BranchModel> repositoryBranchModel = new Repository<BranchModel>(TkmsDbContext);
            var query = (from b in TkmsDbContext.Branches
                         where !b.IsDeleted &&
                         ((string.IsNullOrEmpty(branchName) || b.BranchName.Contains(branchName)) ||
                         (string.IsNullOrEmpty(branchCode) || b.BranchCode.Contains(branchCode))) &&
                         (!branchTypeId.HasValue || branchTypeId.Value == b.BranchTypeId) &&
                         (!regionId.HasValue || regionId.Value == b.RegionId) &&
                         (!isActive.HasValue || isActive.Value == b.IsActive)
                         join bt in TkmsDbContext.BranchTypes on b.BranchTypeId equals bt.BranchTypeId into bts
                         from bt in bts.DefaultIfEmpty()
                         join r in TkmsDbContext.Regions on b.RegionId equals r.RegionId into rs
                         from r in rs.DefaultIfEmpty()
                         join ib in TkmsDbContext.IblBranches on b.IblBranchId equals ib.IblBranchId into ibs
                         from ib in ibs.DefaultIfEmpty()
                         select new BranchModel
                         {
                             BranchId = b.BranchId,
                             BranchName = b.BranchName,
                             BranchCode = b.BranchCode,
                             IfscCode = b.IfscCode,
                             BranchType = bt == null ? "" : bt.BranchTypeName,
                             RegionName = r == null ? "" : r.RegionName,
                             IblBranchCode = ib == null ? "" : ib.IblBranchCode,
                             IblBranchName = ib == null ? "" : ib.IblBranchName,
                             IsActive = b.IsActive,
                         });

            return await repositoryBranchModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetDropdwon(long? regionId = null, long? branchId = null, long? id = null)
        {
            Repository<BranchDropdownModel> repositoryDropdownModel = new(TkmsDbContext);
            var query = (from b in TkmsDbContext.Branches
                         where !b.IsDeleted &&
                         (!regionId.HasValue || b.RegionId == regionId) &&
                         (!branchId.HasValue || b.BranchId == branchId) &&
                         (!id.HasValue || b.BranchId == id)
                         select new BranchDropdownModel
                         {
                             Id = b.BranchId,
                             Text = b.BranchName,
                             IsActive = b.IsActive,
                             BranchCode = b.BranchCode,
                             RegionId = b.RegionId
                         });

            return await repositoryDropdownModel.GetPagedReponseAsync(new Pagination { SortOrderColumn = "Text" }, null, query);
        }
    }
}
