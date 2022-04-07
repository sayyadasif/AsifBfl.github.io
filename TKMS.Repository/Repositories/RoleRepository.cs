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
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetRolePaged(Pagination pagination)
        {
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;

            IRepository<RoleModel> repositoryRoleModel = new Repository<RoleModel>(TkmsDbContext);
            var query = (from us in TkmsDbContext.Roles
                         where us.IsDeleted == false &&
                         (!isActive.HasValue || isActive.Value == us.IsActive)
                         select new RoleModel()
                         {
                             IsActive = us.IsActive,
                         });

            return await repositoryRoleModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetDropdwon(long? id = null, long? roleTypeId = null)
        {
            Repository<DropdownModel> repositoryDropdownModel = new(TkmsDbContext);
            var query = (from r in TkmsDbContext.Roles
                         where !r.IsDeleted &&
                         (!id.HasValue || r.RoleId == id) &&
                         (!roleTypeId.HasValue || r.RoleTypeId == roleTypeId)
                         select new DropdownModel()
                         {
                             Id = r.RoleId,
                             Text = r.RoleName,
                             IsActive = r.IsActive
                         });

            return await repositoryDropdownModel.GetPagedReponseAsync(new Pagination { SortOrderColumn = "Text" }, null, query);
        }
    }
}
