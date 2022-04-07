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
using TKMS.Abstraction.Models;
using TKMS.Repository.Contexts;
using TKMS.Repository.Interfaces;

namespace TKMS.Repository.Repositories
{
    public class IWorksKitRepository : Repository<IWorksKit>, IIWorksKitRepository
    {
        public IWorksKitRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<List<IWorksKit>> GetIWorksKitPending()
        {
            return await (from wk in TkmsDbContext.IWorksKits
                          where !wk.IsSuccess
                          select wk).ToListAsync();
        }
    }
}
