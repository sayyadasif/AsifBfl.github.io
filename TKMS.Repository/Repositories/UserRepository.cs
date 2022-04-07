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
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(TkmsDbContext context)
            : base(context)
        { }

        private TkmsDbContext TkmsDbContext
        {
            get { return _dbContext as TkmsDbContext; }
        }

        public async Task<PagedList> GetUserPaged(Pagination pagination)
        {
            string fullName = IsPropertyExist(pagination.Filters, "fullName") ? pagination.Filters?.fullName : null;
            string staffId = IsPropertyExist(pagination.Filters, "staffId") ? pagination.Filters?.staffId : null;
            string roleIds = IsPropertyExist(pagination.Filters, "roleIds") ? pagination.Filters?.roleIds : null;
            string branchIds = IsPropertyExist(pagination.Filters, "branchIds") ? pagination.Filters?.branchIds : null;
            bool? isActive = IsPropertyExist(pagination.Filters, "isActive") ? pagination.Filters?.isActive : null;

            var _roleIds = GetSplitValues(roleIds);

            var _branchIds = GetSplitValues(branchIds);

            IRepository<UserModel> repositoryUserModel = new Repository<UserModel>(TkmsDbContext);
            var query = (from u in TkmsDbContext.Users
                         .Include(ur => ur.UserRoles)
                         where !u.IsDeleted &&
                         u.StaffId != "Admin" && // Skip System Admin to List Anywhere
                         ((string.IsNullOrEmpty(fullName) || u.FullName.Contains(fullName)) ||
                         (string.IsNullOrEmpty(staffId) || u.StaffId.Contains(staffId))) &&
                         (!isActive.HasValue || isActive.Value == u.IsActive) &&
                         (!_branchIds.Any() || _branchIds.Contains(u.BranchId)) &&
                         (!_roleIds.Any() || u.UserRoles.Any(r => _roleIds.Contains(r.RoleId)))
                         join b in TkmsDbContext.Branches on u.BranchId equals b.BranchId into bs
                         from b in bs.DefaultIfEmpty()
                         select new UserModel()
                         {
                             UserId = u.UserId,
                             FullName = u.FullName,
                             StaffId = u.StaffId,
                             MobileNo = u.MobileNo,
                             IsActive = u.IsActive,
                             BranchName = b == null ? "" : b.BranchName,
                             Roles = (from r in TkmsDbContext.Roles
                                      join ur in TkmsDbContext.UserRoles on r.RoleId equals ur.RoleId
                                      where ur.UserId == u.UserId
                                      select r.RoleName).AsEnumerable(),
                         });

            return await repositoryUserModel.GetPagedReponseAsync(pagination, null, query);
        }

        public async Task<PagedList> GetDropdwon(long? id = null, long? branchId = null, long? roleTypeId = null, bool? isActive = null)
        {
            Repository<DropdownModel> repositoryDropdownModel = new(TkmsDbContext);
            var query = (from u in TkmsDbContext.Users
                         where !u.IsDeleted &&
                         (!id.HasValue || u.UserId == id) &&
                         (!branchId.HasValue || u.BranchId == branchId) &&
                         (!roleTypeId.HasValue || u.RoleTypeId == roleTypeId) &&
                         (!isActive.HasValue || u.IsActive == isActive)
                         select new DropdownModel()
                         {
                             Id = u.UserId,
                             Text = u.StaffId,
                             IsActive = u.IsActive
                         });

            return await repositoryDropdownModel.GetPagedReponseAsync(new Pagination { SortOrderColumn = "Text" }, null, query);
        }
    }
}
