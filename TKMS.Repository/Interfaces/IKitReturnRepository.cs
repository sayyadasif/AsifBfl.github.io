using Core.Repository;
using Core.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.Models;

namespace TKMS.Repository.Interfaces
{
    public interface IKitReturnRepository : IRepository<KitReturn>
    {
        Task<PagedList> GetKitReturnPaged(Pagination pagination);
    }
}
