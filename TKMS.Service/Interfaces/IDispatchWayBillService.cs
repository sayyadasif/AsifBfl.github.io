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
    public interface IDispatchWayBillService
    {
        Task<ResponseModel> GetDispatchWayBillById(long id);
        Task<ResponseModel> GetAllDispatchWayBill(bool? isActive = null);
        Task<ResponseModel> GetDispatchWayBillPaged(Pagination pagination);
        Task<ResponseModel> CreateDispatchWayBill(DispatchWayBill entity);
        Task<ResponseModel> UpdateDispatchWayBill(DispatchWayBill updateEntity);
        Task<ResponseModel> DeleteDispatchWayBill(long id);
    }
}
