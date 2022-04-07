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
    public class DispatchAuditRepository : Repository<DispatchAudit>, IDispatchAuditRepository
    {
        public DispatchAuditRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetDispatchAuditPaged(Pagination pagination)
        {
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;

            IRepository<DispatchAuditModel> repositoryDispatchAuditModel = new Repository<DispatchAuditModel>(TkmsDbContext);
            var query = (from us in TkmsDbContext.DispatchAudits
                         where us.IsDeleted == false &&
                         (!isActive.HasValue || isActive.Value == us.IsActive)
                         select new DispatchAuditModel()
                         {
                         });

            return await repositoryDispatchAuditModel.GetPagedReponseAsync(pagination, null, query);
        }
    }
}
