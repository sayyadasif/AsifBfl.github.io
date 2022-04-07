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
    public interface INotificationTypeService
    {
        Task<ResponseModel> GetNotificationTypeById(long id);
        Task<ResponseModel> GetAllNotificationType(bool? isActive = null);
        Task<ResponseModel> GetNotificationTypePaged(Pagination pagination);
        Task<ResponseModel> CreateNotificationType(NotificationType entity);
        Task<ResponseModel> UpdateNotificationType(NotificationType updateEntity);
        Task<ResponseModel> DeleteNotificationType(long id);
    }
}
