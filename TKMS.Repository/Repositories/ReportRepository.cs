using Core.Repository;
using Core.Repository.Models;
using Core.Utility.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Enums;
using TKMS.Abstraction.Models;
using TKMS.Repository.Contexts;
using TKMS.Repository.Interfaces;

namespace TKMS.Repository.Repositories
{
    public class ReportRepository : Repository<ReportModel>, IReportRepository
    {
        public ReportRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetIndentReport(Pagination pagination)
        {
            string indentNo = IsPropertyExist(pagination.Filters, "indentNo") ? pagination.Filters?.indentNo : null;
            string indentStartDate = IsPropertyExist(pagination.Filters, "indentStartDate") ? pagination.Filters?.indentStartDate : null;
            string indentEndDate = IsPropertyExist(pagination.Filters, "indentEndDate") ? pagination.Filters?.indentEndDate : null;
            long? branchId = IsPropertyExist(pagination.Filters, "branchId") ? pagination.Filters?.branchId : null;
            long? schemeCodeId = IsPropertyExist(pagination.Filters, "schemeCodeId") ? pagination.Filters?.schemeCodeId : null;
            long? c5CodeId = IsPropertyExist(pagination.Filters, "c5CodeId") ? pagination.Filters?.c5CodeId : null;
            long? cardTypeId = IsPropertyExist(pagination.Filters, "cardTypeId") ? pagination.Filters?.cardTypeId : null;
            long? bfilBranchTypeId = IsPropertyExist(pagination.Filters, "bfilBranchTypeId") ? pagination.Filters?.bfilBranchTypeId : null;
            long? regionId = IsPropertyExist(pagination.Filters, "regionId") ? pagination.Filters?.regionId : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;

            DateTime? _indentStartDate = null;
            if (!string.IsNullOrEmpty(indentStartDate))
            {
                _indentStartDate = CommonUtils.GetParseDate(indentStartDate);
            }

            DateTime? _indentEndDate = null;
            if (!string.IsNullOrEmpty(indentEndDate))
            {
                _indentEndDate = CommonUtils.GetParseDate(indentEndDate);
            }

            IRepository<ReportModel> repositoryReportModel = new Repository<ReportModel>(TkmsDbContext);

            var query = (from i in TkmsDbContext.Indents
                         join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                         from ib in ibs.DefaultIfEmpty()
                         join bf in TkmsDbContext.Branches on i.BfilBranchId equals bf.BranchId
                         join bt in TkmsDbContext.BranchTypes on i.BfilBranchTypeId equals bt.BranchTypeId
                         join sc in TkmsDbContext.SchemeCodes on i.SchemeCodeId equals sc.SchemeCodeId
                         join c5 in TkmsDbContext.C5Codes on i.C5CodeId equals c5.C5CodeId
                         join ct in TkmsDbContext.CardTypes on i.CardTypeId equals ct.CardTypeId
                         join a in TkmsDbContext.Addresses on i.DispatchAddressId equals a.AddressId
                         join us in TkmsDbContext.Users on i.CreatedBy equals us.UserId
                         join ust in TkmsDbContext.RoleTypes on us.RoleTypeId equals ust.RoleTypeId

                         join hoins in TkmsDbContext.IndentStatuses on i.HoApproveStatusId equals hoins.IndentStatusId into hostatuses
                         from hoins in hostatuses.DefaultIfEmpty()
                         join houser in TkmsDbContext.Users on i.HoApproveBy equals houser.UserId into hoapproves
                         from houser in hoapproves.DefaultIfEmpty()

                         join cpuins in TkmsDbContext.IndentStatuses on i.CpuApproveStatusId equals cpuins.IndentStatusId into cpustatuses
                         from cpuins in cpustatuses.DefaultIfEmpty()
                         join cpuser in TkmsDbContext.Users on i.CpuApproveBy equals cpuser.UserId into cpuapproves
                         from cpuser in cpuapproves.DefaultIfEmpty()


                         where !i.IsDeleted
                         && (string.IsNullOrEmpty(indentNo) || i.IndentNo.Contains(indentNo))
                         && (!_indentStartDate.HasValue || _indentStartDate.Value.Date <= i.IndentDate.Date)
                         && (!_indentEndDate.HasValue || _indentEndDate.Value.Date >= i.IndentDate.Date)
                         && (!branchId.HasValue || branchId.Value == i.IblBranchId)
                         && (!schemeCodeId.HasValue || schemeCodeId.Value == i.SchemeCodeId)
                         && (!c5CodeId.HasValue || c5CodeId.Value == i.C5CodeId)
                         && (!cardTypeId.HasValue || cardTypeId.Value == i.CardTypeId)
                         && (!bfilBranchTypeId.HasValue || bfilBranchTypeId.Value == i.BfilBranchTypeId)
                         && (!regionId.HasValue || regionId.Value == i.BfilRegionId)
                         && (!bfilBranchId.HasValue || bfilBranchId.Value == i.BfilBranchId)

                         select new ReportModel
                         {
                             IndentNumber = i.IndentNo,
                             IndentDate = i.IndentDate,
                             IblBranchCode = ib != null ? ib.IblBranchCode : "",
                             IblBranchName = ib != null ? ib.IblBranchName : "",
                             SchemeCode = sc.SchemeCodeName,
                             C5Code = c5.C5CodeName,
                             CardType = ct.CardTypeName,
                             NoOfKit = i.NoOfKit,
                             BranchType = bt.BranchTypeName,
                             DispatchAddress = a.AddressDetail,
                             BfilBranchCode = bf.BranchCode,
                             BfilBranchName = bf.BranchName,
                             PinCode = a.PinCode,
                             ContactPerson = i.ContactName,
                             ContactNumber = i.ContactNo,
                             IndentedUserType = ust.RoleTypeKey,
                             IndentedBy = us.FullName,
                             BfilIndentHOStaus = hoins != null ? hoins.StatusName : "",
                             BfilIndentHoApproveBy = hoins != null ? houser.FullName : "",
                             HoApproveDate = i.HoApproveDate,
                             CpuIndentStaus = cpuins != null ? cpuins.StatusName : "",
                             CpuIndentHoApproveBy = cpuser != null ? cpuser.FullName : "",
                             CpuApproveDate = i.CpuApproveDate
                         });

            return await repositoryReportModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetDispatchReport(Pagination pagination)
        {
            string indentNo = IsPropertyExist(pagination.Filters, "indentNo") ? pagination.Filters?.indentNo : null;
            string indentStartDate = IsPropertyExist(pagination.Filters, "indentStartDate") ? pagination.Filters?.indentStartDate : null;
            string indentEndDate = IsPropertyExist(pagination.Filters, "indentEndDate") ? pagination.Filters?.indentEndDate : null;
            string dispatchStartDate = IsPropertyExist(pagination.Filters, "dispatchStartDate") ? pagination.Filters?.dispatchStartDate : null;
            string dispatchEndDate = IsPropertyExist(pagination.Filters, "dispatchEndDate") ? pagination.Filters?.dispatchEndDate : null;
            long? branchId = IsPropertyExist(pagination.Filters, "branchId") ? pagination.Filters?.branchId : null;
            long? schemeCodeId = IsPropertyExist(pagination.Filters, "schemeCodeId") ? pagination.Filters?.schemeCodeId : null;
            long? c5CodeId = IsPropertyExist(pagination.Filters, "c5CodeId") ? pagination.Filters?.c5CodeId : null;
            long? cardTypeId = IsPropertyExist(pagination.Filters, "cardTypeId") ? pagination.Filters?.cardTypeId : null;
            long? regionId = IsPropertyExist(pagination.Filters, "regionId") ? pagination.Filters?.regionId : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;

            DateTime? _indentStartDate = null;
            if (!string.IsNullOrEmpty(indentStartDate))
            {
                _indentStartDate = CommonUtils.GetParseDate(indentStartDate);
            }

            DateTime? _indentEndDate = null;
            if (!string.IsNullOrEmpty(indentEndDate))
            {
                _indentEndDate = CommonUtils.GetParseDate(indentEndDate);
            }

            DateTime? _dispatchStartDate = null;
            if (!string.IsNullOrEmpty(dispatchStartDate))
            {
                _dispatchStartDate = CommonUtils.GetParseDate(dispatchStartDate);
            }

            DateTime? _dispatchEndDate = null;
            if (!string.IsNullOrEmpty(dispatchEndDate))
            {
                _dispatchEndDate = CommonUtils.GetParseDate(dispatchEndDate);
            }

            IRepository<ReportModel> repositoryReportModel = new Repository<ReportModel>(TkmsDbContext);

            var query = (from di in TkmsDbContext.Dispatches
                         join i in TkmsDbContext.Indents on di.IndentId equals i.IndentId
                         join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                         from ib in ibs.DefaultIfEmpty()
                         join bf in TkmsDbContext.Branches on i.BfilBranchId equals bf.BranchId
                         join bt in TkmsDbContext.BranchTypes on i.BfilBranchTypeId equals bt.BranchTypeId
                         join sc in TkmsDbContext.SchemeCodes on i.SchemeCodeId equals sc.SchemeCodeId
                         join ct in TkmsDbContext.CardTypes on i.CardTypeId equals ct.CardTypeId
                         join reuser in TkmsDbContext.Users on di.CreatedBy equals reuser.UserId into reusers
                         from reuser in reusers.DefaultIfEmpty()
                         where !i.IsDeleted
                         && (string.IsNullOrEmpty(indentNo) || i.IndentNo.Contains(indentNo))
                         && (!_indentStartDate.HasValue || _indentStartDate.Value.Date <= i.IndentDate.Date)
                         && (!_indentEndDate.HasValue || _indentEndDate.Value.Date >= i.IndentDate.Date)
                         && (!_dispatchStartDate.HasValue || _dispatchStartDate.Value.Date <= di.DispatchDate.Date)
                         && (!_dispatchEndDate.HasValue || _dispatchEndDate.Value.Date >= di.DispatchDate.Date)
                         && (!branchId.HasValue || branchId.Value == i.IblBranchId)
                         && (!schemeCodeId.HasValue || schemeCodeId.Value == i.SchemeCodeId)
                         && (!c5CodeId.HasValue || c5CodeId.Value == i.C5CodeId)
                         && (!cardTypeId.HasValue || cardTypeId.Value == i.CardTypeId)
                         && (!regionId.HasValue || regionId.Value == i.BfilRegionId)
                         && (!bfilBranchId.HasValue || bfilBranchId.Value == i.BfilBranchId)
                         select new ReportModel
                         {
                             IndentNumber = i.IndentNo,
                             HoApproveDate = i.HoApproveDate,
                             CpuApproveDate = i.CpuApproveDate,
                             DateOfDispatch = di.DispatchDate,
                             ReportUploadDate = di.DispatchDate,
                             ReportUploadBy = reuser != null ? reuser.FullName : "",
                             IblBranchCode = ib != null ? ib.IblBranchCode : "",
                             IblBranchName = ib != null ? ib.IblBranchName : "",
                             SchemeCode = sc.SchemeCodeName,
                             CardType = ct.CardTypeName,
                             Vendor = di.Vendor,
                             Qty = di.DispatchQty,
                             AccountStart = di.AccountStart,
                             AccountEnd = di.AccountEnd,
                             BfilBranchCode = bf.BranchCode,
                             BfilBranchName = bf.BranchName,
                             SchemeType = di.SchemeType,
                             ReferenceNumber = di.ReferenceNo,
                             ReportDate = di.DispatchDate,
                             WayBills = (from diwb in TkmsDbContext.DispatchWayBills
                                         where diwb.DispatchId == di.DispatchId
                                         select diwb.WayBillNo).AsEnumerable(),
                         });

            return await repositoryReportModel.GetPagedReponseAsync(pagination, null, query);
        }
        public async Task<PagedList> GetDeliveryReport(Pagination pagination)
        {
            string indentNo = IsPropertyExist(pagination.Filters, "indentNo") ? pagination.Filters?.indentNo : null;
            string referenceNo = IsPropertyExist(pagination.Filters, "referenceNo") ? pagination.Filters?.referenceNo : null;
            string wayBillNo = IsPropertyExist(pagination.Filters, "wayBillNo") ? pagination.Filters?.wayBillNo : null;
            string dispatchStartDate = IsPropertyExist(pagination.Filters, "dispatchStartDate") ? pagination.Filters?.dispatchStartDate : null;
            string dispatchEndDate = IsPropertyExist(pagination.Filters, "dispatchEndDate") ? pagination.Filters?.dispatchEndDate : null;
            string deliveryStartDate = IsPropertyExist(pagination.Filters, "deliveryStartDate") ? pagination.Filters?.deliveryStartDate : null;
            string deliveryEndDate = IsPropertyExist(pagination.Filters, "deliveryEndDate") ? pagination.Filters?.deliveryEndDate : null;
            long? regionId = IsPropertyExist(pagination.Filters, "regionId") ? pagination.Filters?.regionId : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;

            DateTime? _deliveryStartDate = null;
            if (!string.IsNullOrEmpty(deliveryStartDate))
            {
                _deliveryStartDate = CommonUtils.GetParseDate(deliveryStartDate);
            }

            DateTime? _deliveryEndDate = null;
            if (!string.IsNullOrEmpty(deliveryEndDate))
            {
                _deliveryEndDate = CommonUtils.GetParseDate(deliveryEndDate);
            }

            DateTime? _dispatchStartDate = null;
            if (!string.IsNullOrEmpty(dispatchStartDate))
            {
                _dispatchStartDate = CommonUtils.GetParseDate(dispatchStartDate);
            }

            DateTime? _dispatchEndDate = null;
            if (!string.IsNullOrEmpty(dispatchEndDate))
            {
                _dispatchEndDate = CommonUtils.GetParseDate(dispatchEndDate);
            }

            IRepository<ReportModel> repositoryReportModel = new Repository<ReportModel>(TkmsDbContext);

            var query = (from dibi in TkmsDbContext.DispatchWayBills
                         join di in TkmsDbContext.Dispatches on dibi.DispatchId equals di.DispatchId
                         join i in TkmsDbContext.Indents on di.IndentId equals i.IndentId
                         join cs in TkmsDbContext.CourierStatuses on dibi.CourierStatusId equals cs.CourierStatusId
                         where !i.IsDeleted
                         && (string.IsNullOrEmpty(indentNo) || i.IndentNo.Contains(indentNo))
                         && (string.IsNullOrEmpty(referenceNo) || di.ReferenceNo.Contains(referenceNo))
                         && (string.IsNullOrEmpty(wayBillNo) || dibi.WayBillNo.Contains(wayBillNo))
                         && (!_dispatchStartDate.HasValue || _dispatchStartDate.Value.Date <= di.DispatchDate.Date)
                         && (!_dispatchEndDate.HasValue || _dispatchEndDate.Value.Date >= di.DispatchDate.Date)
                         && (!_deliveryStartDate.HasValue || _deliveryStartDate.Value.Date <= dibi.DeliveryDate)
                         && (!_deliveryEndDate.HasValue || _deliveryEndDate.Value.Date >= dibi.DeliveryDate)
                         && (!regionId.HasValue || regionId.Value == i.BfilRegionId)
                         && (!bfilBranchId.HasValue || bfilBranchId.Value == i.BfilBranchId)

                         select new ReportModel
                         {
                             IndentNumber = i.IndentNo,
                             WayBillNo = dibi.WayBillNo,
                             DateOfDispatch = dibi.DispatchDate,
                             Status = cs.StatusName,
                             DeliveryDate = dibi.DeliveryDate,
                             ReceiverName = dibi.ReceiveBy,
                             CourierName = dibi.CourierPartner,
                             ReferenceNumber = di.ReferenceNo
                         });

            return await repositoryReportModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetAccountLevelDispatchReport(Pagination pagination)
        {
            string indentNo = IsPropertyExist(pagination.Filters, "indentNo") ? pagination.Filters?.indentNo : null;
            string accountNo = IsPropertyExist(pagination.Filters, "accountNo") ? pagination.Filters?.accountNo : null;
            string cifNo = IsPropertyExist(pagination.Filters, "cifNo") ? pagination.Filters?.cifNo : null;
            string dispatchStartDate = IsPropertyExist(pagination.Filters, "dispatchStartDate") ? pagination.Filters?.dispatchStartDate : null;
            string dispatchEndDate = IsPropertyExist(pagination.Filters, "dispatchEndDate") ? pagination.Filters?.dispatchEndDate : null;
            long? branchId = IsPropertyExist(pagination.Filters, "branchId") ? pagination.Filters?.branchId : null;
            long? regionId = IsPropertyExist(pagination.Filters, "regionId") ? pagination.Filters?.regionId : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;

            DateTime? _dispatchStartDate = null;
            if (!string.IsNullOrEmpty(dispatchStartDate))
            {
                _dispatchStartDate = CommonUtils.GetParseDate(dispatchStartDate);
            }

            DateTime? _dispatchEndDate = null;
            if (!string.IsNullOrEmpty(dispatchEndDate))
            {
                _dispatchEndDate = CommonUtils.GetParseDate(dispatchEndDate);
            }

            IRepository<ReportModel> repositoryReportModel = new Repository<ReportModel>(TkmsDbContext);

            var query = (from ki in TkmsDbContext.Kits
                         join di in TkmsDbContext.Dispatches on ki.DispatchId equals di.DispatchId
                         join i in TkmsDbContext.Indents on di.IndentId equals i.IndentId
                         join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                         from ib in ibs.DefaultIfEmpty()
                         join biflbr in TkmsDbContext.Branches on ki.BranchId equals biflbr.BranchId
                         join kis in TkmsDbContext.KitStatuses on ki.KitStatusId equals kis.KitStatusId
                         join reuser in TkmsDbContext.Users on di.CreatedBy equals reuser.UserId into reusers
                         from reuser in reusers.DefaultIfEmpty()

                         where !i.IsDeleted
                         && (string.IsNullOrEmpty(indentNo) || i.IndentNo.Contains(indentNo))
                         && (string.IsNullOrEmpty(accountNo) || ki.AccountNo.Contains(accountNo))
                         && (string.IsNullOrEmpty(cifNo) || ki.CifNo.Contains(cifNo))
                         && (!_dispatchStartDate.HasValue || _dispatchStartDate.Value.Date <= di.DispatchDate.Date)
                         && (!_dispatchEndDate.HasValue || _dispatchEndDate.Value.Date >= di.DispatchDate.Date)
                         && (!branchId.HasValue || branchId.Value == i.IblBranchId)
                         && (!regionId.HasValue || regionId.Value == i.BfilRegionId)
                         && (!bfilBranchId.HasValue || bfilBranchId.Value == ki.BranchId)

                         select new ReportModel
                         {
                             IndentNumber = i.IndentNo,
                             HoApproveDate = i.HoApproveDate,
                             CpuApproveDate = i.CpuApproveDate,
                             DateOfDispatch = di.DispatchDate,
                             ReportUploadDate = di.DispatchDate,
                             ReportUploadBy = reuser != null ? reuser.FullName : "",
                             DateOfUpload = di.DispatchDate,
                             IblBranchCode = ib != null ? ib.IblBranchCode : "",
                             IblBranchName = ib != null ? ib.IblBranchName : "",
                             BfilBranchCode = biflbr.BranchCode,
                             BfilBranchName = biflbr.BranchName,
                             CifNo = ki.CifNo,
                             AccountNo = ki.AccountNo,
                             Status = kis.StatusName,
                         }); ;

            return await repositoryReportModel.GetPagedReponseAsync(pagination, null, query);
        }
        public async Task<PagedList> GetReceivedAtROReport(Pagination pagination)
        {
            string indentNo = IsPropertyExist(pagination.Filters, "indentNo") ? pagination.Filters?.indentNo : null;
            string referenceNo = IsPropertyExist(pagination.Filters, "referenceNo") ? pagination.Filters?.referenceNo : null;
            string wayBillNo = IsPropertyExist(pagination.Filters, "wayBillNo") ? pagination.Filters?.wayBillNo : null;
            string dispatchStartDate = IsPropertyExist(pagination.Filters, "dispatchStartDate") ? pagination.Filters?.dispatchStartDate : null;
            string dispatchEndDate = IsPropertyExist(pagination.Filters, "dispatchEndDate") ? pagination.Filters?.dispatchEndDate : null;
            long? branchId = IsPropertyExist(pagination.Filters, "branchId") ? pagination.Filters?.branchId : null;
            long? schemeCodeId = IsPropertyExist(pagination.Filters, "schemeCodeId") ? pagination.Filters?.schemeCodeId : null;
            long? cardTypeId = IsPropertyExist(pagination.Filters, "cardTypeId") ? pagination.Filters?.cardTypeId : null;
            long? regionId = IsPropertyExist(pagination.Filters, "regionId") ? pagination.Filters?.regionId : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;
            long? bfilBranchTypeId = IsPropertyExist(pagination.Filters, "bfilBranchTypeId") ? pagination.Filters?.bfilBranchTypeId : null;

            DateTime? _dispatchStartDate = null;
            if (!string.IsNullOrEmpty(dispatchStartDate))
            {
                _dispatchStartDate = CommonUtils.GetParseDate(dispatchStartDate);
            }

            DateTime? _dispatchEndDate = null;
            if (!string.IsNullOrEmpty(dispatchEndDate))
            {
                _dispatchEndDate = CommonUtils.GetParseDate(dispatchEndDate);
            }

            var dispatchStatusId = DispatchStatuses.ReceivedAtRo.GetHashCode();

            IRepository<ReportModel> repositoryReportModel = new Repository<ReportModel>(TkmsDbContext);

            var query = (from di in TkmsDbContext.Dispatches
                         .Include(d => d.DispatchWayBills)
                         join i in TkmsDbContext.Indents on di.IndentId equals i.IndentId
                         join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                         from ib in ibs.DefaultIfEmpty()
                         join biflbr in TkmsDbContext.Branches on i.BfilBranchId equals biflbr.BranchId
                         join sch in TkmsDbContext.SchemeCodes on i.SchemeCodeId equals sch.SchemeCodeId
                         join cad in TkmsDbContext.CardTypes on i.CardTypeId equals cad.CardTypeId
                         join dbins in TkmsDbContext.Users on di.CreatedBy equals dbins.UserId into hostatuses
                         from dbins in hostatuses.DefaultIfEmpty()

                         join diau in TkmsDbContext.DispatchAudits.Where(da => da.DispatchStatusId == dispatchStatusId) on di.DispatchId equals diau.DispatchId into diaus
                         from diau in diaus.DefaultIfEmpty()

                         join diaud in TkmsDbContext.Users on diau.UpdatedBy equals diaud.UserId into diauds
                         from diaud in diauds.DefaultIfEmpty()

                         where !i.IsDeleted
                         && (string.IsNullOrEmpty(indentNo) || i.IndentNo.Contains(indentNo))
                         && (string.IsNullOrEmpty(referenceNo) || di.ReferenceNo.Contains(referenceNo))
                         && (string.IsNullOrEmpty(wayBillNo) || di.DispatchWayBills.Any(dw => dw.WayBillNo == wayBillNo))
                         && (!_dispatchStartDate.HasValue || _dispatchStartDate.Value.Date <= di.DispatchDate.Date)
                         && (!_dispatchEndDate.HasValue || _dispatchEndDate.Value.Date >= di.DispatchDate.Date)
                         && (!branchId.HasValue || branchId.Value == i.IblBranchId)
                         && (!schemeCodeId.HasValue || schemeCodeId.Value == i.SchemeCodeId)
                         && (!cardTypeId.HasValue || cardTypeId.Value == i.CardTypeId)
                         && (!bfilBranchTypeId.HasValue || bfilBranchTypeId.Value == i.BfilBranchTypeId)
                         && (!regionId.HasValue || regionId.Value == i.BfilRegionId)
                         && (!bfilBranchId.HasValue || bfilBranchId.Value == i.BfilBranchId)

                         select new ReportModel
                         {
                             IndentNumber = i.IndentNo,
                             HoApproveDate = i.HoApproveDate,
                             CpuApproveDate = i.CpuApproveDate,
                             DateOfDispatch = di.DispatchDate,
                             ReportUploadDate = di.DispatchDate,
                             ReportUploadBy = dbins != null ? dbins.FullName : "",
                             IblBranchCode = ib != null ? ib.IblBranchCode : "",
                             IblBranchName = ib != null ? ib.IblBranchName : "",
                             SchemeCode = sch.SchemeCodeName,
                             CardType = cad.CardTypeName,
                             Vendor = di.Vendor,
                             Qty = di.DispatchQty,
                             AccountStart = di.AccountStart,
                             AccountEnd = di.AccountEnd,
                             BfilBranchCode = biflbr.BranchCode,
                             BfilBranchName = biflbr.BranchName,
                             SchemeType = di.SchemeType,
                             ReportDate = diau.UpdatedDate,
                             ReferenceNumber = di.ReferenceNo,
                             WayBills = (from diwb in TkmsDbContext.DispatchWayBills
                                         where diwb.DispatchId == di.DispatchId
                                         select diwb.WayBillNo).AsEnumerable(),
                             Status = diau != null ? "Received" : "Not Received",
                             User = diaud != null ? diaud.FullName : "",
                             UserConfirmationDate = diaud != null ? diau.UpdatedDate : null,
                         });

            return await repositoryReportModel.GetPagedReponseAsync(pagination, null, query);
        }
        public async Task<PagedList> GetReceivedAtBranchReport(Pagination pagination)
        {
            string indentNo = IsPropertyExist(pagination.Filters, "indentNo") ? pagination.Filters?.indentNo : null;
            string referenceNo = IsPropertyExist(pagination.Filters, "referenceNo") ? pagination.Filters?.referenceNo : null;
            string wayBillNo = IsPropertyExist(pagination.Filters, "wayBillNo") ? pagination.Filters?.wayBillNo : null;
            string dispatchStartDate = IsPropertyExist(pagination.Filters, "dispatchStartDate") ? pagination.Filters?.dispatchStartDate : null;
            string dispatchEndDate = IsPropertyExist(pagination.Filters, "dispatchEndDate") ? pagination.Filters?.dispatchEndDate : null;
            long? branchId = IsPropertyExist(pagination.Filters, "branchId") ? pagination.Filters?.branchId : null;
            long? schemeCodeId = IsPropertyExist(pagination.Filters, "schemeCodeId") ? pagination.Filters?.schemeCodeId : null;
            long? cardTypeId = IsPropertyExist(pagination.Filters, "cardTypeId") ? pagination.Filters?.cardTypeId : null;
            long? regionId = IsPropertyExist(pagination.Filters, "regionId") ? pagination.Filters?.regionId : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;
            long? bfilBranchTypeId = IsPropertyExist(pagination.Filters, "bfilBranchTypeId") ? pagination.Filters?.bfilBranchTypeId : null;

            DateTime? _dispatchStartDate = null;
            if (!string.IsNullOrEmpty(dispatchStartDate))
            {
                _dispatchStartDate = CommonUtils.GetParseDate(dispatchStartDate);
            }

            DateTime? _dispatchEndDate = null;
            if (!string.IsNullOrEmpty(dispatchEndDate))
            {
                _dispatchEndDate = CommonUtils.GetParseDate(dispatchEndDate);
            }

            var dispatchStatusId = DispatchStatuses.Received.GetHashCode();

            IRepository<ReportModel> repositoryReportModel = new Repository<ReportModel>(TkmsDbContext);

            var query = (from di in TkmsDbContext.Dispatches
                         join i in TkmsDbContext.Indents on di.IndentId equals i.IndentId
                         join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                         from ib in ibs.DefaultIfEmpty()
                         join biflbr in TkmsDbContext.Branches on i.BfilBranchId equals biflbr.BranchId
                         join sch in TkmsDbContext.SchemeCodes on i.SchemeCodeId equals sch.SchemeCodeId
                         join cad in TkmsDbContext.CardTypes on i.CardTypeId equals cad.CardTypeId
                         join diuse in TkmsDbContext.Users on di.CreatedBy equals diuse.UserId into diuses
                         from diuse in diuses.DefaultIfEmpty()

                         join diau in TkmsDbContext.DispatchAudits.Where(da => da.DispatchStatusId == dispatchStatusId) on di.DispatchId equals diau.DispatchId into diaus
                         from diau in diaus.DefaultIfEmpty()

                         join diaud in TkmsDbContext.Users on diau.UpdatedBy equals diaud.UserId into diauds
                         from diaud in diauds.DefaultIfEmpty()

                         where !i.IsDeleted
                         && (string.IsNullOrEmpty(indentNo) || i.IndentNo.Contains(indentNo))
                         && (string.IsNullOrEmpty(referenceNo) || di.ReferenceNo.Contains(referenceNo))
                         && (string.IsNullOrEmpty(wayBillNo) || di.DispatchWayBills.Any(dw => dw.WayBillNo == wayBillNo))
                         && (!_dispatchStartDate.HasValue || _dispatchStartDate.Value.Date <= di.DispatchDate.Date)
                         && (!_dispatchEndDate.HasValue || _dispatchEndDate.Value.Date >= di.DispatchDate.Date)
                         && (!branchId.HasValue || branchId.Value == i.IblBranchId)
                         && (!schemeCodeId.HasValue || schemeCodeId.Value == i.SchemeCodeId)
                         && (!cardTypeId.HasValue || cardTypeId.Value == i.CardTypeId)
                         && (!bfilBranchTypeId.HasValue || bfilBranchTypeId.Value == i.BfilBranchTypeId)
                         && (!regionId.HasValue || regionId.Value == i.BfilRegionId)
                         && (!bfilBranchId.HasValue || bfilBranchId.Value == i.BfilBranchId)

                         select new ReportModel
                         {
                             IndentNumber = i.IndentNo,
                             HoApproveDate = i.HoApproveDate,
                             CpuApproveDate = i.CpuApproveDate,
                             DateOfDispatch = di.DispatchDate,
                             ReportUploadDate = di.DispatchDate,
                             ReportUploadBy = diuse != null ? diuse.FullName : "",
                             IblBranchCode = ib != null ? ib.IblBranchCode : "",
                             IblBranchName = ib != null ? ib.IblBranchName : "",
                             SchemeCode = sch.SchemeCodeName,
                             CardType = cad.CardTypeName,
                             Vendor = di.Vendor,
                             Qty = di.DispatchQty,
                             AccountStart = di.AccountStart,
                             AccountEnd = di.AccountEnd,
                             BfilBranchCode = biflbr.BranchCode,
                             BfilBranchName = biflbr.BranchName,
                             SchemeType = di.SchemeType,
                             ReportDate = di.UpdatedDate,
                             ReferenceNumber = di.ReferenceNo,
                             WayBills = (from diwb in TkmsDbContext.DispatchWayBills
                                         where diwb.DispatchId == di.DispatchId
                                         select diwb.WayBillNo).AsEnumerable(),
                             Status = diau != null ? "Received" : "Not Received",
                             User = diaud != null ? diaud.FullName : "",
                             UserConfirmationDate = diaud != null ? diau.UpdatedDate : null,
                         });

            return await repositoryReportModel.GetPagedReponseAsync(pagination, null, query);
        }
        public async Task<PagedList> GetScannedReport(Pagination pagination)
        {
            string indentNo = IsPropertyExist(pagination.Filters, "indentNo") ? pagination.Filters?.indentNo : null;
            string accountNo = IsPropertyExist(pagination.Filters, "accountNo") ? pagination.Filters?.accountNo : null;
            string cifNo = IsPropertyExist(pagination.Filters, "cifNo") ? pagination.Filters?.cifNo : null;
            string dispatchStartDate = IsPropertyExist(pagination.Filters, "dispatchStartDate") ? pagination.Filters?.dispatchStartDate : null;
            string dispatchEndDate = IsPropertyExist(pagination.Filters, "dispatchEndDate") ? pagination.Filters?.dispatchEndDate : null;
            long? branchId = IsPropertyExist(pagination.Filters, "branchId") ? pagination.Filters?.branchId : null;
            long? regionId = IsPropertyExist(pagination.Filters, "regionId") ? pagination.Filters?.regionId : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;

            DateTime? _dispatchStartDate = null;
            if (!string.IsNullOrEmpty(dispatchStartDate))
            {
                _dispatchStartDate = CommonUtils.GetParseDate(dispatchStartDate);
            }

            DateTime? _dispatchEndDate = null;
            if (!string.IsNullOrEmpty(dispatchEndDate))
            {
                _dispatchEndDate = CommonUtils.GetParseDate(dispatchEndDate);
            }

            var kitStatusId = KitStatuses.Received.GetHashCode();

            IRepository<ReportModel> repositoryReportModel = new Repository<ReportModel>(TkmsDbContext);

            var query = (from ki in TkmsDbContext.Kits
                         join di in TkmsDbContext.Dispatches on ki.DispatchId equals di.DispatchId
                         join i in TkmsDbContext.Indents on di.IndentId equals i.IndentId
                         join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                         from ib in ibs.DefaultIfEmpty()
                         join biflbr in TkmsDbContext.Branches on ki.BranchId equals biflbr.BranchId
                         join kiS in TkmsDbContext.KitStatuses on ki.KitStatusId equals kiS.KitStatusId

                         join diuserc in TkmsDbContext.Users on di.CreatedBy equals diuserc.UserId into diusercs
                         from diuserc in diusercs.DefaultIfEmpty()

                         join diuseru in TkmsDbContext.Users on di.UpdatedBy equals diuseru.UserId into diuserus
                         from diuseru in diuserus.DefaultIfEmpty()

                         join kiu in TkmsDbContext.Users on ki.UpdatedBy equals kiu.UserId into kius
                         from kiu in kius.DefaultIfEmpty()

                         where !i.IsDeleted
                         && ki.KitStatusId == kitStatusId
                         && (string.IsNullOrEmpty(indentNo) || i.IndentNo.Contains(indentNo))
                         && (string.IsNullOrEmpty(accountNo) || ki.AccountNo.Contains(accountNo))
                         && (string.IsNullOrEmpty(cifNo) || ki.CifNo.Contains(cifNo))
                         && (!_dispatchStartDate.HasValue || _dispatchStartDate.Value.Date <= di.DispatchDate.Date)
                         && (!_dispatchEndDate.HasValue || _dispatchEndDate.Value.Date >= di.DispatchDate.Date)
                         && (!branchId.HasValue || branchId.Value == i.IblBranchId)
                         && (!regionId.HasValue || regionId.Value == i.BfilRegionId)
                         && (!bfilBranchId.HasValue || bfilBranchId.Value == ki.BranchId)

                         select new ReportModel
                         {
                             IndentNumber = i.IndentNo,
                             HoApproveDate = i.HoApproveDate,
                             CpuApproveDate = i.CpuApproveDate,
                             DateOfDispatch = di.DispatchDate,
                             ReportUploadDate = di.DispatchDate,
                             ReportUploadBy = diuserc != null ? diuserc.FullName : "",
                             DateOfUpload = di.CreatedDate,
                             IblBranchCode = ib != null ? ib.IblBranchCode : "",
                             IblBranchName = ib != null ? ib.IblBranchName : "",
                             BfilBranchCode = biflbr.BranchCode,
                             BfilBranchName = biflbr.BranchName,
                             CifNo = ki.CifNo,
                             AccountNo = ki.AccountNo,
                             Status = kiS.StatusName,
                             ReceivedDate = di.UpdatedDate,
                             ReceivedBy = diuseru != null ? diuseru.FullName : "",
                             ReportDate = di.UpdatedDate,
                             ReportStatus = "Received",
                             ScannedBy = kiu != null ? kiu.FullName : "",
                             ScannedDate = ki.UpdatedDate,
                         })
                         .Distinct();

            return await repositoryReportModel.GetPagedReponseAsync(pagination, null, query);
        }
        public async Task<PagedList> GetAllocationReport(Pagination pagination)
        {
            string indentNo = IsPropertyExist(pagination.Filters, "indentNo") ? pagination.Filters?.indentNo : null;
            string accountNo = IsPropertyExist(pagination.Filters, "accountNo") ? pagination.Filters?.accountNo : null;
            string cifNo = IsPropertyExist(pagination.Filters, "cifNo") ? pagination.Filters?.cifNo : null;
            string dispatchStartDate = IsPropertyExist(pagination.Filters, "dispatchStartDate") ? pagination.Filters?.dispatchStartDate : null;
            string dispatchEndDate = IsPropertyExist(pagination.Filters, "dispatchEndDate") ? pagination.Filters?.dispatchEndDate : null;
            long? branchId = IsPropertyExist(pagination.Filters, "branchId") ? pagination.Filters?.branchId : null;
            long? regionId = IsPropertyExist(pagination.Filters, "regionId") ? pagination.Filters?.regionId : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;

            DateTime? _dispatchStartDate = null;
            if (!string.IsNullOrEmpty(dispatchStartDate))
            {
                _dispatchStartDate = CommonUtils.GetParseDate(dispatchStartDate);
            }

            DateTime? _dispatchEndDate = null;
            if (!string.IsNullOrEmpty(dispatchEndDate))
            {
                _dispatchEndDate = CommonUtils.GetParseDate(dispatchEndDate);
            }

            var kitStatusId = KitStatuses.Allocated.GetHashCode();
            var received = KitStatuses.Received.GetHashCode();

            IRepository<ReportModel> repositoryReportModel = new Repository<ReportModel>(TkmsDbContext);

            var query = (from ki in TkmsDbContext.Kits
                         join di in TkmsDbContext.Dispatches on ki.DispatchId equals di.DispatchId
                         join i in TkmsDbContext.Indents on di.IndentId equals i.IndentId
                         join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                         from ib in ibs.DefaultIfEmpty()
                         join biflbr in TkmsDbContext.Branches on ki.BranchId equals biflbr.BranchId
                         join kiS in TkmsDbContext.KitStatuses on ki.KitStatusId equals kiS.KitStatusId

                         join diuserc in TkmsDbContext.Users on di.CreatedBy equals diuserc.UserId into diusercs
                         from diuserc in diusercs.DefaultIfEmpty()

                         join diuseru in TkmsDbContext.Users on di.UpdatedBy equals diuseru.UserId into diuserus
                         from diuseru in diuserus.DefaultIfEmpty()

                         join kir in TkmsDbContext.KitAudits.Where(da => da.KitStatusId == received) on ki.KitId equals kir.KitId into kirs
                         from kir in kirs.DefaultIfEmpty()

                         join kiru in TkmsDbContext.Users on kir.UpdatedBy equals kiru.UserId into kirus
                         from kiru in kirus.DefaultIfEmpty()

                         join kiatu in TkmsDbContext.Users on ki.AllocatedToId equals kiatu.UserId into kiatus
                         from kiatu in kiatus.DefaultIfEmpty()

                         join kiabu in TkmsDbContext.Users on ki.AllocatedToId equals kiabu.UserId into kiabus
                         from kiabu in kiabus.DefaultIfEmpty()

                         where !i.IsDeleted
                          && ki.KitStatusId == kitStatusId
                          && (string.IsNullOrEmpty(indentNo) || i.IndentNo.Contains(indentNo))
                          && (string.IsNullOrEmpty(accountNo) || ki.AccountNo.Contains(accountNo))
                          && (string.IsNullOrEmpty(cifNo) || ki.CifNo.Contains(cifNo))
                          && (!_dispatchStartDate.HasValue || _dispatchStartDate.Value.Date <= di.DispatchDate.Date)
                          && (!_dispatchEndDate.HasValue || _dispatchEndDate.Value.Date >= di.DispatchDate.Date)
                          && (!branchId.HasValue || branchId.Value == i.IblBranchId)
                          && (!regionId.HasValue || regionId.Value == i.BfilRegionId)
                          && (!bfilBranchId.HasValue || bfilBranchId.Value == ki.BranchId)

                         select new ReportModel
                         {
                             IndentNumber = i.IndentNo,
                             HoApproveDate = i.HoApproveDate,
                             CpuApproveDate = i.CpuApproveDate,
                             DateOfDispatch = di.DispatchDate,
                             ReportUploadDate = di.DispatchDate,
                             ReportUploadBy = diuserc != null ? diuserc.FullName : "",
                             DateOfUpload = di.CreatedDate,
                             IblBranchCode = ib != null ? ib.IblBranchCode : "",
                             IblBranchName = ib != null ? ib.IblBranchName : "",
                             BfilBranchCode = biflbr.BranchCode,
                             BfilBranchName = biflbr.BranchName,
                             CifNo = ki.CifNo,
                             AccountNo = ki.AccountNo,
                             Status = kiS.StatusName,
                             ReceivedDate = di.UpdatedDate,
                             ReceivedBy = diuseru != null ? diuseru.FullName : "",
                             ReportDate = di.UpdatedDate,
                             ReportStatus = "Allocated",
                             ScannedBy = kiru != null ? kiru.FullName : "",
                             ScannedDate = kir != null ? kir.UpdatedDate : null,
                             AllocatedToStaffId = kiatu != null ? kiatu.StaffId : "",
                             AllocatedTo = kiatu != null ? kiatu.FullName : "",
                             AllocatedBy = kiabu != null ? kiabu.FullName : "",
                             AllocatedDate = ki.AllocatedDate,
                         })
                         .Distinct();

            return await repositoryReportModel.GetPagedReponseAsync(pagination, null, query);
        }
        public async Task<PagedList> GetAssignedReport(Pagination pagination)
        {
            string indentNo = IsPropertyExist(pagination.Filters, "indentNo") ? pagination.Filters?.indentNo : null;
            string accountNo = IsPropertyExist(pagination.Filters, "accountNo") ? pagination.Filters?.accountNo : null;
            string cifNo = IsPropertyExist(pagination.Filters, "cifNo") ? pagination.Filters?.cifNo : null;
            string dispatchStartDate = IsPropertyExist(pagination.Filters, "dispatchStartDate") ? pagination.Filters?.dispatchStartDate : null;
            string dispatchEndDate = IsPropertyExist(pagination.Filters, "dispatchEndDate") ? pagination.Filters?.dispatchEndDate : null;
            long? branchId = IsPropertyExist(pagination.Filters, "branchId") ? pagination.Filters?.branchId : null;
            long? regionId = IsPropertyExist(pagination.Filters, "regionId") ? pagination.Filters?.regionId : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;

            DateTime? _dispatchStartDate = null;
            if (!string.IsNullOrEmpty(dispatchStartDate))
            {
                _dispatchStartDate = CommonUtils.GetParseDate(dispatchStartDate);
            }

            DateTime? _dispatchEndDate = null;
            if (!string.IsNullOrEmpty(dispatchEndDate))
            {
                _dispatchEndDate = CommonUtils.GetParseDate(dispatchEndDate);
            }

            var kitStatusId = KitStatuses.Assigned.GetHashCode();
            var received = KitStatuses.Received.GetHashCode();

            IRepository<ReportModel> repositoryReportModel = new Repository<ReportModel>(TkmsDbContext);

            var query = (from ki in TkmsDbContext.Kits
                         join di in TkmsDbContext.Dispatches on ki.DispatchId equals di.DispatchId
                         join i in TkmsDbContext.Indents on di.IndentId equals i.IndentId
                         join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                         from ib in ibs.DefaultIfEmpty()
                         join biflbr in TkmsDbContext.Branches on ki.BranchId equals biflbr.BranchId
                         join kiS in TkmsDbContext.KitStatuses on ki.KitStatusId equals kiS.KitStatusId

                         join diuserc in TkmsDbContext.Users on di.CreatedBy equals diuserc.UserId into diusercs
                         from diuserc in diusercs.DefaultIfEmpty()

                         join diuseru in TkmsDbContext.Users on di.UpdatedBy equals diuseru.UserId into diuserus
                         from diuseru in diuserus.DefaultIfEmpty()

                         join kir in TkmsDbContext.KitAudits.Where(da => da.KitStatusId == received) on ki.KitId equals kir.KitId into kirs
                         from kir in kirs.DefaultIfEmpty()

                         join kiru in TkmsDbContext.Users on kir.UpdatedBy equals kiru.UserId into kirus
                         from kiru in kirus.DefaultIfEmpty()

                         join kiatu in TkmsDbContext.Users on ki.AllocatedToId equals kiatu.UserId into kiatus
                         from kiatu in kiatus.DefaultIfEmpty()

                         join kiabu in TkmsDbContext.Users on ki.AllocatedToId equals kiabu.UserId into kiabus
                         from kiabu in kiabus.DefaultIfEmpty()

                         where !i.IsDeleted
                          && ki.KitStatusId == kitStatusId
                          && (string.IsNullOrEmpty(indentNo) || i.IndentNo.Contains(indentNo))
                          && (string.IsNullOrEmpty(accountNo) || ki.AccountNo.Contains(accountNo))
                          && (string.IsNullOrEmpty(cifNo) || ki.CifNo.Contains(cifNo))
                          && (!_dispatchStartDate.HasValue || _dispatchStartDate.Value.Date <= di.DispatchDate.Date)
                          && (!_dispatchEndDate.HasValue || _dispatchEndDate.Value.Date >= di.DispatchDate.Date)
                          && (!branchId.HasValue || branchId.Value == i.IblBranchId)
                          && (!regionId.HasValue || regionId.Value == i.BfilRegionId)
                          && (!bfilBranchId.HasValue || bfilBranchId.Value == ki.BranchId)

                         select new ReportModel
                         {
                             IndentNumber = i.IndentNo,
                             HoApproveDate = i.HoApproveDate,
                             CpuApproveDate = i.CpuApproveDate,
                             DateOfDispatch = di.DispatchDate,
                             ReportUploadDate = di.DispatchDate,
                             ReportUploadBy = diuserc != null ? diuserc.FullName : "",
                             DateOfUpload = di.CreatedDate,
                             IblBranchCode = ib != null ? ib.IblBranchCode : "",
                             IblBranchName = ib != null ? ib.IblBranchName : "",
                             BfilBranchCode = biflbr.BranchCode,
                             BfilBranchName = biflbr.BranchName,
                             CifNo = ki.CifNo,
                             AccountNo = ki.AccountNo,
                             Status = kiS.StatusName,
                             ReceivedDate = di.UpdatedDate,
                             ReceivedBy = diuseru != null ? diuseru.FullName : "",
                             ReportDate = di.UpdatedDate,
                             ReportStatus = "Assigned",
                             ScannedBy = kiru != null ? kiru.FullName : "",
                             ScannedDate = kir != null ? kir.UpdatedDate : null,
                             AllocatedToStaffId = kiatu != null ? kiatu.StaffId : "",
                             AllocatedTo = kiatu != null ? kiatu.FullName : "",
                             AllocatedBy = kiabu != null ? kiabu.FullName : "",
                             AllocatedDate = ki.AllocatedDate,
                             AssignedDate = ki.AssignedDate,
                             AssignedTo = ki.CustomerName
                         });

            return await repositoryReportModel.GetPagedReponseAsync(pagination, null, query);
        }
        public async Task<PagedList> GetDestructionReport(Pagination pagination)
        {
            string indentNo = IsPropertyExist(pagination.Filters, "indentNo") ? pagination.Filters?.indentNo : null;
            string accountNo = IsPropertyExist(pagination.Filters, "accountNo") ? pagination.Filters?.accountNo : null;
            string cifNo = IsPropertyExist(pagination.Filters, "cifNo") ? pagination.Filters?.cifNo : null;
            string dispatchStartDate = IsPropertyExist(pagination.Filters, "dispatchStartDate") ? pagination.Filters?.dispatchStartDate : null;
            string dispatchEndDate = IsPropertyExist(pagination.Filters, "dispatchEndDate") ? pagination.Filters?.dispatchEndDate : null;
            long? branchId = IsPropertyExist(pagination.Filters, "branchId") ? pagination.Filters?.branchId : null;
            long? regionId = IsPropertyExist(pagination.Filters, "regionId") ? pagination.Filters?.regionId : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;

            DateTime? _dispatchStartDate = null;
            if (!string.IsNullOrEmpty(dispatchStartDate))
            {
                _dispatchStartDate = CommonUtils.GetParseDate(dispatchStartDate);
            }

            DateTime? _dispatchEndDate = null;
            if (!string.IsNullOrEmpty(dispatchEndDate))
            {
                _dispatchEndDate = CommonUtils.GetParseDate(dispatchEndDate);
            }

            var kitStatusIds = new List<long> { KitStatuses.Damaged.GetHashCode(), KitStatuses.Destruction.GetHashCode() };
            var received = KitStatuses.Received.GetHashCode();

            IRepository<ReportModel> repositoryReportModel = new Repository<ReportModel>(TkmsDbContext);

            var query = (from ki in TkmsDbContext.Kits
                         join di in TkmsDbContext.Dispatches on ki.DispatchId equals di.DispatchId
                         join i in TkmsDbContext.Indents on di.IndentId equals i.IndentId
                         join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                         from ib in ibs.DefaultIfEmpty()
                         join biflbr in TkmsDbContext.Branches on ki.BranchId equals biflbr.BranchId
                         join kiS in TkmsDbContext.KitStatuses on ki.KitStatusId equals kiS.KitStatusId

                         join diuserc in TkmsDbContext.Users on di.CreatedBy equals diuserc.UserId into diusercs
                         from diuserc in diusercs.DefaultIfEmpty()

                         join diuseru in TkmsDbContext.Users on di.UpdatedBy equals diuseru.UserId into diuserus
                         from diuseru in diuserus.DefaultIfEmpty()

                         join kir in TkmsDbContext.KitAudits.Where(da => da.KitStatusId == received) on ki.KitId equals kir.KitId into kirs
                         from kir in kirs.DefaultIfEmpty()

                         join kiru in TkmsDbContext.Users on kir.UpdatedBy equals kiru.UserId into kirus
                         from kiru in kirus.DefaultIfEmpty()

                         join kitDmg in TkmsDbContext.KitDamageReasons on ki.KitDamageReasonId equals kitDmg.KitDamageReasonId into kitDmgs
                         from kitDmg in kitDmgs.DefaultIfEmpty()

                         join kitDeau in TkmsDbContext.Users on ki.DestructionById equals kitDeau.UserId into kitDeaus
                         from kitDeau in kitDeaus.DefaultIfEmpty()

                         where !i.IsDeleted
                          && kitStatusIds.Contains(ki.KitStatusId)
                          && (string.IsNullOrEmpty(indentNo) || i.IndentNo.Contains(indentNo))
                          && (string.IsNullOrEmpty(accountNo) || ki.AccountNo.Contains(accountNo))
                          && (string.IsNullOrEmpty(cifNo) || ki.CifNo.Contains(cifNo))
                          && (!_dispatchStartDate.HasValue || _dispatchStartDate.Value.Date <= di.DispatchDate.Date)
                          && (!_dispatchEndDate.HasValue || _dispatchEndDate.Value.Date >= di.DispatchDate.Date)
                          && (!branchId.HasValue || branchId.Value == i.IblBranchId)
                          && (!regionId.HasValue || regionId.Value == i.BfilRegionId)
                          && (!bfilBranchId.HasValue || bfilBranchId.Value == ki.BranchId)

                         select new ReportModel
                         {
                             IndentNumber = i.IndentNo,
                             HoApproveDate = i.HoApproveDate,
                             CpuApproveDate = i.CpuApproveDate,
                             DateOfDispatch = di.DispatchDate,
                             ReportUploadDate = di.DispatchDate,
                             ReportUploadBy = diuserc != null ? diuserc.FullName : "",
                             DateOfUpload = di.CreatedDate,
                             IblBranchCode = ib != null ? ib.IblBranchCode : "",
                             IblBranchName = ib != null ? ib.IblBranchName : "",
                             BfilBranchCode = biflbr.BranchCode,
                             BfilBranchName = biflbr.BranchName,
                             CifNo = ki.CifNo,
                             AccountNo = ki.AccountNo,
                             Status = kiS.StatusName,
                             ReceivedDate = di.UpdatedDate,
                             ReceivedBy = diuseru != null ? diuseru.FullName : "",
                             ReportDate = di.UpdatedDate,
                             ReportStatus = "Destruction",
                             ScannedBy = kiru != null ? kiru.FullName : "",
                             ScannedDate = kir != null ? kir.UpdatedDate : null,
                             Reason = kitDmg.Reason,
                             DesturtionApproved = ki.IsDestructionApproved.HasValue ? (ki.IsDestructionApproved.Value ? "Approved" : "Rejected") : "Pending Approval",
                             DestructionBy = kitDeau != null ? kitDeau.FullName : "",
                             DestructionByDate = ki.UpdatedDate
                         });

            return await repositoryReportModel.GetPagedReponseAsync(pagination, null, query);
        }
        public async Task<PagedList> GetTotalStockReport(Pagination pagination)
        {
            long? regionId = IsPropertyExist(pagination.Filters, "regionId") ? pagination.Filters?.regionId : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;

            var bfilBranchIds = (from i in TkmsDbContext.Indents
                                 where !i.IsDeleted
                                 && (!regionId.HasValue || regionId.Value == i.BfilRegionId)
                                 && (!bfilBranchId.HasValue || bfilBranchId.Value == i.BfilBranchId)
                                 select i.BfilBranchId).Distinct();

            var dispatched = KitStatuses.Dispatched.GetHashCode();
            var received = KitStatuses.Received.GetHashCode();
            var allocated = KitStatuses.Allocated.GetHashCode();
            var assigned = KitStatuses.Assigned.GetHashCode();
            var destruction = KitStatuses.Destruction.GetHashCode();

            IRepository<dynamic> repositoryReportModel = new Repository<dynamic>(TkmsDbContext);
            var query = (from b in TkmsDbContext.Branches
                         where bfilBranchIds.Contains(b.BranchId)
                         select new StockReportModel
                         {
                             BfilBranchCode = b.BranchCode,
                             BfilBranchName = b.BranchName,
                             TotalIndented = (from ki in TkmsDbContext.Kits
                                              where ki.BranchId == b.BranchId
                                              select ki).Count(),
                             TotalDispatched = (from ki in TkmsDbContext.Kits
                                                where ki.BranchId == b.BranchId
                                                && ki.KitStatusId == dispatched
                                                select ki).Count(),
                             TotalReceived = (from ki in TkmsDbContext.Kits
                                              where ki.BranchId == b.BranchId
                                              && ki.KitStatusId == received
                                              select ki).Count(),
                             TotalAllocated = (from ki in TkmsDbContext.Kits
                                               where ki.BranchId == b.BranchId
                                               && ki.KitStatusId == allocated
                                               select ki).Count(),
                             TotalDestructed = (from ki in TkmsDbContext.Kits
                                                where ki.BranchId == b.BranchId
                                                && ki.KitStatusId == destruction
                                                select ki).Count(),
                             TotalAssigned = (from ki in TkmsDbContext.Kits
                                              where ki.BranchId == b.BranchId
                                              && ki.KitStatusId == assigned
                                              select ki).Count(),
                         });

            return await repositoryReportModel.GetPagedReponseAsync(pagination, null, query);
        }
        public async Task<PagedList> GetReturnReport(Pagination pagination)
        {
            string indentNo = IsPropertyExist(pagination.Filters, "indentNo") ? pagination.Filters?.indentNo : null;
            string accountNo = IsPropertyExist(pagination.Filters, "accountNo") ? pagination.Filters?.accountNo : null;
            string cifNo = IsPropertyExist(pagination.Filters, "cifNo") ? pagination.Filters?.cifNo : null;
            string dispatchStartDate = IsPropertyExist(pagination.Filters, "dispatchStartDate") ? pagination.Filters?.dispatchStartDate : null;
            string dispatchEndDate = IsPropertyExist(pagination.Filters, "dispatchEndDate") ? pagination.Filters?.dispatchEndDate : null;
            long? branchId = IsPropertyExist(pagination.Filters, "branchId") ? pagination.Filters?.branchId : null;
            long? regionId = IsPropertyExist(pagination.Filters, "regionId") ? pagination.Filters?.regionId : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;

            DateTime? _dispatchStartDate = null;
            if (!string.IsNullOrEmpty(dispatchStartDate))
            {
                _dispatchStartDate = CommonUtils.GetParseDate(dispatchStartDate);
            }

            DateTime? _dispatchEndDate = null;
            if (!string.IsNullOrEmpty(dispatchEndDate))
            {
                _dispatchEndDate = CommonUtils.GetParseDate(dispatchEndDate);
            }

            var received = KitStatuses.Received.GetHashCode();
            var allocated = KitStatuses.Allocated.GetHashCode();

            IRepository<ReportModel> repositoryReportModel = new Repository<ReportModel>(TkmsDbContext);

            var query = (from kr in TkmsDbContext.KitReturns
                         join ki in TkmsDbContext.Kits on kr.KitId equals ki.KitId
                         join di in TkmsDbContext.Dispatches on ki.DispatchId equals di.DispatchId
                         join i in TkmsDbContext.Indents on di.IndentId equals i.IndentId
                         join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                         from ib in ibs.DefaultIfEmpty()
                         join biflbr in TkmsDbContext.Branches on ki.BranchId equals biflbr.BranchId
                         join kiS in TkmsDbContext.KitStatuses on ki.KitStatusId equals kiS.KitStatusId

                         join kru in TkmsDbContext.Users on kr.ReturnBy equals kru.UserId into krus
                         from kru in krus.DefaultIfEmpty()

                         join krua in TkmsDbContext.Users on kr.ReturnAcceptedBy equals krua.UserId into kruas
                         from krua in kruas.DefaultIfEmpty()

                         join diuserc in TkmsDbContext.Users on di.CreatedBy equals diuserc.UserId into diusercs
                         from diuserc in diusercs.DefaultIfEmpty()

                         join diuseru in TkmsDbContext.Users on di.UpdatedBy equals diuseru.UserId into diuserus
                         from diuseru in diuserus.DefaultIfEmpty()

                         let kir = TkmsDbContext.KitAudits.FirstOrDefault(da => da.KitStatusId == received && da.KitId == ki.KitId)

                         join kiru in TkmsDbContext.Users on kir.UpdatedBy equals kiru.UserId into kirus
                         from kiru in kirus.DefaultIfEmpty()

                         let kia = TkmsDbContext.KitAudits.FirstOrDefault(da => da.KitStatusId == allocated && da.KitId == ki.KitId)

                         join kiatu in TkmsDbContext.Users on kia.AllocatedToId equals kiatu.UserId into kiatus
                         from kiatu in kiatus.DefaultIfEmpty()

                         join kiabu in TkmsDbContext.Users on kia.AllocatedToId equals kiabu.UserId into kiabus
                         from kiabu in kiabus.DefaultIfEmpty()

                         where !i.IsDeleted
                          && (string.IsNullOrEmpty(indentNo) || i.IndentNo.Contains(indentNo))
                          && (string.IsNullOrEmpty(accountNo) || ki.AccountNo.Contains(accountNo))
                          && (string.IsNullOrEmpty(cifNo) || ki.CifNo.Contains(cifNo))
                          && (!_dispatchStartDate.HasValue || _dispatchStartDate.Value.Date <= di.DispatchDate.Date)
                          && (!_dispatchEndDate.HasValue || _dispatchEndDate.Value.Date >= di.DispatchDate.Date)
                          && (!branchId.HasValue || branchId.Value == i.IblBranchId)
                          && (!regionId.HasValue || regionId.Value == i.BfilRegionId)
                          && (!bfilBranchId.HasValue || bfilBranchId.Value == ki.BranchId)

                         select new ReportModel
                         {
                             IndentNumber = i.IndentNo,
                             HoApproveDate = i.HoApproveDate,
                             CpuApproveDate = i.CpuApproveDate,
                             DateOfDispatch = di.DispatchDate,
                             ReportUploadDate = di.DispatchDate,
                             ReportUploadBy = diuserc != null ? diuserc.FullName : "",
                             DateOfUpload = di.CreatedDate,
                             IblBranchCode = ib != null ? ib.IblBranchCode : "",
                             IblBranchName = ib != null ? ib.IblBranchName : "",
                             BfilBranchCode = biflbr.BranchCode,
                             BfilBranchName = biflbr.BranchName,
                             CifNo = ki.CifNo,
                             AccountNo = ki.AccountNo,
                             Status = kiS.StatusName,
                             ReceivedDate = di.UpdatedDate,
                             ReceivedBy = diuseru != null ? diuseru.FullName : "",
                             ReportDate = di.UpdatedDate,
                             ReportStatus = "Return",
                             ScannedBy = kiru != null ? kiru.FullName : "",
                             ScannedDate = kir != null ? kir.UpdatedDate : null,
                             AllocatedToStaffId = kiatu != null ? kiatu.StaffId : "",
                             AllocatedTo = kiatu != null ? kiatu.FullName : "",
                             AllocatedBy = kiabu != null ? kiabu.FullName : "",
                             AllocatedDate = kia != null ? kia.AllocatedDate : null,
                             ReturnedBy = kru != null ? kru.FullName : "",
                             ReturnedDate = kru.UpdatedDate,
                             ReturnedAcceptBy = krua != null ? krua.FullName : "",
                         })
                         .Distinct();

            return await repositoryReportModel.GetPagedReponseAsync(pagination, null, query);
        }
    }
}
