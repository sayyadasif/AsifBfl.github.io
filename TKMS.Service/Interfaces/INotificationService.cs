using Core.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Models;

namespace TKMS.Service.Interfaces
{
    public interface INotificationService
    {
        Task<ResponseModel> GetNotificationById(long id);
        Task<ResponseModel> GetAllNotification(bool? isActive = null);
        Task<List<NotificationModel>> GetUserNotifications(bool? isActive = true);
        Task<ResponseModel> GetNotificationPaged(Pagination pagination);
        Task<ResponseModel> CreateNotification(Notification entity);
        Task<ResponseModel> CreateNotifications(List<Notification> entities);
        Task<ResponseModel> UpdateNotification(Notification updateEntity);
        Task<ResponseModel> UpdateNotifications(List<Notification> updateEntities, long notificationTypeId);
        Task<ResponseModel> DeleteNotification(long id);
        Task GenerateNotifactions();
    }
}
