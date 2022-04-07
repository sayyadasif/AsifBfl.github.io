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
    public class IndentAuditRepository : Repository<IndentAudit>, IIndentAuditRepository
    {
        public IndentAuditRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetIndentAuditPaged(Pagination pagination)
        {
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;

            IRepository<IndentAuditModel> repositoryIndentAuditModel = new Repository<IndentAuditModel>(TkmsDbContext);
            var query = (from us in TkmsDbContext.IndentAudits
                         where us.IsDeleted == false &&
                         (!isActive.HasValue || isActive.Value == us.IsActive)
                         select new IndentAuditModel()
                         {
                             IsActive = us.IsActive,
                         });

            return await repositoryIndentAuditModel.GetPagedReponseAsync(pagination, null, query);
        }
    }
}
