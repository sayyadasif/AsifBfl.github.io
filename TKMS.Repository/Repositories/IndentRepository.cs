using Core.Repository;
using Core.Repository.Models;
using Core.Utility.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
    public class IndentRepository : Repository<Indent>, IIndentRepository
    {
        public IndentRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<long> GetIndentCount(dynamic filters = null)
        {
            string indentStatusIds = IsPropertyExist(filters, "indentStatusIds") ? filters?.indentStatusIds : null;
            string indentStartDate = IsPropertyExist(filters, "indentStartDate") ? filters?.indentStartDate : null;
            string indentEndDate = IsPropertyExist(filters, "indentEndDate") ? filters?.indentEndDate : null;
            long? regionId = IsPropertyExist(filters, "regionId") ? filters?.regionId : null;
            long? branchId = IsPropertyExist(filters, "branchId") ? filters?.branchId : null;
            long? bfilBranchId = IsPropertyExist(filters, "bfilBranchId") ? filters?.bfilBranchId : null;
            long? branchTypeId = IsPropertyExist(filters, "branchTypeId") ? filters?.branchTypeId : null;

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

            var _indentStatusIds = GetSplitValues(indentStatusIds);

            return await (from i in TkmsDbContext.Indents
                          where !i.IsDeleted
                          && !_indentStatusIds.Any() || _indentStatusIds.Contains(i.IndentStatusId)
                          && (!regionId.HasValue || regionId.Value == i.BfilRegionId)
                          && (!branchId.HasValue || branchId.Value == i.IblBranchId)
                          && (!bfilBranchId.HasValue || bfilBranchId.Value == i.BfilBranchId)
                          && (!branchTypeId.HasValue || branchTypeId.Value == i.BfilBranchTypeId)
                          && (!_indentStartDate.HasValue || _indentStartDate.Value.Date <= i.IndentDate.Date)
                          && (!_indentEndDate.HasValue || _indentEndDate.Value.Date >= i.IndentDate.Date)
                          select i).CountAsync();
        }

        public async Task<long> GetIndentKitTotal(dynamic filters = null)
        {
            string indentStatusIds = IsPropertyExist(filters, "indentStatusIds") ? filters?.indentStatusIds : null;
            string indentStartDate = IsPropertyExist(filters, "indentStartDate") ? filters?.indentStartDate : null;
            string indentEndDate = IsPropertyExist(filters, "indentEndDate") ? filters?.indentEndDate : null;
            long? regionId = IsPropertyExist(filters, "regionId") ? filters?.regionId : null;
            long? branchId = IsPropertyExist(filters, "branchId") ? filters?.branchId : null;
            long? bfilBranchId = IsPropertyExist(filters, "bfilBranchId") ? filters?.bfilBranchId : null;
            long? branchTypeId = IsPropertyExist(filters, "branchTypeId") ? filters?.branchTypeId : null;

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

            var _indentStatusIds = GetSplitValues(indentStatusIds);

            return await (from i in TkmsDbContext.Indents
                          where !i.IsDeleted
                          && !_indentStatusIds.Any() || _indentStatusIds.Contains(i.IndentStatusId)
                          && (!regionId.HasValue || regionId.Value == i.BfilRegionId)
                          && (!branchId.HasValue || branchId.Value == i.IblBranchId)
                          && (!bfilBranchId.HasValue || bfilBranchId.Value == i.BfilBranchId)
                          && (!branchTypeId.HasValue || branchTypeId.Value == i.BfilBranchTypeId)
                          && (!_indentStartDate.HasValue || _indentStartDate.Value.Date <= i.IndentDate.Date)
                          && (!_indentEndDate.HasValue || _indentEndDate.Value.Date >= i.IndentDate.Date)
                          select i.NoOfKit).SumAsync();
        }

        public async Task<PagedList> GetIndentPaged(Pagination pagination)
        {
            string indentStatusIds = IsPropertyExist(pagination.Filters, "indentStatusIds") ? pagination.Filters?.indentStatusIds : null;
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
            bool? dispatchQty = IsPropertyExist(pagination.Filters, "dispatchQty") ? pagination.Filters?.dispatchQty : null;
            bool? showStock = IsPropertyExist(pagination.Filters, "showStock") ? pagination.Filters?.showStock : null;

            var _indentStatusIds = GetSplitValues(indentStatusIds);

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

            var stockStatuses = new List<long> {
                KitStatuses.Dispatched.GetHashCode(),
                KitStatuses.Received.GetHashCode(),
                KitStatuses.Allocated.GetHashCode(),
            };

            IRepository<IndentModel> repositoryIndentModel = new Repository<IndentModel>(TkmsDbContext);
            var query = (from i in TkmsDbContext.Indents
                         where !i.IsDeleted
                         && (!_indentStatusIds.Any() || _indentStatusIds.Contains(i.IndentStatusId))
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
                         join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                         from ib in ibs.DefaultIfEmpty()
                         join bfilb in TkmsDbContext.Branches on i.BfilBranchId equals bfilb.BranchId
                         join sc in TkmsDbContext.SchemeCodes on i.SchemeCodeId equals sc.SchemeCodeId
                         join c5 in TkmsDbContext.C5Codes on i.C5CodeId equals c5.C5CodeId
                         join ct in TkmsDbContext.CardTypes on i.CardTypeId equals ct.CardTypeId
                         join bt in TkmsDbContext.BranchTypes on i.BfilBranchTypeId equals bt.BranchTypeId
                         join ist in TkmsDbContext.IndentStatuses on i.IndentStatusId equals ist.IndentStatusId
                         join rr in TkmsDbContext.RejectionReasons on i.RejectionReasonId equals rr.RejectionReasonId into rrs
                         from rr in rrs.DefaultIfEmpty()
                         select new IndentModel
                         {
                             IndentId = i.IndentId,
                             IndentNo = i.IndentNo,
                             IndentDate = i.IndentDate,
                             IblBranchCode = ib != null ? ib.IblBranchCode : "",
                             IblBranchName = ib != null ? ib.IblBranchName : "",
                             SchemeCode = sc.SchemeCodeName,
                             C5Code = c5.C5CodeName,
                             CardType = ct.CardTypeName,
                             NoOfKit = i.NoOfKit,
                             NoOfKitDispatched = dispatchQty.HasValue && dispatchQty.Value ? (from d in TkmsDbContext.Dispatches
                                                                                              where d.IndentId == i.IndentId
                                                                                              select d.DispatchQty).Sum() : 0,
                             StockPresent = !showStock.HasValue || !showStock.Value ? 0 :
                                           (from ki in TkmsDbContext.Kits
                                            where ki.BranchId == bfilb.BranchId
                                            && stockStatuses.Contains(ki.KitStatusId)
                                            select ki).Count(),
                             BfilBranchType = bt.BranchTypeName,
                             BfilBranchCode = bfilb.BranchCode,
                             IfscCode = bfilb.IfscCode,
                             RejectedReason = rr == null ? "" : rr.Reason,
                             IndentStatus = ist.StatusName,
                             IndentStatusId = i.IndentStatusId,
                             IsActive = i.IsActive,
                             Remarks = i.Remarks,
                         });

            return await repositoryIndentModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<IndentDetailModel> GetIndentDetailById(long id)
        {
            return await (from i in TkmsDbContext.Indents
                          where i.IndentId == id
                          join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                          from ib in ibs.DefaultIfEmpty()
                          join bfilb in TkmsDbContext.Branches on i.BfilBranchId equals bfilb.BranchId
                          join sc in TkmsDbContext.SchemeCodes on i.SchemeCodeId equals sc.SchemeCodeId
                          join c5 in TkmsDbContext.C5Codes on i.C5CodeId equals c5.C5CodeId
                          join ct in TkmsDbContext.CardTypes on i.CardTypeId equals ct.CardTypeId
                          join bt in TkmsDbContext.BranchTypes on i.BfilBranchTypeId equals bt.BranchTypeId
                          join ist in TkmsDbContext.IndentStatuses on i.IndentStatusId equals ist.IndentStatusId
                          join rr in TkmsDbContext.RejectionReasons on i.RejectionReasonId equals rr.RejectionReasonId into rrs
                          from rr in rrs.DefaultIfEmpty()
                          select new IndentDetailModel
                          {
                              IndentId = i.IndentId,
                              IndentStatusId = i.IndentStatusId,
                              BfilBranchTypeId = i.BfilBranchTypeId,
                              BfilBranchId = i.BfilBranchId,
                              IndentStatus = ist.StatusName,
                              IndentDate = i.IndentDate,
                              IndentNo = i.IndentNo,
                              IblBranchCode = ib != null ? ib.IblBranchCode : "",
                              IblBranchName = ib != null ? ib.IblBranchName : "",
                              SchemeCode = sc.SchemeCodeName,
                              C5Code = c5.C5CodeName,
                              CardType = ct.CardTypeName,
                              NoOfKit = i.NoOfKit,
                              BfilBranchType = bt.BranchTypeName,
                              BfilBranchCode = bfilb.BranchCode,
                              ContactName = i.ContactName,
                              ContactNo = i.ContactNo,
                              IfscCode = bfilb.IfscCode,
                              RejectedReason = rr == null ? "" : rr.Reason,
                              Remarks = i.Remarks,
                              NoOfKitDispatched = (from d in TkmsDbContext.Dispatches
                                                   where d.IndentId == i.IndentId
                                                   select d.DispatchQty).Sum(),
                              DispatchAddress = (from a in TkmsDbContext.Addresses
                                                 where a.AddressId == i.DispatchAddressId
                                                 select a).FirstOrDefault(),
                          }).FirstOrDefaultAsync();
        }

        public async Task<List<IndentDetailModel>> GetIndentDetailByIndentNos(List<string> indentNos)
        {
            return await (from i in TkmsDbContext.Indents
                          where indentNos.Contains(i.IndentNo)
                          join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                          from ib in ibs.DefaultIfEmpty()
                          join bfilb in TkmsDbContext.Branches on i.BfilBranchId equals bfilb.BranchId
                          join sc in TkmsDbContext.SchemeCodes on i.SchemeCodeId equals sc.SchemeCodeId
                          join c5 in TkmsDbContext.C5Codes on i.C5CodeId equals c5.C5CodeId
                          join ct in TkmsDbContext.CardTypes on i.CardTypeId equals ct.CardTypeId
                          join bt in TkmsDbContext.BranchTypes on i.BfilBranchTypeId equals bt.BranchTypeId
                          select new IndentDetailModel
                          {
                              IndentId = i.IndentId,
                              IndentDate = i.IndentDate,
                              IndentNo = i.IndentNo,
                              BfilBranchId = i.BfilBranchId,
                              IblBranchCode = ib != null ? ib.IblBranchCode : "",
                              IblBranchName = ib != null ? ib.IblBranchName : "",
                              SchemeCode = sc.SchemeCodeName,
                              C5Code = c5.C5CodeName,
                              CardType = ct.CardTypeName,
                              NoOfKit = i.NoOfKit,
                              BfilBranchType = bt.BranchTypeName,
                              BfilBranchCode = bfilb.BranchCode,
                              Remarks = i.Remarks,
                              NoOfKitDispatched = (from d in TkmsDbContext.Dispatches
                                                   where d.IndentId == i.IndentId
                                                   select d.DispatchQty).Sum(),
                          }).ToListAsync();
        }
    }
}
