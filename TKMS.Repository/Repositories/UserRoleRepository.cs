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
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetUserRolePaged(Pagination pagination)
        {
            long? userId = IsPropertyExist(pagination.Filters, "userId") ? pagination.Filters?.userId : null;

            IRepository<UserRoleModel> repositoryUserRoleModel = new Repository<UserRoleModel>(TkmsDbContext);
            var query = (from us in TkmsDbContext.UserRoles
                         where (!userId.HasValue || userId.Value == us.UserId)
                         select new UserRoleModel()
                         {
                             UserRoleId = us.UserRoleId,
                             UserId = us.UserId,
                             RoleId = us.RoleId,
                         });

            return await repositoryUserRoleModel.GetPagedReponseAsync(pagination, null, query);
        }
    }
}
