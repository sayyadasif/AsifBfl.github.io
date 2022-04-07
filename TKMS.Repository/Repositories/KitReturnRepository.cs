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
    public class KitReturnRepository : Repository<KitReturn>, IKitReturnRepository
    {
        public KitReturnRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetKitReturnPaged(Pagination pagination)
        {
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;

            IRepository<KitReturnModel> repositoryKitReturnModel = new Repository<KitReturnModel>(TkmsDbContext);
            var query = (from kr in TkmsDbContext.KitReturns
                         where !kr.IsDeleted
                         && (!isActive.HasValue || isActive.Value == kr.IsActive)
                         select new KitReturnModel
                         {
                         });

            return await repositoryKitReturnModel.GetPagedReponseAsync(pagination, null, query);
        }
    }
}
