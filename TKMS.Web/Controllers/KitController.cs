using Core.Repository.Models;
using Core.Utility.Utils;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Constants;
using TKMS.Abstraction.Enums;
using TKMS.Abstraction.Models;
using TKMS.Repository.Interfaces;
using TKMS.Service.Interfaces;
using TKMS.Web.Models;

namespace TKMS.Web.Controllers
{
    public class KitController : BaseController
    {
        private readonly IKitService _kitService;
        private readonly IUserService _userService;
        private readonly IUserProviderService _userProviderService;
        private readonly ISettingService _settingService;
        private readonly IFileService _fileService;
        private readonly IBranchService _branchService;
        private readonly IIblBranchService _iblBranchService;
        private readonly ICardTypeService _cardTypeService;
        private readonly IKitDamageReasonService _kitDamageReasonService;
        private readonly ISmsService _smsService;
        private readonly ISentSmsService _sentSmsService;

        public KitController(
            IKitService kitService,
            IUserService userService,
            IUserProviderService userProviderService,
            ISettingService settingService,
            IFileService fileService,
            IBranchService branchService,
            IIblBranchService iblBranchService,
            ICardTypeService cardTypeService,
            IKitDamageReasonService kitDamageReasonService,
            ISmsService smsService,
            ISentSmsService sentSmsService
        )
        {
            _kitService = kitService;
            _userService = userService;
            _userProviderService = userProviderService;
            _settingService = settingService;
            _fileService = fileService;
            _branchService = branchService;
            _iblBranchService = iblBranchService;
            _cardTypeService = cardTypeService;
            _kitDamageReasonService = kitDamageReasonService;
            _smsService = smsService;
            _sentSmsService = sentSmsService;
        }

        public async Task<IActionResult> Index()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            var model = new KitDisplayModel
            {
                KitDetail = _userProviderService.BM() || _userProviderService.BCM(),
                KitForAllocation = _userProviderService.BM() || _userProviderService.BCM(),
                KitAllocated = _userProviderService.BM() || _userProviderService.BCM(),
                KitAllocatedStaffWise = _userProviderService.BM() || _userProviderService.BCM(),
                KitDestruction = _userProviderService.BM() || _userProviderService.BCM(),
                KitDestructionCpu = _userProviderService.IBLCPU(),
                KitDetailStaff = _userProviderService.Staff(),
                KitAssigned = _userProviderService.Staff(),
                KitDestructionHo = _userProviderService.HOApprover(),
                KitDestructionApproved = _userProviderService.HOApprover(),
                KitDestructionRejected = _userProviderService.HOApprover(),
            };

            await SetFilterComboBoxes();

            return View(model);
        }

