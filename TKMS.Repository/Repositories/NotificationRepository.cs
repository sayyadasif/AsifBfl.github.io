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
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetNotificationPaged(Pagination pagination)
        {
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;

            IRepository<NotificationModel> repositoryNotificationModel = new Repository<NotificationModel>(TkmsDbContext);
            var query = (from n in TkmsDbContext.Notifications
                         where (!isActive.HasValue || isActive.Value == n.IsActive)
                         && (!bfilBranchId.HasValue || bfilBranchId.Value == n.BranchId)
                         select new NotificationModel
                         {
                             NotificationId = n.NotificationId,
                             Title = n.Title,
                             Description = n.Description,
                             DispatchId = n.DispatchId,
                             KitId = n.KitId,
                             RedirectUrl = n.RedirectUrl,
                             NotificationDate = n.CreatedDate,
                             VisibleToRoles = n.VisibleToRoles,
                             IsActive = n.IsActive,
                         });

            return await repositoryNotificationModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<List<DispatchNotificationModel>> DispatchConfirmations(int days, long bfilBranchId, int notificationTypeId)
        {
            var currentDate = CommonUtils.GetDefaultDateTime();

            var notifications = await GetExistNotifications(bfilBranchId, notificationTypeId);

            return await (from d in TkmsDbContext.Dispatches
                          join i in TkmsDbContext.Indents on d.IndentId equals i.IndentId
                          where d.BranchDispatchDate.HasValue && !d.BranchReceiveDate.HasValue
                          && !notifications.Contains(d.DispatchId)
#if DEBUG
#else
                          && i.BfilBranchId == bfilBranchId
#endif
                          && EF.Functions.DateDiffDay(d.BranchDispatchDate, currentDate) > days
                          select new DispatchNotificationModel
                          {
                              DispatchId = d.DispatchId,
                              DispatchDate = d.DispatchDate,
                              BranchDispatchDate = d.BranchDispatchDate,
                              BranchReceiveDate = d.BranchReceiveDate,
                              ReferenceNo = d.ReferenceNo,
                              IndentNo = i.IndentNo,
                              BranchId = i.BfilBranchId,
                          }).ToListAsync();
        }

        public async Task<List<DispatchNotificationModel>> ScanConfirmations(int days, long bfilBranchId, int notificationTypeId)
        {
            var currentDate = CommonUtils.GetDefaultDateTime();
            var dispatchedStatus = KitStatuses.Dispatched.GetHashCode();

            var notifications = await GetExistNotifications(bfilBranchId, notificationTypeId);

            return await (from d in TkmsDbContext.Dispatches
                          .Include(k => k.Kits)
                          join i in TkmsDbContext.Indents on d.IndentId equals i.IndentId
                          where d.BranchReceiveDate.HasValue
                          && !notifications.Contains(d.DispatchId)
#if DEBUG
#else
                          && i.BfilBranchId == bfilBranchId
#endif
                          && d.Kits.Count() > 0
                          && !d.Kits.Any(dk => dk.KitStatusId > dispatchedStatus)
                          && EF.Functions.DateDiffDay(d.BranchReceiveDate, currentDate) > days
                          select new DispatchNotificationModel
                          {
                              DispatchId = d.DispatchId,
                              DispatchDate = d.DispatchDate,
                              BranchDispatchDate = d.BranchDispatchDate,
                              BranchReceiveDate = d.BranchReceiveDate,
                              ReferenceNo = d.ReferenceNo,
                              IndentNo = i.IndentNo,
                              BranchId = i.BfilBranchId,
                          }).ToListAsync();
        }

        public async Task<List<DispatchNotificationModel>> KitReturnNotifications(int days, long bfilBranchId, int notificationTypeId)
        {
            var currentDate = CommonUtils.GetDefaultDateTime();
            var dispatchedStatus = KitStatuses.Allocated.GetHashCode();

            var notifications = await GetExistNotifications(bfilBranchId, notificationTypeId);

            return await (from k in TkmsDbContext.Kits
                          join i in TkmsDbContext.Indents on k.IndentId equals i.IndentId
                          where k.AllocatedDate.HasValue
                          && !notifications.Contains(k.KitId)
#if DEBUG
#else
                          && i.BfilBranchId == bfilBranchId
#endif
                          && EF.Functions.DateDiffDay(k.AllocatedDate, currentDate) > days
                          select new DispatchNotificationModel
                          {
                              DispatchId = k.DispatchId,
                              KitId = k.KitId,
                              AllocatedDate = k.AllocatedDate,
                              AccountNo = k.AccountNo,
                              IndentNo = i.IndentNo,
                              BranchId = i.BfilBranchId,
                          }).ToListAsync();
        }

        private async Task<List<long>> GetExistNotifications(long bfilBranchId, int notificationTypeId)
        {
            return await (from n in TkmsDbContext.Notifications
                          where n.IsActive
                          && n.DispatchId.HasValue
                          && n.NotificationTypeId == notificationTypeId
#if DEBUG
#else
                          && n.BranchId == bfilBranchId
#endif
                          select n.DispatchId.Value).ToListAsync();
        }
    }
}
