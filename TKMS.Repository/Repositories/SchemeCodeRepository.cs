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
    public class SchemeCodeRepository : Repository<SchemeCode>, ISchemeCodeRepository
    {
        public SchemeCodeRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetSchemeCodePaged(Pagination pagination)
        {
            string schemeCodeName = IsPropertyExist(pagination.Filters, "schemeCodeName") ? pagination.Filters?.schemeCodeName : null;

            IRepository<SchemeCodeModel> repositorySchemeCodeModel = new Repository<SchemeCodeModel>(TkmsDbContext);
            var query = (from sc in TkmsDbContext.SchemeCodes
                         where !sc.IsDeleted
                         && (string.IsNullOrEmpty(schemeCodeName) || sc.SchemeCodeName.Contains(schemeCodeName))
                         select new SchemeCodeModel
                         {
                             SchemeCodeId = sc.SchemeCodeId,
                             SchemeCodeName = sc.SchemeCodeName,
                             SchemeC5Codes = (from c5 in TkmsDbContext.C5Codes
                                        join ct in TkmsDbContext.CardTypes on c5.CardTypeId equals ct.CardTypeId into cts
                                        from ct in cts.DefaultIfEmpty()
                                        join scc in TkmsDbContext.SchemeC5Codes on c5.C5CodeId equals scc.C5CodeId
                                        where scc.SchemeCodeId == sc.SchemeCodeId
                                        select $"{c5.C5CodeName} - {(ct != null ? ct.CardTypeName : "")}").AsEnumerable(),
                         });

            return await repositorySchemeCodeModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetDropdwon(long? id = null, bool? isActive = null)
        {
            Repository<DropdownModel> repositoryDropdownModel = new(TkmsDbContext);
            var query = (from r in TkmsDbContext.SchemeCodes
                         where !r.IsDeleted &&
                         (!id.HasValue || r.SchemeCodeId == id) &&
                         (!isActive.HasValue || r.IsActive == isActive)
                         select new DropdownModel()
                         {
                             Id = r.SchemeCodeId,
                             Text = r.SchemeCodeName,
                             IsActive = r.IsActive
                         });

            return await repositoryDropdownModel.GetPagedReponseAsync(new Pagination { SortOrderColumn = "Text" }, null, query);
        }
    }
}
