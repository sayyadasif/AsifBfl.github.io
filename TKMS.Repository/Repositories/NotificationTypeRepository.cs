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
    public class NotificationTypeRepository : Repository<NotificationType>, INotificationTypeRepository
    {
        public NotificationTypeRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetNotificationTypePaged(Pagination pagination)
        {
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;

            IRepository<NotificationTypeModel> repositoryNotificationTypeModel = new Repository<NotificationTypeModel>(TkmsDbContext);
            var query = (from nt in TkmsDbContext.NotificationTypes
                         where !nt.IsDeleted
                         && (!isActive.HasValue || isActive.Value == nt.IsActive)
                         select new NotificationTypeModel
                         {
                         });

            return await repositoryNotificationTypeModel.GetPagedReponseAsync(pagination, null, query);
        }
    }
}
