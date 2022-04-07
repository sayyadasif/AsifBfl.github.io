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
using TKMS.Abstraction.Enums;
using TKMS.Abstraction.Models;
using TKMS.Repository.Contexts;
using TKMS.Repository.Interfaces;

namespace TKMS.Repository.Repositories
{
    public class KitAuditRepository : Repository<KitAudit>, IKitAuditRepository
    {
        public KitAuditRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetKitAuditPaged(Pagination pagination)
        {
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;

            IRepository<KitAuditModel> repositoryKitAuditModel = new Repository<KitAuditModel>(TkmsDbContext);
            var query = (from us in TkmsDbContext.KitAudits
                         where us.IsDeleted == false &&
                         (!isActive.HasValue || isActive.Value == us.IsActive)
                         select new KitAuditModel()
                         {
                             IsActive = us.IsActive,
                         });

            return await repositoryKitAuditModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetDropdwon(long? id = null, bool? isActive = null)
        {
            Repository<DropdownModel> repositoryDropdownModel = new(TkmsDbContext);
            var query = (from r in TkmsDbContext.KitAudits
                         where !r.IsDeleted &&
                         (!id.HasValue || r.KitAuditId == id) &&
                         (!isActive.HasValue || r.IsActive == isActive)
                         select new DropdownModel()
                         {
                             Id = r.KitAuditId,
                             IsActive = r.IsActive
                         });

            return await repositoryDropdownModel.GetPagedReponseAsync(new Pagination { SortOrderColumn = "Text" }, null, query);
        }

        public async Task<KitAudit> GetKitLastDestruction(long kitId)
        {
            var skipKitStatusId = KitStatuses.Damaged.GetHashCode();

            return await (from ka in TkmsDbContext.KitAudits
                          where ka.KitId == kitId
                          && ka.KitStatusId != skipKitStatusId
                          select ka)
                         .OrderByDescending(k => k.KitAuditId)
                         .FirstOrDefaultAsync();
        }
    }
}
