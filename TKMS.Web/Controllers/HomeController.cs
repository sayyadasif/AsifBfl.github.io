using Core.Repository.Models;
using Core.Utility.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Constants;
using TKMS.Abstraction.Enums;
using TKMS.Service.Interfaces;
using TKMS.Web.Models;

namespace TKMS.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserProviderService _userProviderService;
        private readonly IIndentService _indentService;
        private readonly IKitService _kitService;
        private readonly INotificationService _notificationService;
        private readonly ISettingService _settingService;

        public HomeController(
           IUserProviderService userProviderService,
           IIndentService indentService,
           IKitService kitService,
           INotificationService notificationService,
           ISettingService settingService

        )
        {
            _userProviderService = userProviderService;
            _indentService = indentService;
            _kitService = kitService;
            _notificationService = notificationService;
            _settingService = settingService;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> GetDashboard(int monthId)
        {
            return PartialView("_Dashboard", await GetDashboardModel(monthId));
        }

        public async Task<List<StatusCount>> GetIndentStatusOverview(int monthId)
        {
            var dateRange = GetDateRange(monthId);

            dynamic filters = new ExpandoObject();
            filters.regionId = _userProviderService.GetRegionId();
            filters.bfilBranchId = _userProviderService.GetBranchId();
            filters.indentStartDate = dateRange.Item1;
            filters.indentEndDate = dateRange.Item2;

            var statusOverview = new List<StatusCount>();
            if (_userProviderService.ROIndentMaker() || _userProviderService.ROKitManagement() || _userProviderService.HOIndentMaker())
            {

                filters.indentStatusIds = $"{IndentStatuses.PendingApproval.GetHashCode()}";
                statusOverview.Add(new StatusCount
                {
                    StatusName = IndentStatuses.PendingApproval.DescriptionAttr(),
                    Count = await _indentService.GetIndentCount(filters),
                    Color = "#D8A61A",
                });

                filters.indentStatusIds = $"{IndentStatuses.Approved.GetHashCode()}";
                statusOverview.Add(new StatusCount
                {
                    StatusName = IndentStatuses.Approved.DescriptionAttr(),
                    Count = await _indentService.GetIndentCount(filters),
                    Color = "#2D5600",
                });

                filters.indentStatusIds = $"{IndentStatuses.Rejected.GetHashCode()}";
                statusOverview.Add(new StatusCount
                {
                    StatusName = IndentStatuses.Rejected.DescriptionAttr(),
                    Count = await _indentService.GetIndentCount(filters),
                    Color = "#98272A",
                });

                filters.indentStatusIds = $"{IndentStatuses.PartialDispatched.GetHashCode()},{IndentStatuses.Dispatched.GetHashCode()}";
                statusOverview.Add(new StatusCount
                {
                    StatusName = IndentStatuses.Dispatched.DescriptionAttr(),
                    Count = await _indentService.GetIndentCount(filters),
                    Color = "#C45819",
                });

                filters.indentStatusIds = $"{IndentStatuses.IndentBoxReceived.GetHashCode()}";
                statusOverview.Add(new StatusCount
                {
                    StatusName = IndentStatuses.IndentBoxReceived.DescriptionAttr(),
                    Count = await _indentService.GetIndentCount(filters),
                    Color = "#4644c9",
                });
            }

            return statusOverview;
        }

        public async Task<List<StatusCount>> GetKitStatusOverview(int monthId)
        {
            var dateRange = GetDateRange(monthId);

            dynamic filters = new ExpandoObject();
            filters.regionId = _userProviderService.GetRegionId();
            filters.bfilBranchId = _userProviderService.GetBranchId();
            filters.indentStartDate = dateRange.Item1;
            filters.indentEndDate = dateRange.Item2;

            var statusOverview = new List<StatusCount>();

            if (_userProviderService.BM() || _userProviderService.BCM())
            {
                filters.kitStatusIds = $"{KitStatuses.Dispatched.GetHashCode()}";
                statusOverview.Add(new StatusCount
                {
                    StatusName = KitStatuses.Dispatched.DescriptionAttr(),
                    Count = await _kitService.GetKitCount(filters),
                    Color = "#D8A61A",
                });

                filters.kitStatusIds = $"{KitStatuses.Received.GetHashCode()}";
                statusOverview.Add(new StatusCount
                {
                    StatusName = KitStatuses.Received.DescriptionAttr(),
                    Count = await _kitService.GetKitCount(filters),
                    Color = "#2D5600",
                });

                filters.kitStatusIds = $"{KitStatuses.Allocated.GetHashCode()}";
                statusOverview.Add(new StatusCount
                {
                    StatusName = KitStatuses.Allocated.DescriptionAttr(),
                    Count = await _kitService.GetKitCount(filters),
                    Color = "#98272A",
                });

                filters.kitStatusIds = $"{KitStatuses.Assigned.GetHashCode()}";
                statusOverview.Add(new StatusCount
                {
                    StatusName = KitStatuses.Assigned.DescriptionAttr(),
                    Count = await _kitService.GetKitCount(filters),
                    Color = "#C45819",
                });

                filters.kitStatusIds = $"{KitStatuses.Damaged.GetHashCode()},{KitStatuses.Destruction.GetHashCode()}";
                statusOverview.Add(new StatusCount
                {
                    StatusName = KitStatuses.Destruction.DescriptionAttr(),
                    Count = await _kitService.GetKitCount(filters),
                    Color = "#4644c9",
                });
            }

            return statusOverview;
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public Tuple<string, string> GetDateRange(int monthId = 1)
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            if (monthId == 2)
            {
                startDate = startDate.AddMonths(-1);
                endDate = startDate.AddMonths(1).AddDays(-1);
            }
            else if (monthId == 3)
            {
                startDate = startDate.AddMonths(-2);
            }

            return new Tuple<string, string>(CommonUtils.GetFormatedDate(startDate), CommonUtils.GetFormatedDate(endDate));
        }

        private async Task<DashboardModel> GetDashboardModel(int monthId = 1)
        {
            var model = new DashboardModel
            {
                UserClaim = _userProviderService.UserClaim,
                AllowIndentCreate = _userProviderService.AllowIndentCreate(),
                DisplayRejected = _userProviderService.AllowIndentCreate() || _userProviderService.HOApprover(),
                DisplayDispatched = _userProviderService.ROKitManagement(),
            };

            var dateRange = GetDateRange(monthId);

            dynamic filters = new ExpandoObject();
            filters.regionId = _userProviderService.GetRegionId();
            filters.bfilBranchId = _userProviderService.GetBranchId();
            filters.indentStartDate = dateRange.Item1;
            filters.indentEndDate = dateRange.Item2;

            #region Indent Cards

            if (_userProviderService.HOApprover())
            {
                filters.indentStatusIds = $"{IndentStatuses.PendingApproval.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Indents Received",
                    CardDetail = $"{await _indentService.GetIndentCount(filters)}",
                    CardUrl = $"/Indent#indentReceived"
                });
            }

            if (_userProviderService.HOIndentMaker() ||
                _userProviderService.ROIndentMaker() || _userProviderService.ROKitManagement())
            {
                filters.indentStatusIds = $"{IndentStatuses.PendingApproval.GetHashCode()},{IndentStatuses.Approved.GetHashCode()},{IndentStatuses.IndentForDispatch.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Indents Raised",
                    CardDetail = $"{await _indentService.GetIndentCount(filters)}",
                    CardUrl = $"/Indent#indentRaised"
                });
            }

            if (_userProviderService.IBLCPU())
            {
                filters.indentStatusIds = $"{IndentStatuses.Approved.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Indents Received",
                    CardDetail = $"{await _indentService.GetIndentCount(filters)}",
                    CardUrl = "/Indent#indentReceived"
                });
            }

            if (_userProviderService.HOApprover() || _userProviderService.HOIndentMaker() ||
                _userProviderService.ROIndentMaker() || _userProviderService.ROKitManagement())
            {
                filters.indentStatusIds = $"{IndentStatuses.Approved.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Indents Approved",
                    CardDetail = $"{await _indentService.GetIndentCount(filters)}",
                    CardUrl = $"/Indent#{(_userProviderService.AllowIndentCreate() || _userProviderService.ROKitManagement() ? "indentRaised" : (_userProviderService.IBLCPU() ? "indentReceived" : "indentApproved"))}"
                });
            }

            if (_userProviderService.IBLCPU())
            {
                filters.indentStatusIds = $"{IndentStatuses.IndentForDispatch.GetHashCode()},{IndentStatuses.PartialDispatched.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Indents Approved",
                    CardDetail = $"{await _indentService.GetIndentCount(filters)}",
                    CardUrl = $"/Indent#indentForDispatched"
                });
            }

            if (_userProviderService.HOApprover() || _userProviderService.HOIndentMaker() ||
                _userProviderService.ROIndentMaker() || _userProviderService.IBLCPU())
            {
                filters.indentStatusIds = $"{IndentStatuses.Rejected.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Indents Rejected",
                    CardDetail = $"{await _indentService.GetIndentCount(filters)}",
                    CardUrl = "/Indent#indentRejected"
                });
            }

            if (_userProviderService.HOApprover() || _userProviderService.HOIndentMaker() ||
                _userProviderService.ROIndentMaker() || _userProviderService.ROKitManagement())
            {
                filters.indentStatusIds = $"{IndentStatuses.PendingApproval.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Indents Pending for Approval",
                    CardDetail = $"{await _indentService.GetIndentCount(filters)}",
                    CardUrl = $"/Indent#{(_userProviderService.HOApprover() ? "indentReceived" : "indentRaised")}"
                });
            }

            if (_userProviderService.ROIndentMaker() || _userProviderService.HOIndentMaker() || _userProviderService.HOApprover())
            {
                filters.indentStatusIds = null;
                model.NoOfKitsCard = new DisplayCard
                {
                    CardTitle = "No of Kits Indented",
                    CardDetail = $"{await _indentService.GetIndentKitTotal(filters)}",
                    CardUrl = $"/Indent#{(_userProviderService.HOApprover() ? "indentReceived" : "indentRaised")}"
                };
            }

            if (_userProviderService.ROKitManagement())
            {
                filters.indentStatusIds = $"{IndentStatuses.PendingApproval.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Indents Received",
                    CardDetail = $"{await _indentService.GetIndentCount(filters)}",
                    CardUrl = "/Indent#indentRaised"
                });

                filters.indentStatusIds = $"{IndentStatuses.PartialDispatched.GetHashCode()},{IndentStatuses.Dispatched.GetHashCode()}";
                model.DispatchCards.Add(new DisplayCard
                {
                    CardTitle = "Indents Dispatched To Branch",
                    CardDetail = $"{await _indentService.GetIndentCount(filters)}",
                    CardUrl = "/Indent#indentDispatched"
                });

                filters.indentStatusIds = $"{IndentStatuses.PartialDispatched.GetHashCode()},{IndentStatuses.Dispatched.GetHashCode()}";
                model.DispatchCards.Add(new DisplayCard
                {
                    CardTitle = "Indents Pending For Branch",
                    CardDetail = $"{await _indentService.GetIndentCount(filters)}",
                    CardUrl = "/Indent#indentDispatched"
                });
            }

            if (_userProviderService.IBLCPU())
            {
                filters.indentStatusIds = $"{IndentStatuses.PartialDispatched.GetHashCode()},{IndentStatuses.Dispatched.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Indents Dispatched",
                    CardDetail = $"{await _indentService.GetIndentCount(filters)}",
                    CardUrl = "/Indent#indentDispatched"
                });

                filters.indentStatusIds = $"{IndentStatuses.Cancelled.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Indents Cancelled",
                    CardDetail = $"{await _indentService.GetIndentCount(filters)}",
                    CardUrl = "/Indent#indentCancelled"
                });
            }

            if (_userProviderService.BM() || _userProviderService.BCM())
            {
                filters.indentStatusIds = $"{IndentStatuses.IndentBoxReceived.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Indents Box Received",
                    CardDetail = $"{await _indentService.GetIndentCount(filters)}",
                    CardUrl = "/Indent#indentDispatched"
                });

                filters.indentStatusIds = $"{IndentStatuses.PartialDispatched.GetHashCode()},{IndentStatuses.Dispatched.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Indents Pending for Scanning",
                    CardDetail = $"{await _indentService.GetIndentCount(filters)}",
                    CardUrl = "/Indent#indentDispatched"
                });

                #region Reminders

                var reminders = await _notificationService.GetUserNotifications();
                model.Notifications = reminders.OrderByDescending(r => r.NotificationDate).Take(5).ToList();

                #endregion
            }

            #endregion

            #region Kit Cards

            var kitAllocationAlert = await _settingService.GetSettingsByKey(SettingKeys.KitAllocationAlert);
            var alertDays = Convert.ToInt32(kitAllocationAlert.SettingValue);

            var kitRetunAfterAllocation = await _settingService.GetSettingsByKey(SettingKeys.KitRetunAfterAllocation);
            var allocationDays = Convert.ToInt32(kitRetunAfterAllocation.SettingValue);

            if (_userProviderService.HOApprover())
            {
                filters.kitStatusIds = $"{KitStatuses.Damaged.GetHashCode()}";
                model.KitDestruction = new DisplayCard
                {
                    CardTitle = "Kits Destruction",
                    CardDetail = $"{await _kitService.GetKitCount(filters)}",
                    CardUrl = $"/Kit#kitDestructionHo"
                };
            }

            if (_userProviderService.IBLCPU())
            {
                filters.kitStatusIds = $"{KitStatuses.Destruction.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Kits Destruction",
                    CardDetail = $"{await _kitService.GetKitCount(filters)}",
                    CardUrl = $"/Kit#kitDestructionCpu"
                });
            }

            if (_userProviderService.BM() || _userProviderService.BCM())
            {
                filters.kitStatusIds = $"{KitStatuses.Received.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Kits Received",
                    CardDetail = $"{await _kitService.GetKitCount(filters)}",
                    CardUrl = $"/Kit#kitForAllocation"
                });

                filters.kitStatusIds = $"{KitStatuses.Allocated.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Kits Allocated",
                    CardDetail = $"{await _kitService.GetKitCount(filters)}",
                    CardUrl = $"/Kit#kitAllocated"
                });

                filters.kitStatusIds = $"{KitStatuses.Assigned.GetHashCode()}";
                model.KitCards.Add(new DisplayCard
                {
                    CardTitle = "Kits Assigned",
                    CardDetail = $"{await _kitService.GetKitCount(filters)}",
                    CardUrl = $"/Kit#kitAllocated"
                });

                filters.kitStatusIds = $"{KitStatuses.Damaged.GetHashCode()},{KitStatuses.Destruction.GetHashCode()}";
                model.KitCards.Add(new DisplayCard
                {
                    CardTitle = "Kits Destruction",
                    CardDetail = $"{await _kitService.GetKitCount(filters)}",
                    CardUrl = $"/Kit#kitDestruction"
                });

                filters.kitStatusIds = $"{KitStatuses.Allocated.GetHashCode()}";
                var kitResult = await _kitService.GetKitPaged(new Pagination { Filters = filters });
                var kits = (kitResult.Data as PagedList).Data as List<KitModel>;

                model.KitsAllocationCard = new DisplayCard
                {
                    CardTitle = "Kit Allocated",
                    CardDetail = $"{kits.Count}",
                    CardUrl = $"/Kit#kitAllocated"
                };




                model.KitAllocationAge.Add(new StatusCount
                {
                    StatusName = $"0 - {alertDays} Days",
                    Count = kits.Count(k => k.Age <= alertDays)
                });

                model.KitAllocationAge.Add(new StatusCount
                {
                    StatusName = $"> {alertDays} Days",
                    Count = kits.Count(k => k.Age > alertDays && k.Age <= allocationDays)
                });

                model.KitAllocationAge.Add(new StatusCount
                {
                    StatusName = $"> {allocationDays} Days",
                    Count = kits.Count(k => k.Age > allocationDays)
                });
            }

            if (_userProviderService.Staff())
            {
                filters.kitStatusIds = $"{KitStatuses.Allocated.GetHashCode()},{KitStatuses.Assigned.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Kits Allocated",
                    CardDetail = $"{await _kitService.GetKitCount(filters)}",
                    CardUrl = $"/Kit#kitDetailStaff"
                });

                filters.kitStatusIds = $"{KitStatuses.Assigned.GetHashCode()}";
                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = "Kits Assigned",
                    CardDetail = $"{await _kitService.GetKitCount(filters)}",
                    CardUrl = $"/Kit#kitAssigned"
                });

                filters.kitStatusIds = $"{KitStatuses.Allocated.GetHashCode()}";
                var kitResult = await _kitService.GetKitPaged(new Pagination { Filters = filters });
                var kits = (kitResult.Data as PagedList).Data as List<KitModel>;

                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = $"Kit Aged > {alertDays} Days",
                    CardDetail = $"{kits.Count(k => k.Age > alertDays && k.Age <= allocationDays)}",
                    CardUrl = $"/Kit#kitAssigned"
                });

                model.DisplayCards.Add(new DisplayCard
                {
                    CardTitle = $"Kit Aged > {allocationDays} Days",
                    CardDetail = $"{kits.Count(k => k.Age > allocationDays)}",
                    CardUrl = $"/Kit#kitAssigned"
                });
            }

            #endregion

            return model;
        }
    }
}
