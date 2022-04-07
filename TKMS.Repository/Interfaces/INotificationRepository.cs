using Core.Repository;
using Core.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Models;

namespace TKMS.Repository.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<PagedList> GetNotificationPaged(Pagination pagination);
        Task<List<DispatchNotificationModel>> DispatchConfirmations(int days, long bfilBranchId, int notificationTypeId);
        Task<List<DispatchNotificationModel>> ScanConfirmations(int days, long bfilBranchId, int notificationTypeId);
        Task<List<DispatchNotificationModel>> KitReturnNotifications(int days, long bfilBranchId, int notificationTypeId);

    }
}
