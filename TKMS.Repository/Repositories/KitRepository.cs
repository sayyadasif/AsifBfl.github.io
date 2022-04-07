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
    public class KitRepository : Repository<Kit>, IKitRepository
    {
        public KitRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetKitPaged(Pagination pagination)
        {
            string kitStatusIds = IsPropertyExist(pagination.Filters, "kitStatusIds") ? pagination.Filters?.kitStatusIds : null;
            string indentNo = IsPropertyExist(pagination.Filters, "indentNo") ? pagination.Filters?.indentNo : null;
            string accountNo = IsPropertyExist(pagination.Filters, "accountNo") ? pagination.Filters?.accountNo : null;
            string cifNo = IsPropertyExist(pagination.Filters, "cifNo") ? pagination.Filters?.cifNo : null;
            long? branchId = IsPropertyExist(pagination.Filters, "branchId") ? pagination.Filters?.branchId : null;
            long? cardTypeId = IsPropertyExist(pagination.Filters, "cardTypeId") ? pagination.Filters?.cardTypeId : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;
            long? allocatedToId = IsPropertyExist(pagination.Filters, "allocatedToId") ? pagination.Filters?.allocatedToId : null;
            long? kitDamageReasonId = IsPropertyExist(pagination.Filters, "kitDamageReasonId") ? pagination.Filters?.kitDamageReasonId : null;
            string allocatedDate = IsPropertyExist(pagination.Filters, "allocatedDate") ? pagination.Filters?.allocatedDate : null;
            string assignedDate = IsPropertyExist(pagination.Filters, "assignedDate") ? pagination.Filters?.assignedDate : null;
            string kitDate = IsPropertyExist(pagination.Filters, "kitDate") ? pagination.Filters?.kitDate : null;

            bool? isDestructionApproved = IsPropertyExist(pagination.Filters, "isDestructionApproved") ? pagination.Filters?.isDestructionApproved : null;

            var _kitStatusIds = GetSplitValues(kitStatusIds);

            DateTime? _allocatedDate = null;
            if (!string.IsNullOrEmpty(allocatedDate))
            {
                _allocatedDate = CommonUtils.GetParseDate(allocatedDate);
            }

            DateTime? _assignedDate = null;
            if (!string.IsNullOrEmpty(assignedDate))
            {
                _assignedDate = CommonUtils.GetParseDate(assignedDate);
            }

            DateTime? _kitDate = null;
            if (!string.IsNullOrEmpty(kitDate))
            {
                _kitDate = CommonUtils.GetParseDate(kitDate);
            }

#if DEBUG
            bfilBranchId = null;
#endif

            IRepository<KitModel> repositoryKitModel = new Repository<KitModel>(TkmsDbContext);
            var query = (from k in TkmsDbContext.Kits
                         join ks in TkmsDbContext.KitStatuses on k.KitStatusId equals ks.KitStatusId
                         join i in TkmsDbContext.Indents on k.IndentId equals i.IndentId
                         join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                         from ib in ibs.DefaultIfEmpty()
                         join bfilb in TkmsDbContext.Branches on k.BranchId equals bfilb.BranchId
                         join ct in TkmsDbContext.CardTypes on i.CardTypeId equals ct.CardTypeId
                         join u in TkmsDbContext.Users on k.AllocatedToId equals u.UserId into us
                         from u in us.DefaultIfEmpty()
                         join kdr in TkmsDbContext.KitDamageReasons on k.KitDamageReasonId equals kdr.KitDamageReasonId into kdrs
                         from kdr in kdrs.DefaultIfEmpty()
                         where !k.IsDeleted
                         && !_kitStatusIds.Any() || _kitStatusIds.Contains(k.KitStatusId)
                         && (string.IsNullOrEmpty(indentNo) || indentNo == i.IndentNo)
                         && (string.IsNullOrEmpty(accountNo) || accountNo == k.AccountNo)
                         && (string.IsNullOrEmpty(cifNo) || cifNo == k.CifNo)
                         && (!branchId.HasValue || branchId.Value == i.IblBranchId)
                         && (!cardTypeId.HasValue || cardTypeId.Value == i.CardTypeId)
                         && (!bfilBranchId.HasValue || bfilBranchId.Value == k.BranchId)
                         && (!allocatedToId.HasValue || allocatedToId.Value == k.AllocatedToId)
                         && (!_allocatedDate.HasValue || _allocatedDate.Value.Date == k.AllocatedDate.Value.Date)
                         && (!kitDamageReasonId.HasValue || kitDamageReasonId.Value == k.KitDamageReasonId)
                         && (!_assignedDate.HasValue || _assignedDate.Value.Date == k.AssignedDate.Value.Date)
                         && (!isDestructionApproved.HasValue || isDestructionApproved.Value == k.IsDestructionApproved)
                         && (!_kitDate.HasValue || _kitDate.Value.Date == k.CreatedDate.Date)
                         select new KitModel
                         {
                             KitId = k.KitId,
                             KitStatusId = k.KitStatusId,
                             KitStatus = ks.StatusName,
                             IndentNo = i.IndentNo,
                             AccountNo = k.AccountNo,
                             CifNo = k.CifNo,
                             IblBranchCode = ib != null ? ib.IblBranchCode : "",
                             IblBranchName = ib != null ? ib.IblBranchName : "",
                             BfilBranchCode = bfilb.BranchCode,
                             CardType = ct.CardTypeName,
                             StaffId = u == null ? "" : u.StaffId,
                             StaffName = u == null ? "" : u.FullName,
                             AllocatedDate = k.AllocatedDate,
                             KitDamageReasonId = k.KitDamageReasonId,
                             KitDamageReason = kdr == null ? "" : kdr.Reason,
                             DamageRemarks = k.DamageRemarks,
                             IsDestructionApproved = k.IsDestructionApproved,
                             DestructionRemarks = k.DestructionRemarks,
                             CustomerName = k.CustomerName,
                             Remarks = k.Remarks,
                             KitDate = k.CreatedDate,
                         });

            return await repositoryKitModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetKitStaffPaged(Pagination pagination)
        {
            string kitStatusIds = IsPropertyExist(pagination.Filters, "kitStatusIds") ? pagination.Filters?.kitStatusIds : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;
            long? allocatedToId = IsPropertyExist(pagination.Filters, "allocatedToId") ? pagination.Filters?.allocatedToId : null;

            var _kitStatusIds = GetSplitValues(kitStatusIds);

#if DEBUG
            bfilBranchId = null;
#endif

            IRepository<KitStaffModel> repositoryKitModel = new Repository<KitStaffModel>(TkmsDbContext);
            var query = (from k in TkmsDbContext.Kits
                         join i in TkmsDbContext.Indents on k.IndentId equals i.IndentId
                         join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                         from ib in ibs.DefaultIfEmpty()
                         join u in TkmsDbContext.Users on k.AllocatedToId equals u.UserId
                         where !k.IsDeleted
                         && k.AllocatedById.HasValue
                         && !_kitStatusIds.Any() || _kitStatusIds.Contains(k.KitStatusId)
                         && (!bfilBranchId.HasValue || bfilBranchId.Value == k.BranchId)
                         && (!allocatedToId.HasValue || allocatedToId.Value == k.AllocatedToId)
                         group new { k, u } by new { u.UserId, u.StaffId, u.FullName } into staffs
                         select new KitStaffModel
                         {
                             UserId = staffs.Key.UserId,
                             StaffId = staffs.Key.StaffId,
                             StaffName = staffs.Key.FullName,
                             KitAllocated = staffs.Count(),
                             KitAssigned = staffs.Count(k => k.k.KitStatusId == KitStatuses.Assigned.GetHashCode()),
                         });

            return await repositoryKitModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<List<KitModel>> GetStaffKits(List<long> kitIds)
        {
            return await (from k in TkmsDbContext.Kits
                          where kitIds.Contains(k.KitId)
                          select new KitModel
                          {
                              AccountNo = k.AccountNo,
                              CifNo = k.CifNo,
                              AllocatedDate = k.AllocatedDate,
                          }).ToListAsync();
        }

        public async Task<List<string>> ValidateKitAccountNo(List<string> accountNos)
        {
            return await (from k in TkmsDbContext.Kits
                          where accountNos.Contains(k.AccountNo)
                          select k.AccountNo).ToListAsync();
        }

        public async Task<KitModel> GetKitDetailById(long id)
        {
            return await (from k in TkmsDbContext.Kits
                          join ks in TkmsDbContext.KitStatuses on k.KitStatusId equals ks.KitStatusId
                          join i in TkmsDbContext.Indents on k.IndentId equals i.IndentId
                          join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                          from ib in ibs.DefaultIfEmpty()
                          join bfilb in TkmsDbContext.Branches on k.BranchId equals bfilb.BranchId
                          join ct in TkmsDbContext.CardTypes on i.CardTypeId equals ct.CardTypeId
                          join u in TkmsDbContext.Users on k.AllocatedToId equals u.UserId into us
                          from u in us.DefaultIfEmpty()
                          join kdr in TkmsDbContext.KitDamageReasons on k.KitDamageReasonId equals kdr.KitDamageReasonId into kdrs
                          from kdr in kdrs.DefaultIfEmpty()
                          where k.KitId == id
                          select new KitModel
                          {
                              KitId = k.KitId,
                              KitStatusId = k.KitStatusId,
                              KitStatus = ks.StatusName,
                              IndentNo = i.IndentNo,
                              AccountNo = k.AccountNo,
                              CifNo = k.CifNo,
                              IblBranchCode = ib != null ? ib.IblBranchCode : "",
                              IblBranchName = ib != null ? ib.IblBranchName : "",
                              BfilBranchCode = bfilb.BranchCode,
                              CardType = ct.CardTypeName,
                              StaffId = u == null ? "" : u.StaffId,
                              StaffName = u == null ? "" : u.FullName,
                              AllocatedDate = k.AllocatedDate,
                              KitDamageReasonId = k.KitDamageReasonId,
                              KitDamageReason = kdr == null ? "" : kdr.Reason,
                              DamageRemarks = k.DamageRemarks,
                              CustomerName = k.CustomerName,
                              Remarks = k.Remarks,
                              KitDate = k.CreatedDate,
                          }).FirstOrDefaultAsync();

        }

        public async Task<KitDetailModel> GetKitByAccountNo(dynamic filters = null)
        {
            string accountNo = IsPropertyExist(filters, "accountNo") ? filters?.accountNo : null;
            long? kitStatusId = IsPropertyExist(filters, "kitStatusId") ? filters?.kitStatusId : null;
            long? bfilBranchId = IsPropertyExist(filters, "bfilBranchId") ? filters?.bfilBranchId : null;
            long? dispatchStatusId = IsPropertyExist(filters, "dispatchStatusId") ? filters?.dispatchStatusId : null;

#if DEBUG
            bfilBranchId = null;
#endif

            return await (from k in TkmsDbContext.Kits
                          join d in TkmsDbContext.Dispatches on k.DispatchId equals d.DispatchId
                          join i in TkmsDbContext.Indents on k.IndentId equals i.IndentId
                          join ib in TkmsDbContext.IblBranches on i.IblBranchId equals ib.IblBranchId into ibs
                          from ib in ibs.DefaultIfEmpty()
                          join bfilb in TkmsDbContext.Branches on k.BranchId equals bfilb.BranchId
                          where !k.IsDeleted
                          && accountNo == k.AccountNo
                          && (!kitStatusId.HasValue || kitStatusId.Value == k.KitStatusId)
                          && (!bfilBranchId.HasValue || bfilBranchId.Value == k.BranchId)
                          && (!dispatchStatusId.HasValue || dispatchStatusId.Value == d.DispatchStatusId)
                          select new KitDetailModel
                          {
                              KitId = k.KitId,
                              KitDate = k.CreatedDate,
                              IndentNo = i.IndentNo,
                              AccountNo = k.AccountNo,
                              BranchCode = ib != null ? ib.IblBranchCode : "",
                              BranchName = ib != null ? ib.IblBranchName : "",
                              BfilBranchCode = bfilb.BranchCode,
                              CifNo = k.CifNo,
                          }).FirstOrDefaultAsync();
        }

        public async Task<KitStaffDetailModel> KitStaffDetailByAllocatedId(long userId)
        {
            return await (from u in TkmsDbContext.Users
                          where u.UserId == userId
                          select new KitStaffDetailModel
                          {
                              StaffId = u.StaffId,
                              StaffName = u.FullName,
                              KitAllocated = (from k in TkmsDbContext.Kits
                                              where k.AllocatedToId == userId
                                              && k.KitStatusId == KitStatuses.Allocated.GetHashCode()
                                              select new KitStaffDetail
                                              {
                                                  AccountNo = k.AccountNo,
                                                  CifNo = k.CifNo,
                                                  AllocatedDate = k.AllocatedDate.Value,
                                                  CustomerName = k.CustomerName,
                                              }).AsEnumerable(),
                              KitAssigned = (from k in TkmsDbContext.Kits
                                             where k.AllocatedToId == userId
                                             && k.KitStatusId == KitStatuses.Assigned.GetHashCode()
                                             select new KitStaffDetail
                                             {
                                                 AccountNo = k.AccountNo,
                                                 CifNo = k.CifNo,
                                                 AllocatedDate = k.AllocatedDate.Value,
                                                 AssignedDate = k.AssignedDate.Value,
                                                 CustomerName = k.CustomerName,
                                             }).AsEnumerable(),
                          }).FirstOrDefaultAsync();

        }

        public async Task<int> GetAllocatedKitCount(long staffId, long kitStatusId)
        {
            return await (from k in TkmsDbContext.Kits
                          where k.AllocatedToId == staffId
                          && k.KitStatusId == kitStatusId
                          select k).CountAsync();
        }

        public async Task<long> GetKitCount(dynamic filters = null)
        {
            string kitStatusIds = IsPropertyExist(filters, "kitStatusIds") ? filters?.kitStatusIds : null;
            string indentStartDate = IsPropertyExist(filters, "indentStartDate") ? filters?.indentStartDate : null;
            string indentEndDate = IsPropertyExist(filters, "indentEndDate") ? filters?.indentEndDate : null;
            long? branchId = IsPropertyExist(filters, "branchId") ? filters?.branchId : null;
            long? bfilBranchId = IsPropertyExist(filters, "bfilBranchId") ? filters?.bfilBranchId : null;
            long? branchTypeId = IsPropertyExist(filters, "branchTypeId") ? filters?.branchTypeId : null;

            var _kitStatusIds = GetSplitValues(kitStatusIds);

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

            return await (from k in TkmsDbContext.Kits
                          join indent in TkmsDbContext.Indents on k.IndentId equals indent.IndentId
                          join ib in TkmsDbContext.IblBranches on indent.IblBranchId equals ib.IblBranchId into ibs
                          from ib in ibs.DefaultIfEmpty()
                          join bfilb in TkmsDbContext.Branches on k.BranchId equals bfilb.BranchId
                          where !indent.IsDeleted
                          && (!_kitStatusIds.Any() || _kitStatusIds.Contains(k.KitStatusId))
                          && (!branchId.HasValue || branchId.Value == indent.IblBranchId)
                          && (!bfilBranchId.HasValue || bfilBranchId.Value == k.BranchId)
                          && (!branchTypeId.HasValue || branchTypeId.Value == indent.BfilBranchTypeId)
                          && (!_indentStartDate.HasValue || _indentStartDate.Value.Date <= indent.IndentDate.Date)
                          && (!_indentEndDate.HasValue || _indentEndDate.Value.Date >= indent.IndentDate.Date)
                          select k).CountAsync();
        }

        public async Task<bool> IsSingleStaff(List<long> kitIds)
        {
            var staffs = await (from k in TkmsDbContext.Kits
                                where kitIds.Contains(k.KitId)
                                select k.AllocatedToId).Distinct().ToListAsync();
            return staffs.Count == 1;
        }
    }
}
