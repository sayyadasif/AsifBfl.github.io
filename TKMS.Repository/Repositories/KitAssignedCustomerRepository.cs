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
    public class KitAssignedCustomerRepository : Repository<KitAssignedCustomer>, IKitAssignedCustomerRepository
    {
        public KitAssignedCustomerRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetKitAssignedCustomerPaged(Pagination pagination)
        {
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;

            IRepository<KitAssignedCustomerModel> repositoryKitAssignedCustomerModel = new Repository<KitAssignedCustomerModel>(TkmsDbContext);
            var query = (from kac in TkmsDbContext.KitAssignedCustomers
                         select new KitAssignedCustomerModel
                         {
                            
                         });

            return await repositoryKitAssignedCustomerModel.GetPagedReponseAsync(pagination, null, query);
        }
    }
}
