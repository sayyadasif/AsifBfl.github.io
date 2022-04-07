using Core.Repository;
using Core.Repository.Models;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;

namespace TKMS.Repository.Interfaces
{
    public interface IReportRepository : IRepository<ReportModel>
    {
        Task<PagedList> GetIndentReport(Pagination pagination);
        Task<PagedList> GetDispatchReport(Pagination pagination);
        Task<PagedList> GetDeliveryReport(Pagination pagination);
        Task<PagedList> GetAccountLevelDispatchReport(Pagination pagination);
        Task<PagedList> GetReceivedAtROReport(Pagination pagination);
        Task<PagedList> GetReceivedAtBranchReport(Pagination pagination);
        Task<PagedList> GetScannedReport(Pagination pagination);
        Task<PagedList> GetAllocationReport(Pagination pagination);
        Task<PagedList> GetAssignedReport(Pagination pagination);
        Task<PagedList> GetDestructionReport(Pagination pagination);
        Task<PagedList> GetTotalStockReport(Pagination pagination);
        Task<PagedList> GetReturnReport(Pagination pagination);
    }
}
