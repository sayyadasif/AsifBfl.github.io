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
    public class RoleTypeRepository : Repository<RoleType>, IRoleTypeRepository
    {
        public RoleTypeRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetRoleTypePaged(Pagination pagination)
        {
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;

            IRepository<RoleTypeModel> repositoryRoleTypeModel = new Repository<RoleTypeModel>(TkmsDbContext);
            var query = (from us in TkmsDbContext.RoleTypes
                         where us.IsDeleted == false &&
                         (!isActive.HasValue || isActive.Value == us.IsActive)
                         select new RoleTypeModel()
                         {
                             IsActive = us.IsActive,
                         });

            return await repositoryRoleTypeModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetDropdwon(long? id = null, bool? isActive = null)
        {
            Repository<DropdownModel> repositoryDropdownModel = new(TkmsDbContext);
            var query = (from r in TkmsDbContext.RoleTypes
                         where !r.IsDeleted &&
                         (!id.HasValue || r.RoleTypeId == id) &&
                         (!isActive.HasValue || r.IsActive == isActive)
                         select new DropdownModel()
                         {
                             Id = r.RoleTypeId,
                             Text = r.RoleTypeName,
                             IsActive = r.IsActive
                         });

            return await repositoryDropdownModel.GetPagedReponseAsync(new Pagination { SortOrderColumn = "Text" }, null, query);
        }
    }
}
