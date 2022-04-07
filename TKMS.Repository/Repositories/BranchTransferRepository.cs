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
    public class BranchTransferRepository : Repository<BranchTransfer>, IBranchTransferRepository
    {
        public BranchTransferRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetBranchTransferPaged(Pagination pagination)
        {
            string indentNo = IsPropertyExist(pagination.Filters, "indentNo") ? pagination.Filters?.indentNo : null;
            string accountNo = IsPropertyExist(pagination.Filters, "accountNo") ? pagination.Filters?.accountNo : null;
            string cifNo = IsPropertyExist(pagination.Filters, "cifNo") ? pagination.Filters?.cifNo : null;
            long? branchId = IsPropertyExist(pagination.Filters, "branchId") ? pagination.Filters?.branchId : null;
            long? cardTypeId = IsPropertyExist(pagination.Filters, "cardTypeId") ? pagination.Filters?.cardTypeId : null;
            long? bfilBranchId = IsPropertyExist(pagination.Filters, "bfilBranchId") ? pagination.Filters?.bfilBranchId : null;
            string transferDate = IsPropertyExist(pagination.Filters, "transferDate") ? pagination.Filters?.transferDate : null;
            string receivedDate = IsPropertyExist(pagination.Filters, "receivedDate") ? pagination.Filters?.receivedDate : null;
            bool? isSent = IsPropertyExist(pagination.Filters, "isSent") ? pagination.Filters?.isSent : null;

            DateTime? _transferDate = null;
            if (!string.IsNullOrEmpty(transferDate))
            {
                _transferDate = CommonUtils.GetParseDate(transferDate);
            }

            DateTime? _receivedDate = null;
            if (!string.IsNullOrEmpty(receivedDate))
            {
                _receivedDate = CommonUtils.GetParseDate(receivedDate);
            }

#if DEBUG
            bfilBranchId = null;
#endif

            IRepository<BranchTransferModel> repositoryBranchTransferModel = new Repository<BranchTransferModel>(TkmsDbContext);
            var query = (from bt in TkmsDbContext.BranchTransfers
                         join k in TkmsDbContext.Kits on bt.KitId equals k.KitId
                         join i in TkmsDbContext.Indents on k.IndentId equals i.IndentId
                         join ib in TkmsDbContext.Branches on bt.FromBranchId equals ib.BranchId
                         join bfilb in TkmsDbContext.Branches on bt.ToBranchId equals bfilb.BranchId
                         join ct in TkmsDbContext.CardTypes on i.CardTypeId equals ct.CardTypeId
                         where !k.IsDeleted
                         && (string.IsNullOrEmpty(indentNo) || indentNo == i.IndentNo)
                         && (string.IsNullOrEmpty(accountNo) || accountNo == k.AccountNo)
                         && (string.IsNullOrEmpty(cifNo) || cifNo == k.CifNo)
                         && (!branchId.HasValue || branchId.Value == bt.FromBranchId)
                         && (!cardTypeId.HasValue || cardTypeId.Value == i.CardTypeId)
                         && (!bfilBranchId.HasValue || bfilBranchId.Value == bt.ToBranchId)
                         && (!_transferDate.HasValue || _transferDate.Value.Date == bt.TransferDate.Date)
                         && (!_receivedDate.HasValue || _receivedDate.Value.Date == bt.ReceivedDate.Value.Date)
                         && (bt.ReceivedDate.HasValue == (isSent.HasValue && !isSent.Value))
                         select new BranchTransferModel
                         {
                             BranchTransferId = bt.BranchTransferId,
                             KitId = k.KitId,
                             IndentNo = i.IndentNo,
                             AccountNo = k.AccountNo,
                             CifNo = k.CifNo,
                             FromBranchCode = ib.BranchCode,
                             FromBranchName = ib.BranchName,
                             ToBranchCode = bfilb.BranchCode,
                             CardType = ct.CardTypeName,
                             TransferDate = bt.TransferDate,
                             ReceivedDate = bt.ReceivedDate,
                         });

            return await repositoryBranchTransferModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<BranchTransferModel> GetTransferDetailById(long id)
        {
            return await (from bt in TkmsDbContext.BranchTransfers
                          join k in TkmsDbContext.Kits on bt.KitId equals k.KitId
                          join i in TkmsDbContext.Indents on k.IndentId equals i.IndentId
                          join ib in TkmsDbContext.Branches on bt.FromBranchId equals ib.BranchId
                          join bfilb in TkmsDbContext.Branches on bt.ToBranchId equals bfilb.BranchId
                          join ct in TkmsDbContext.CardTypes on i.CardTypeId equals ct.CardTypeId
                          join tu in TkmsDbContext.Users on bt.TransferById equals tu.UserId into tus
                          from tu in tus.DefaultIfEmpty()
                          join ru in TkmsDbContext.Users on bt.ReceivedById equals ru.UserId into rus
                          from ru in rus.DefaultIfEmpty()
                          where bt.BranchTransferId == id
                          select new BranchTransferModel
                          {
                              BranchTransferId = bt.BranchTransferId,
                              KitId = k.KitId,
                              IndentNo = i.IndentNo,
                              AccountNo = k.AccountNo,
                              CifNo = k.CifNo,
                              FromBranchCode = ib.BranchCode,
                              FromBranchName = ib.BranchName,
                              ToBranchCode = bfilb.BranchCode,
                              CardType = ct.CardTypeName,
                              TransferByStaffId = tu == null ? "" : tu.StaffId,
                              TransferByName = tu == null ? "" : tu.FullName,
                              TransferDate = bt.TransferDate,
                              ReceivedByStaffId = ru == null ? "" : ru.StaffId,
                              ReceivedByName = ru == null ? "" : ru.FullName,
                              ReceivedDate = bt.ReceivedDate,
                              Remarks = bt.Remarks,
                          }).FirstOrDefaultAsync();
        }

        public async Task<TransferKitDetailModel> GetTransferKitByAccountNo(dynamic filters = null)
        {
            string accountNo = IsPropertyExist(filters, "accountNo") ? filters?.accountNo : null;
            long? bfilBranchId = IsPropertyExist(filters, "bfilBranchId") ? filters?.bfilBranchId : null;

#if DEBUG
            bfilBranchId = null;
#endif

            return await (from bt in TkmsDbContext.BranchTransfers
                          join k in TkmsDbContext.Kits on bt.KitId equals k.KitId
                          join i in TkmsDbContext.Indents on k.IndentId equals i.IndentId
                          join ib in TkmsDbContext.Branches on bt.FromBranchId equals ib.BranchId
                          join bfilb in TkmsDbContext.Branches on bt.ToBranchId equals bfilb.BranchId
                          where !k.IsDeleted
                          && !bt.ReceivedById.HasValue
                          && accountNo == k.AccountNo
                          && (!bfilBranchId.HasValue || bfilBranchId.Value == bt.ToBranchId)
                          select new TransferKitDetailModel
                          {
                              BranchTransferId = bt.BranchTransferId,
                              TransferDate = bt.TransferDate,
                              IndentNo = i.IndentNo,
                              AccountNo = k.AccountNo,
                              BranchCode = ib.BranchCode,
                              BranchName = ib.BranchName,
                              BfilBranchCode = bfilb.BranchCode,
                              CifNo = k.CifNo,
                          }).FirstOrDefaultAsync();
        }

    }
}
