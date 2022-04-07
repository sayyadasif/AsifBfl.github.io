using Core.Repository.Models;
using Core.Utility.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TKMS.Abstraction.Enums;
using TKMS.Service.Interfaces;

namespace TKMS.Service.Services
{
    public class UserProviderService : IUserProviderService
    {
        private readonly IHttpContextAccessor _context;

        public UserProviderService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public UserClaim UserClaim { get => GetUserClaims(); }

        public bool BCM()
        {
            return UserClaim.RoleIds.Contains(Roles.BCM.GetHashCode());
        }

        public bool BM()
        {
            return UserClaim.RoleIds.Contains(Roles.BM.GetHashCode());
        }

        public bool HOAdmin()
        {
            return UserClaim.RoleIds.Contains(Roles.HOAdmin.GetHashCode());
        }

        public bool HOApprover()
        {
            return UserClaim.RoleIds.Contains(Roles.HOApprover.GetHashCode());
        }

        public bool HOIndentMaker()
        {
            return UserClaim.RoleIds.Contains(Roles.HOIndentMaker.GetHashCode());
        }

        public bool IBLCPU()
        {
            return UserClaim.RoleIds.Contains(Roles.IBLCPU.GetHashCode());
        }

        public bool Staff()
        {
            return UserClaim.RoleIds.Contains(Roles.Staff.GetHashCode());
        }

        public bool ROIndentMaker()
        {
            return UserClaim.RoleIds.Contains(Roles.ROIndentMaker.GetHashCode());
        }

        public bool ROKitManagement()
        {
            return UserClaim.RoleIds.Contains(Roles.ROKitManagement.GetHashCode());
        }

        public bool SystemAdmin()
        {
            return UserClaim.RoleIds.Contains(Roles.SystemAdmin.GetHashCode());
        }

        public bool HOUser()
        {
            return UserClaim.RoleTypeId == RoleTypes.HO.GetHashCode();
        }

        public bool ROUser()
        {
            return UserClaim.RoleTypeId == RoleTypes.RO.GetHashCode();
        }

        public bool SystemAdminUser()
        {
            return UserClaim.RoleTypeId == RoleTypes.SA.GetHashCode();
        }

        public bool StaffUser()
        {
            return UserClaim.RoleTypeId == RoleTypes.STAFF.GetHashCode();
        }

        public bool BranchUser()
        {
            return UserClaim.RoleTypeId == RoleTypes.BRANCH.GetHashCode();
        }

        public bool CpuUser()
        {
            return UserClaim.RoleTypeId == RoleTypes.CPU.GetHashCode();
        }

        public bool AllowIndentCreate()
        {
            return HOIndentMaker() || ROIndentMaker();
        }

        public long? GetRegionId()
        {
#if DEBUG
            return null;
#else
            return HOApprover() || HOIndentMaker() || IBLCPU() ? null : UserClaim.RegionId;
#endif
        }

        public long? GetBranchId()
        {
#if DEBUG
            return null;
#else
            return HOApprover() || HOIndentMaker() || IBLCPU() || ROIndentMaker() || ROKitManagement() ? null : UserClaim.BranchId;
#endif
        }

        public string GetRoleTypeName()
        {
            return Enum.GetName(typeof(RoleTypes), UserClaim.RoleTypeId);
        }

        public string GetRoleName()
        {
            var roles = new List<string>();
            foreach (var role in UserClaim.RoleIds)
            {
                roles.Add(Enum.Parse(typeof(Roles), role.ToString()).DescriptionAttr());
            }
            return string.Join(",", roles);
        }

        private UserClaim GetUserClaims()
        {
            var userClaims = new UserClaim();

            var name = _context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Name);
            if (name != null) { userClaims.Name = name.Value; }

            var userId = _context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "UserId");
            if (userId != null && !string.IsNullOrEmpty(userId.Value)) { userClaims.UserId = Convert.ToInt64(userId.Value); }

            var branchId = _context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "BranchId");
            if (branchId != null && !string.IsNullOrEmpty(branchId.Value)) { userClaims.BranchId = Convert.ToInt64(branchId.Value); }

            var branchCode = _context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "BranchCode");
            if (name != null) { userClaims.BranchCode = branchCode.Value; }

            var roleTypeId = _context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "RoleTypeId");
            if (roleTypeId != null && !string.IsNullOrEmpty(roleTypeId.Value)) { userClaims.RoleTypeId = Convert.ToInt64(roleTypeId.Value); }

            var regionId = _context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "RegionId");
            if (regionId != null && !string.IsNullOrEmpty(regionId.Value)) { userClaims.RegionId = Convert.ToInt64(regionId.Value); }

            var roleIds = _context.HttpContext.User.Claims.FirstOrDefault(i => i.Type == "RoleIds");
            if (roleIds != null && !string.IsNullOrEmpty(roleIds.Value)) { userClaims.RoleIds = roleIds.Value.Split(',').Select(long.Parse).ToList(); }

            return userClaims;
        }
    }
}
