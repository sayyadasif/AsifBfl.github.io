using Core.Repository;
using Core.Repository.Models;
using Core.Utility.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Models;
using TKMS.Repository.Contexts;
using TKMS.Repository.Interfaces;

namespace TKMS.Repository.Repositories
{
    public class DispatchWayBillRepository : Repository<DispatchWayBill>, IDispatchWayBillRepository
    {
        public DispatchWayBillRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetDispatchWayBillPaged(Pagination pagination)
        {
            string dispatchIds = pagination.Filters?.dispatchIds;

            var _dispatchIds = GetSplitValues(dispatchIds);

            IRepository<DispatchWayBillModel> repositoryDispatchWayBillModel = new Repository<DispatchWayBillModel>(TkmsDbContext);
            var query = (from dw in TkmsDbContext.DispatchWayBills
                         where !dw.IsDeleted &&
                         (!_dispatchIds.Any() || _dispatchIds.Contains(dw.DispatchId))
                         join ds in TkmsDbContext.DispatchStatuses on dw.DispatchStatusId equals ds.DispatchStatusId
                         join cs in TkmsDbContext.CourierStatuses on dw.CourierStatusId equals cs.CourierStatusId
                         join csr in TkmsDbContext.CourierStatuses on dw.ReceiveStatusId equals csr.CourierStatusId into csrs
                         from csr in csrs.DefaultIfEmpty()
                         select new DispatchWayBillModel
                         {
                             DispatchWayBillId = dw.DispatchWayBillId,
                             DispatchId = dw.DispatchId,
                             WayBillNo = dw.WayBillNo,
                             DispatchDate = dw.DispatchDate,
                             DeliveryDate = dw.DeliveryDate,
                             CourierStatus = cs.StatusName,
                             ReceiveBy = dw.ReceiveBy,
                             ReceiveStatus = csr == null ? "" : csr.StatusName,
                             DispatchStatus = ds.StatusName,
                             CourierPartner = dw.CourierPartner,
                             Remarks = dw.Remarks,
                         });

            return await repositoryDispatchWayBillModel.GetPagedReponseAsync(pagination, null, query);
        }
    }
}
