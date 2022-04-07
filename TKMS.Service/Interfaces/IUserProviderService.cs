using Core.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TKMS.Service.Interfaces
{
    public interface IUserProviderService
    {
        UserClaim UserClaim { get; }
        bool HOAdmin();
        bool HOIndentMaker();
        bool HOApprover();
        bool ROIndentMaker();
        bool ROKitManagement();
        bool BM();
        bool BCM();
        bool IBLCPU();
        bool Staff();
        bool SystemAdmin();
        bool HOUser();
        bool ROUser();
        bool BranchUser();
        bool CpuUser();
        bool StaffUser();
        bool SystemAdminUser();
        bool AllowIndentCreate();
        long? GetRegionId();
        long? GetBranchId();
        string GetRoleTypeName();
        string GetRoleName();
    }
}
