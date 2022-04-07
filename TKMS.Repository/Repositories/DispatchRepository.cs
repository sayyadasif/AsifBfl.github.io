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
    public class DispatchRepository : Repository<Dispatch>, IDispatchRepository
    {
        private readonly IDispatchWayBillRepository _dispatchWayBillRepository;

        public DispatchRepository(TkmsDbContext context, IDispatchWayBillRepository dispatchWayBillRepository)
            : base(context)
        {
            _dispatchWayBillRepository = dispatchWayBillRepository;
        }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetDispatchPaged(Pagination pagination)
        {
            long? indentId = IsPropertyExist(pagination.Filters, "indentId") ? pagination.Filters?.indentId : null;
            long? dispatchId = IsPropertyExist(pagination.Filters, "dispatchId") ? pagination.Filters?.dispatchId : null;

            IRepository<DispatchModel> repositoryDispatchModel = new Repository<DispatchModel>(TkmsDbContext);
            var query = (from d in TkmsDbContext.Dispatches
                         where !d.IsDeleted &&
                         (!indentId.HasValue || indentId.Value == d.IndentId) &&
                         (!dispatchId.HasValue || dispatchId.Value == d.DispatchId)
                         join ds in TkmsDbContext.DispatchStatuses on d.DispatchStatusId equals ds.DispatchStatusId
                         select new DispatchModel
                         {
                             DispatchId = d.DispatchId,
                             DispatchStatusId = d.DispatchStatusId,
                             IndentId = d.IndentId,
                             AccountStart = d.AccountStart,
                             AccountEnd = d.AccountEnd,
                             Vendor = d.Vendor,
                             DispatchQty = d.DispatchQty,
                             DispatchDate = d.DispatchDate,
                             SchemeType = d.SchemeType,
                             ReferenceNo = d.ReferenceNo,
                             DispatchStatus = ds.StatusName,
                             Remarks = d.Remarks
                         });

            var dispatchResult = await repositoryDispatchModel.GetPagedReponseAsync(pagination, null, query);

            var dispatches = dispatchResult.Data as List<DispatchModel>;

            if (dispatches.Any())
            {
                dynamic filters = new ExpandoObject();
                filters.dispatchIds = string.Join(",", dispatches.Select(d => d.DispatchId));

                var wayBillResult = await _dispatchWayBillRepository.GetDispatchWayBillPaged(new Pagination { Filters = filters });
                var wayBills = wayBillResult.Data as List<DispatchWayBillModel>;

                dispatches.ForEach(d => d.DispatchWayBills = wayBills.Where(w => w.DispatchId == d.DispatchId).ToList());
            }

            return new PagedList { Data = dispatches, TotalCount = dispatches.Count() };
        }

        public async Task<bool> IsAllDispatchReceived(long indentId)
        {
            var indent = await (from i in TkmsDbContext.Indents
                                   where indentId == i.IndentId
                                   select new
                                   {
                                       NoOfKit = i.NoOfKit,
                                       NoOfKitDispatched = (from d in TkmsDbContext.Dispatches
                                                            where d.IndentId == i.IndentId
                                                            select d.DispatchQty).Sum(),
                                   }).FirstOrDefaultAsync();

            if (indent.NoOfKit == indent.NoOfKitDispatched)
            {
                return !(await TkmsDbContext.Dispatches.AnyAsync(d => d.IndentId == indentId &&
                                                                      d.DispatchStatusId != DispatchStatuses.Received.GetHashCode()));
            }
            return false;
        }
    }
}
