using Core.Repository.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Repository.Interfaces;
using TKMS.Service.Interfaces;

namespace TKMS.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportService(
            IReportRepository reportRepository
            )
        {
            _reportRepository = reportRepository;

        }

        public async Task<ResponseModel> GetIndentReport(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _reportRepository.GetIndentReport(pagination)
            };
        }

        public async Task<ResponseModel> GetAccountLevelDispatchReport(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _reportRepository.GetAccountLevelDispatchReport(pagination)
            };
        }

        public async Task<ResponseModel> GetAllocationReport(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _reportRepository.GetAllocationReport(pagination)
            };
        }

        public async Task<ResponseModel> GetAssignedReport(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _reportRepository.GetAssignedReport(pagination)
            };
        }

        public async Task<ResponseModel> GetDeliveryReport(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _reportRepository.GetDeliveryReport(pagination)
            };
        }

        public async Task<ResponseModel> GetDestructionReport(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _reportRepository.GetDestructionReport(pagination)
            };
        }

        public async Task<ResponseModel> GetDispatchReport(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _reportRepository.GetDispatchReport(pagination)
            };
        }
        public async Task<ResponseModel> GetReceivedAtROReport(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _reportRepository.GetReceivedAtROReport(pagination)
            };
        }

        public async Task<ResponseModel> GetReceivedAtBranchReport(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _reportRepository.GetReceivedAtBranchReport(pagination)
            };

        }

        public async Task<ResponseModel> GetReturnReport(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _reportRepository.GetReturnReport(pagination)
            };
        }

        public async Task<ResponseModel> GetScannedReport(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _reportRepository.GetScannedReport(pagination)
            };
        }

        public async Task<ResponseModel> GetTotalStockReport(Pagination pagination)
        {
            return new ResponseModel
            {
                Success = true,
                StatusCode = StatusCodes.Status200OK,
                Data = await _reportRepository.GetTotalStockReport(pagination)
            };

        }
    }
}
