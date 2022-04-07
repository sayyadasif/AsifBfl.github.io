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
    public interface IReportService
    {
        Task<ResponseModel> GetIndentReport(Pagination pagination);
        Task<ResponseModel> GetDispatchReport(Pagination pagination);
        Task<ResponseModel> GetDeliveryReport(Pagination pagination);
        Task<ResponseModel> GetAccountLevelDispatchReport(Pagination pagination);
        Task<ResponseModel> GetReceivedAtROReport(Pagination pagination);
        Task<ResponseModel> GetReceivedAtBranchReport(Pagination pagination);
        Task<ResponseModel> GetScannedReport(Pagination pagination);
        Task<ResponseModel> GetAllocationReport(Pagination pagination);
        Task<ResponseModel> GetAssignedReport(Pagination pagination);
        Task<ResponseModel> GetDestructionReport(Pagination pagination);
        Task<ResponseModel> GetTotalStockReport(Pagination pagination);
        Task<ResponseModel> GetReturnReport(Pagination pagination);
    }
}