        public async Task<IActionResult> KitDetail(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await _kitService.GetKitDetailById(id));
        }

        public async Task<IActionResult> KitAllocation(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await _kitService.GetKitDetailById(id));
        }

        public async Task<IActionResult> KitAllocated(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await _kitService.GetKitDetailById(id));
        }

        public async Task<IActionResult> Destruction(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await _kitService.GetKitDetailById(id));
        }

        public async Task<IActionResult> KitDestruction(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await _kitService.GetKitDetailById(id));
        }

        public async Task<IActionResult> DestructionCpu(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await _kitService.GetKitDetailById(id));
        }

        public async Task<IActionResult> Approved(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await _kitService.GetKitDetailById(id));
        }

        public async Task<IActionResult> Rejected(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await _kitService.GetKitDetailById(id));
        }

        public async Task<IActionResult> KitDetailStaff(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await _kitService.GetKitDetailById(id));
        }

        public async Task<IActionResult> KitDetailStaffWise(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            var staffDetail = await _kitService.KitStaffDetailByAllocatedId(id);

            if (staffDetail.KitAllocated.Any())
            {
                var kitAllocationAlert = await _settingService.GetSettingsByKey(SettingKeys.KitAllocationAlert);
                var alertDays = Convert.ToInt32(kitAllocationAlert.SettingValue);

                var kitRetunAfterAllocation = await _settingService.GetSettingsByKey(SettingKeys.KitRetunAfterAllocation);
                var allocationDays = Convert.ToInt32(kitRetunAfterAllocation.SettingValue);

                foreach (var kit in staffDetail.KitAllocated)
                {
                    kit.KitAlert = kit.Age > alertDays;
                    kit.ReturnAlert = kit.Age > allocationDays;
                }
            }

            return View(staffDetail);
        }

        public async Task<IActionResult> KitDetailAssigned(long id)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View(await _kitService.GetKitDetailById(id));
        }

        public async Task<IActionResult> KitAllocate()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            var model = new KitAllocateModel { KitIds = Convert.ToString(TempData["AllocateKitIds"]) };
            model.KitDetails = await _kitService.GetStaffKits(model.Kits);
            var allowOTPAuth = await _settingService.GetSettingsByKey(SettingKeys.AllowOTPAuth);
            model.AllowOTPAuth = allowOTPAuth.SettingValue == "1";
            await SetStaffs();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> KitAllocate(KitAllocateModel model)
        {
            var kitAllocationStaff = await _settingService.GetSettingsByKey(SettingKeys.KitAllocationStaff);
            if (Convert.ToInt32(kitAllocationStaff.SettingValue) < model.Kits.Count)
            {
                SetNotification($"Maximum kit can be allocated to a staff {kitAllocationStaff.SettingValue}", NotificationTypes.Error, "Kit Allocation");
                await SetStaffs();
                model.KitDetails = await _kitService.GetStaffKits(model.Kits);
                return View(model);
            }

            var kitCount = await _kitService.GetAllocatedKitCount(model.StaffId, KitStatuses.Allocated.GetHashCode());
            var minKitAllocationStaff = await _settingService.GetSettingsByKey(SettingKeys.MinKitAllocationStaff);
            if (Convert.ToInt32(minKitAllocationStaff.SettingValue) <= kitCount)
            {
                SetNotification($"Staff already has {minKitAllocationStaff.SettingValue} allocated kits", NotificationTypes.Error, "Kit Allocation");
                await SetStaffs();
                model.KitDetails = await _kitService.GetStaffKits(model.Kits);
                return View(model);
            }

            var result = await _kitService.UpdateKitsAllocated(model);
            if (result.Success)
            {
                SetNotification($"Kit{(model.Kits.Count > 0 ? "s" : "")} Allocated!", NotificationTypes.Success, "Kit Allocation");
                return Redirect(Url.Action("Index") + "#kitForAllocation");
            }

            SetNotification($"Kit{(model.Kits.Count > 0 ? "s" : "")} Not Allocated!", NotificationTypes.Error, "Kit Allocation");
            await SetStaffs();
            model.KitDetails = await _kitService.GetStaffKits(model.Kits);
            return View(model);
        }

        public async Task<IActionResult> KitDestruct()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            var model = new KitDestructModel { KitIds = Convert.ToString(TempData["DestructKitIds"]) };
            model.KitDetails = await _kitService.GetStaffKits(model.Kits);
            await SetKitDamageReasons();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> KitDestruct(KitDestructModel model)
        {
            var result = await _kitService.UpdateKitsDestructed(model);
            if (result.Success)
            {
                SetNotification($"Kit{(model.Kits.Count > 0 ? "s" : "")} Destructed!", NotificationTypes.Success, "Kit Destruct");
                return Redirect(Url.Action("Index") + "#kitAllocated");
            }

            SetNotification($"Kit{(model.Kits.Count > 0 ? "s" : "")} Not Destructed!", NotificationTypes.Error, "Kit Destruct");
            await SetKitDamageReasons();
            model.KitDetails = await _kitService.GetStaffKits(model.Kits);
            return View(model);
        }

        public async Task<IActionResult> KitReturn()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            var model = new KitAllocateModel { KitIds = Convert.ToString(TempData["ReturnKitIds"]) };
            var allowOTPAuth = await _settingService.GetSettingsByKey(SettingKeys.AllowOTPAuth);
            model.AllowOTPAuth = allowOTPAuth.SettingValue == "1";
            model.KitDetails = await _kitService.GetStaffKits(model.Kits);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> KitReturn(KitAllocateModel model)
        {
            var result = await _kitService.UpdateKitReturned(model);
            if (result.Success)
            {
                SetNotification($"Kit{(model.Kits.Count > 0 ? "s" : "")} Returned", NotificationTypes.Success, "Kit Return");
                return Redirect(Url.Action("Index") + "#kitForAllocation");
            }

            SetNotification($"Kit{(model.Kits.Count > 0 ? "s" : "")} Not Returned!", NotificationTypes.Error, "Kit Return");
            await SetStaffs();
            model.KitDetails = await _kitService.GetStaffKits(model.Kits);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> KitDestuctionApproval(KitDestructionRequest model)
        {
            return Ok(await _kitService.UpdateKitDestruction(model));
        }

        [HttpPost]
        public async Task<IActionResult> ValidateUser(LoginModel model)
        {
            return Ok(await _userService.AuthenticateBfilUser(model));
        }

        [HttpPost]
        public async Task<IActionResult> GenerateOTP(long userId, string mobile, bool isAllocate)
        {
            var smsRequest = new SentSmsRequest
            {
                Otp = new Random().Next(100000, 999999).ToString(),
                MobileNumbers = mobile,
                IsAllocate = isAllocate
            };

            return Ok(await _smsService.SendSms(smsRequest));
        }

        [HttpPost]
        public async Task<IActionResult> ValidateOTP(long sentSmsId, string otp)
        {
            var sentSms = await _sentSmsService.GetSentSmsById(sentSmsId);
            if (sentSms.Success)
            {
                var sms = sentSms.Data as SentSms;
                if (otp == sms.Otp)
                {
                    return Ok(new ResponseModel { Success = true, StatusCode = StatusCodes.Status200OK, Message = "OTP matched" });
                }
            }

            return Ok(new ResponseModel { Success = false, StatusCode = StatusCodes.Status406NotAcceptable, Message = "OTP did not match" });
        }

        public async Task<IActionResult> GetKits(
            string kitStatusIds = null,
            string indentNo = null,
            string accountNo = null,
            string cifNo = null,
            long? branchId = null,
            long? cardTypeId = null,
            long? bfilBranchId = null,
            long? allocatedToId = null,
            string allocatedDate = null,
            long? kitDamageReasonId = null,
            string assignedDate = null,
            bool? isDestructionApproved = null,
            string kitDate = null
            )
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int currentPage = skip / Convert.ToInt32(length) + 1;

                dynamic filters = new ExpandoObject();
                filters.kitStatusIds = kitStatusIds;
                filters.indentNo = indentNo;
                filters.accountNo = accountNo;
                filters.cifNo = cifNo;
                filters.branchId = branchId;
                filters.cardTypeId = cardTypeId;
                filters.bfilBranchId = bfilBranchId.HasValue ? bfilBranchId : _userProviderService.GetBranchId();
                filters.allocatedToId = allocatedToId;
                filters.allocatedDate = allocatedDate;
                filters.kitDamageReasonId = kitDamageReasonId;
                filters.assignedDate = assignedDate;
                filters.isDestructionApproved = isDestructionApproved;
                filters.kitDate = kitDate;

                var kitResult = await _kitService.GetKitPaged(
                    new Pagination
                    {
                        PageNumber = currentPage,
                        PageSize = pageSize,
                        SortOrderBy = sortColumnDirection,
                        SortOrderColumn = sortColumn,
                        Filters = filters
                    });

                var kitPaged = kitResult.Data as PagedList;

                recordsTotal = kitPaged.TotalCount;

                var kits = kitPaged.Data as List<KitModel>;

                if (kits.Any())
                {
                    var kitAllocationAlert = await _settingService.GetSettingsByKey(SettingKeys.KitAllocationAlert);
                    var alertDays = Convert.ToInt32(kitAllocationAlert.SettingValue);

                    var kitRetunAfterAllocation = await _settingService.GetSettingsByKey(SettingKeys.KitRetunAfterAllocation);
                    var allocationDays = Convert.ToInt32(kitRetunAfterAllocation.SettingValue);

                    kits.ForEach(k =>
                    {
                        k.KitAlert = k.Age > alertDays && k.KitStatusId == KitStatuses.Allocated.GetHashCode();
                        k.AllowReAllocate = k.Age > allocationDays && k.KitStatusId == KitStatuses.Allocated.GetHashCode();
                    });
                }

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = kits };

                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetKitStaff(
            string kitStatusIds = null,
            long? allocatedToId = null
        )
        {
            try
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                int currentPage = skip / Convert.ToInt32(length) + 1;

                dynamic filters = new ExpandoObject();
                filters.kitStatusIds = kitStatusIds;
                filters.allocatedToId = allocatedToId;
                filters.bfilBranchId = _userProviderService.GetBranchId();

                var kitResult = await _kitService.GetKitStaffPaged(
                    new Pagination
                    {
                        PageNumber = currentPage,
                        PageSize = pageSize,
                        SortOrderBy = sortColumnDirection,
                        SortOrderColumn = sortColumn,
                        Filters = filters
                    });
                var kitPaged = kitResult.Data as PagedList;
                recordsTotal = kitPaged.TotalCount;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = kitPaged.Data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> SetDestructIds(string kitIds)
        {
            TempData["DestructKitIds"] = kitIds;
            return Ok();
        }

        public async Task<IActionResult> SetAllocateIds(string kitIds)
        {
            TempData["AllocateKitIds"] = kitIds;
            return Ok();
        }

        public async Task<IActionResult> SetReturnIds(string kitIds)
        {
            var result = await _kitService.IsSingleStaff(kitIds.Split(',').Select(long.Parse).ToList());
            if (result)
            {
                TempData["ReturnKitIds"] = kitIds;
            }
            return Ok(result);
        }

        public async Task<IActionResult> GetUserDetails(long userId)
        {
            return Ok(await _userService.GetUserById(userId));
        }

        private async Task SetKitDamageReasons()
        {
            ViewBag.KitDamageReasons = GetSelectList(await _kitDamageReasonService.GetDropdwon(), "Select Destruction Reason");
        }

        private async Task SetStaffs()
        {
#if DEBUG
            ViewBag.Staffs = GetSelectList(await _userService.GetDropdwon(
                roleTypeId: RoleTypes.STAFF.GetHashCode()), "Select Staff");
#else
            ViewBag.Staffs = GetSelectList(await _userService.GetDropdwon(
                branchId: _userProviderService.UserClaim.BranchId,
                roleTypeId: RoleTypes.STAFF.GetHashCode()), "Select Staff");
#endif
        }

        private async Task SetFilterComboBoxes()
        {
            long? regionId = _userProviderService.GetRegionId();

            var branches = (await _branchService.GetDropdwon(regionId: regionId))
                           .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.BranchCode} - {b.Text} ", IsActive = b.IsActive }).ToList();

            var iblBranches = (await _iblBranchService.GetDropdwon())
                           .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.IblBranchCode} - {b.Text} ", IsActive = b.IsActive }).ToList();

            ViewBag.Branches = GetSelectList(iblBranches, "All IBL Branch");
            ViewBag.CardTypes = GetSelectList(await _cardTypeService.GetDropdwon(), "All Card Type");
            ViewBag.BfilBranches = GetSelectList(branches, "All BFIL Branch");
            ViewBag.DamageReasons = GetSelectList(await _kitDamageReasonService.GetDropdwon(), "All Destruction Reason");

#if DEBUG
            ViewBag.Staffs = GetSelectList(await _userService.GetDropdwon(roleTypeId: RoleTypes.STAFF.GetHashCode()), "All Staff");
#else
            ViewBag.Staffs = GetSelectList(await _userService.GetDropdwon(
                branchId: _userProviderService.UserClaim.BranchId,
                roleTypeId: RoleTypes.STAFF.GetHashCode()), "All Staff");
#endif
        }

        private bool AuthorizeUser()
        {
            return _userProviderService.BM() || _userProviderService.BCM() ||
                _userProviderService.Staff() || _userProviderService.HOApprover() || _userProviderService.IBLCPU();
        }
    }
}
