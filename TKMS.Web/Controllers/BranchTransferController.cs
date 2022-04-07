using Core.Repository.Models;
using Core.Utility.Utils;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class BranchTransferController : BaseController
    {
        private readonly IBranchTransferService _branchTransferService;
        private readonly IKitService _kitService;
        private readonly IUserProviderService _userProviderService;
        private readonly IBranchService _branchService;
        private readonly ICardTypeService _cardTypeService;

        public BranchTransferController(
            IBranchTransferService branchTransferService,
            IKitService kitService,
            IUserProviderService userProviderService,
            ICardTypeService cardTypeService,
            IBranchService branchService
        )
        {
            _branchTransferService = branchTransferService;
            _kitService = kitService;
            _userProviderService = userProviderService;
            _cardTypeService = cardTypeService;
            _branchService = branchService;
        }

        public async Task<IActionResult> Index()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            await SetFilterComboBoxes();

            return View();
        }

        public async Task<IActionResult> KitTransfer(long id)
        {
            return View(await _kitService.GetKitDetailById(id));
        }

        public async Task<IActionResult> KitSent(long id)
        {
            return View(await _branchTransferService.GetTransferDetailById(id));
        }

        public async Task<IActionResult> KitReceived(long id)
        {
            return View(await _branchTransferService.GetTransferDetailById(id));
        }

        public async Task<IActionResult> BranchTransfer()
        {
            await SetBranches();
            var model = new TransferModel { KitIds = Convert.ToString(TempData["TransferKitIds"]) };
            model.KitDetails = await _kitService.GetStaffKits(model.Kits);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BranchTransfer(TransferModel model)
        {
            var result = await _branchTransferService.CreateBranchTransfers(model);
            if (result.Success)
            {
                SetNotification("Kits Transfer", NotificationTypes.Success, "Kit Transfer");
                return Redirect(Url.Action("Index") + "#kitTransfer");
            }

            await SetBranches();
            model.KitDetails = await _kitService.GetStaffKits(model.Kits);
            return View(model);
        }

        public async Task<IActionResult> SetTransferIds(string kitIds)
        {
            TempData["TransferKitIds"] = kitIds;
            return Ok();
        }

        public async Task<IActionResult> GetTransferKits(
            string indentNo = null,
            string accountNo = null,
            string cifNo = null,
            long? branchId = null,
            long? cardTypeId = null,
            long? bfilBranchId = null,
            string transferDate = null,
            string receivedDate = null,
            bool? isSent = null
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
                filters.isSent = isSent;
                filters.indentNo = indentNo;
                filters.accountNo = accountNo;
                filters.cifNo = cifNo;
                filters.branchId = isSent.HasValue && isSent.Value ? (branchId.HasValue ? branchId : _userProviderService.UserClaim.BranchId) : null;
                filters.cardTypeId = cardTypeId;
                filters.bfilBranchId = bfilBranchId.HasValue ? bfilBranchId : _userProviderService.UserClaim.BranchId;
                filters.bfilBranchId = isSent.HasValue && !isSent.Value ? (bfilBranchId.HasValue ? bfilBranchId : _userProviderService.UserClaim.BranchId) : null;
                filters.transferDate = transferDate;
                filters.receivedDate = receivedDate;

                var tranferKitResult = await _branchTransferService.GetBranchTransferPaged(
                    new Pagination
                    {
                        PageNumber = currentPage,
                        PageSize = pageSize,
                        SortOrderBy = sortColumnDirection,
                        SortOrderColumn = sortColumn,
                        Filters = filters
                    });

                var tranferKitPaged = tranferKitResult.Data as PagedList;

                recordsTotal = tranferKitPaged.TotalCount;

                var kits = tranferKitPaged.Data as List<BranchTransferModel>;

                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = kits };

                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetBranchDetails(long branchId)
        {
            return Ok(await _branchService.GetBranchById(branchId));
        }

        private async Task SetBranches()
        {
            long? regionId = _userProviderService.GetRegionId();
            long? branchId = _userProviderService.GetBranchId();

            var branches = (await _branchService.GetDropdwon(regionId: regionId))
                           .Where(b => b.Id != branchId)
                           .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.BranchCode} - {b.Text} ", IsActive = b.IsActive }).ToList();

            ViewBag.Branches = GetSelectList(branches, "Select Branch");
        }

        private async Task SetFilterComboBoxes()
        {
            long? regionId = _userProviderService.GetRegionId();

            var branches = (await _branchService.GetDropdwon(regionId: regionId))
                           .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.BranchCode} - {b.Text} ", IsActive = b.IsActive }).ToList();

            ViewBag.Branches = GetSelectList(branches, "All Branch");
            ViewBag.CardTypes = GetSelectList(await _cardTypeService.GetDropdwon(), "All Card Type");
            ViewBag.BfilBranches = GetSelectList(branches, "All BFIL Branch");
            ViewBag.DamageReasons = new List<SelectListItem>();
            ViewBag.Staffs = new List<SelectListItem>();
        }

        private bool AuthorizeUser()
        {
            return _userProviderService.BM() || _userProviderService.BCM();
        }
    }
}
