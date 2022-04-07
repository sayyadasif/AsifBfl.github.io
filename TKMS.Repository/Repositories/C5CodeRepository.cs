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
    public class C5CodeRepository : Repository<C5Code>, IC5CodeRepository
    {
        public C5CodeRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetC5CodePaged(Pagination pagination)
        {
            string c5CodeName = IsPropertyExist(pagination.Filters, "c5CodeName") ? pagination.Filters?.c5CodeName : null;

            IRepository<C5CodeModel> repositoryC5CodeModel = new Repository<C5CodeModel>(TkmsDbContext);
            var query = (from c5 in TkmsDbContext.C5Codes
                         join ct in TkmsDbContext.CardTypes on c5.CardTypeId equals ct.CardTypeId into cts
                         from ct in cts.DefaultIfEmpty()

                         where !c5.IsDeleted
                         && (string.IsNullOrEmpty(c5CodeName) || c5.C5CodeName.Contains(c5CodeName))

                         select new C5CodeModel
                         {
                             C5CodeId = c5.C5CodeId,
                             C5CodeName = c5.C5CodeName,
                             CardTypeName = ct != null ? ct.CardTypeName : "",
                         });

            return await repositoryC5CodeModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetDropdwon(long? id = null, long? schemeCodeId = null)
        {
            var c5Codes = new List<long>();
            if (schemeCodeId.HasValue && schemeCodeId > 0)
            {
                c5Codes = TkmsDbContext.SchemeC5Codes.Where(sc => sc.SchemeCodeId == schemeCodeId).Select(c => c.C5CodeId).ToList();
            }

            Repository<C5CodeDropdownModel> repositoryDropdownModel = new(TkmsDbContext);
            var query = (from c5 in TkmsDbContext.C5Codes
                         join ct in TkmsDbContext.CardTypes on c5.CardTypeId equals ct.CardTypeId into cts
                         from ct in cts.DefaultIfEmpty()

                         where !c5.IsDeleted &&
                         (!id.HasValue || c5.C5CodeId == id) &&
                         (!schemeCodeId.HasValue || c5Codes.Contains(c5.C5CodeId))
                         select new C5CodeDropdownModel
                         {
                             Id = c5.C5CodeId,
                             Text = c5.C5CodeName,
                             CardTypeId = c5.CardTypeId,
                             CardTypeName = ct != null ? ct.CardTypeName : "",
                             IsActive = c5.IsActive
                         });

            return await repositoryDropdownModel.GetPagedReponseAsync(new Pagination { SortOrderColumn = "Text" }, null, query);
        }
    }
}
