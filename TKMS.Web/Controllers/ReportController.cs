using Core.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Enums;
using TKMS.Repository.Interfaces;
using TKMS.Service.Interfaces;

namespace TKMS.Web.Controllers
{
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;
        private readonly IIndentService _indentService;
        private readonly IBranchService _branchService;
        private readonly IIblBranchService _iblBranchService;
        private readonly ISchemeCodeService _schemeCodeService;
        private readonly IC5CodeService _c5CodeService;
        private readonly ICardTypeService _cardTypeService;
        private readonly IBranchTypeService _branchTypeService;
        private readonly IRegionService _regionService;
        private readonly IAddressService _addressService;
        private readonly IUserProviderService _userProviderService;
        private readonly IIndentStatusService _indentStatusService;
        private readonly IRejectionReasonService _rejectionReasonService;
        private readonly IDispatchService _dispatchService;
        private readonly IBranchDispatchService _branchDispatchService;
        private readonly ICourierStatusService _courierStatusService;
        private readonly IAddressRepository _addressRepository;
        private readonly IFileService _fileService;

        public ReportController(
            IReportService reportService,
            IIndentService indentService,
            IBranchService branchService,
            IIblBranchService iblBranchService,
            ISchemeCodeService schemeCodeService,
            IC5CodeService c5CodeService,
            ICardTypeService cardTypeService,
            IBranchTypeService branchTypeService,
            IRegionService regionService,
            IAddressService addressService,
            IUserProviderService userProviderService,
            IIndentStatusService indentStatusService,
            IRejectionReasonService rejectionReasonService,
            IDispatchService dispatchService,
            ICourierStatusService courierStatusService,
            IBranchDispatchService branchDispatchService,
            IAddressRepository addressRepository,
            IFileService fileService
        )
        {
            _reportService = reportService;
            _indentService = indentService;
            _branchService = branchService;
            _iblBranchService = iblBranchService;
            _schemeCodeService = schemeCodeService;
            _c5CodeService = c5CodeService;
            _cardTypeService = cardTypeService;
            _branchTypeService = branchTypeService;
            _regionService = regionService;
            _addressService = addressService;
            _userProviderService = userProviderService;
            _indentStatusService = indentStatusService;
            _rejectionReasonService = rejectionReasonService;
            _dispatchService = dispatchService;
            _courierStatusService = courierStatusService;
            _branchDispatchService = branchDispatchService;
            _addressRepository = addressRepository;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            return View();
        }

        #region Indent Report
        public async Task<IActionResult> Indent()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            await SetFilterComboBoxes();
            return View();
        }

        public async Task<IActionResult> GetIndentReport(
            string indentNo = null,
            string indentStartDate = null,
            string indentEndDate = null,
            long? branchId = null,
            long? schemeCodeId = null,
            long? c5CodeId = null,
            long? cardTypeId = null,
            long? bfilBranchTypeId = null,
            long? regionId = null,
            long? bfilBranchId = null
       )
        {
            dynamic filters = new ExpandoObject();
            filters.indentNo = indentNo;
            filters.indentStartDate = indentStartDate;
            filters.indentEndDate = indentEndDate;
            filters.branchId = branchId;
            filters.schemeCodeId = schemeCodeId;
            filters.c5CodeId = c5CodeId;
            filters.cardTypeId = cardTypeId;
            filters.bfilBranchTypeId = bfilBranchTypeId;
            filters.regionId = regionId.HasValue ? regionId : _userProviderService.GetRegionId();
            filters.bfilBranchId = bfilBranchId.HasValue ? bfilBranchId : _userProviderService.GetBranchId();

            dynamic preparedObje = PrepareRequestObject(filters);
            var result = await _reportService.GetIndentReport(preparedObje.objPagination);

            var paged = result.Data as PagedList;

            var jsonData = new { draw = preparedObje.draw, recordsFiltered = paged.TotalCount, recordsTotal = paged.TotalCount, data = paged.Data };
            return Ok(jsonData);
        }
        #endregion

        #region Dispatch Report
        public async Task<IActionResult> Dispatch()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            await SetFilterComboBoxes();
            return View();
        }

        public async Task<IActionResult> GetDispatchReport(
            string indentNo = null,
            string indentStartDate = null,
            string indentEndDate = null,
            string dispatchStartDate = null,
            string dispatchEndDate = null,
            long? branchId = null,
            long? schemeCodeId = null,
            long? c5CodeId = null,
            long? cardTypeId = null,
            long? regionId = null,
            long? bfilBranchId = null
       )
        {
            dynamic filters = new ExpandoObject();
            filters.indentNo = indentNo;
            filters.indentStartDate = indentStartDate;
            filters.indentEndDate = indentEndDate;
            filters.dispatchStartDate = dispatchStartDate;
            filters.dispatchEndDate = dispatchEndDate;
            filters.branchId = branchId;
            filters.schemeCodeId = schemeCodeId;
            filters.c5CodeId = c5CodeId;
            filters.cardTypeId = cardTypeId;
            filters.regionId = regionId.HasValue ? regionId : _userProviderService.GetRegionId();
            filters.bfilBranchId = bfilBranchId.HasValue ? bfilBranchId : _userProviderService.GetBranchId();

            dynamic preparedObje = PrepareRequestObject(filters);
            var result = await _reportService.GetDispatchReport(preparedObje.objPagination);

            var paged = result.Data as PagedList;
            var jsonData = new { draw = preparedObje.draw, recordsFiltered = paged.TotalCount, recordsTotal = paged.TotalCount, data = paged.Data };
            return Ok(jsonData);
        }
        #endregion

        #region Delivery Report
        public async Task<IActionResult> Delivery()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            await SetFilterComboBoxes();
            return View();
        }

        public async Task<IActionResult> GetDeliveryReport(
            string indentNo = null,
            string referenceNo = null,
            string wayBillNo = null,
            string dispatchStartDate = null,
            string dispatchEndDate = null,
            string deliveryStartDate = null,
            string deliveryEndDate = null
       )
        {
            dynamic filters = new ExpandoObject();
            filters.indentNo = indentNo;
            filters.referenceNo = referenceNo;
            filters.wayBillNo = wayBillNo;
            filters.dispatchStartDate = dispatchStartDate;
            filters.dispatchEndDate = dispatchEndDate;
            filters.deliveryStartDate = deliveryStartDate;
            filters.deliveryEndDate = deliveryEndDate;
            filters.regionId = _userProviderService.GetRegionId();
            filters.bfilBranchId = _userProviderService.GetBranchId();

            dynamic preparedObje = PrepareRequestObject(filters);
            var result = await _reportService.GetDeliveryReport(preparedObje.objPagination);

            var paged = result.Data as PagedList;

            var jsonData = new { draw = preparedObje.draw, recordsFiltered = paged.TotalCount, recordsTotal = paged.TotalCount, data = paged.Data };
            return Ok(jsonData);
        }
        #endregion

        #region Account Level Dispatch Report
        public async Task<IActionResult> AccountLevelDispatch()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            await SetFilterComboBoxes();
            return View();
        }

        public async Task<IActionResult> GetAccountLevelDispatchReport(
            string indentNo = null,
            string accountNo = null,
            string cifNo = null,
            string dispatchStartDate = null,
            string dispatchEndDate = null,
            long? branchId = null,
            long? regionId = null,
            long? bfilBranchId = null
       )
        {
            dynamic filters = new ExpandoObject();
            filters.indentNo = indentNo;
            filters.accountNo = accountNo;
            filters.cifNo = cifNo;
            filters.dispatchStartDate = dispatchStartDate;
            filters.dispatchEndDate = dispatchEndDate;
            filters.branchId = branchId;
            filters.regionId = regionId.HasValue ? regionId : _userProviderService.GetRegionId();
            filters.bfilBranchId = bfilBranchId.HasValue ? bfilBranchId : _userProviderService.GetBranchId();

            dynamic preparedObje = PrepareRequestObject(filters);
            var result = await _reportService.GetAccountLevelDispatchReport(preparedObje.objPagination);

            var paged = result.Data as PagedList;

            var jsonData = new { draw = preparedObje.draw, recordsFiltered = paged.TotalCount, recordsTotal = paged.TotalCount, data = paged.Data };
            return Ok(jsonData);
        }
        #endregion

        #region Received At RO Report
        public async Task<IActionResult> ReceivedAtRO()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            await SetFilterComboBoxes();

            return View();
        }

        public async Task<IActionResult> GetReceivedAtROReport(
            string indentNo = null,
            string referenceNo = null,
            string wayBillNo = null,
            string dispatchStartDate = null,
            string dispatchEndDate = null,
            long? branchId = null,
            long? schemeCodeId = null,
            long? cardTypeId = null,
            long? regionId = null,
            long? bfilBranchId = null
       )
        {
            dynamic filters = new ExpandoObject();
            filters.indentNo = indentNo;
            filters.referenceNo = referenceNo;
            filters.wayBillNo = wayBillNo;
            filters.dispatchStartDate = dispatchStartDate;
            filters.dispatchEndDate = dispatchEndDate;
            filters.branchId = branchId;
            filters.schemeCodeId = schemeCodeId;
            filters.cardTypeId = cardTypeId;
            filters.regionId = regionId.HasValue ? regionId : _userProviderService.GetRegionId();
            filters.bfilBranchId = bfilBranchId.HasValue ? bfilBranchId : _userProviderService.GetBranchId();
            filters.bfilBranchTypeId = BranchTypes.RO.GetHashCode();

            dynamic preparedObje = PrepareRequestObject(filters);
            var result = await _reportService.GetReceivedAtROReport(preparedObje.objPagination);

            var paged = result.Data as PagedList;

            var jsonData = new { draw = preparedObje.draw, recordsFiltered = paged.TotalCount, recordsTotal = paged.TotalCount, data = paged.Data };
            return Ok(jsonData);
        }
        #endregion

        #region Received At Branch Report
        public async Task<IActionResult> ReceivedAtBranch()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            await SetFilterComboBoxes();

            return View();
        }

        public async Task<IActionResult> GetReceivedAtBranchReport(
            string indentNo = null,
            string referenceNo = null,
            string wayBillNo = null,
            string dispatchStartDate = null,
            string dispatchEndDate = null,
            long? branchId = null,
            long? schemeCodeId = null,
            long? cardTypeId = null,
            long? regionId = null,
            long? bfilBranchId = null
       )
        {
            dynamic filters = new ExpandoObject();
            filters.indentNo = indentNo;
            filters.referenceNo = referenceNo;
            filters.wayBillNo = wayBillNo;
            filters.dispatchStartDate = dispatchStartDate;
            filters.dispatchEndDate = dispatchEndDate;
            filters.branchId = branchId;
            filters.schemeCodeId = schemeCodeId;
            filters.cardTypeId = cardTypeId;
            filters.regionId = regionId.HasValue ? regionId : _userProviderService.GetRegionId();
            filters.bfilBranchId = bfilBranchId.HasValue ? bfilBranchId : _userProviderService.GetBranchId();

            dynamic preparedObje = PrepareRequestObject(filters);
            var result = await _reportService.GetReceivedAtBranchReport(preparedObje.objPagination);

            var paged = result.Data as PagedList;

            var jsonData = new { draw = preparedObje.draw, recordsFiltered = paged.TotalCount, recordsTotal = paged.TotalCount, data = paged.Data };
            return Ok(jsonData);
        }
        #endregion

        #region Scanned Report
        public async Task<IActionResult> Scanned()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            await SetFilterComboBoxes();

            return View();
        }

        public async Task<IActionResult> GetScannedReport(
            string indentNo = null,
            string accountNo = null,
            string cifNo = null,
            string dispatchStartDate = null,
            string dispatchEndDate = null,
            long? branchId = null,
            long? regionId = null,
            long? bfilBranchId = null
       )
        {
            dynamic filters = new ExpandoObject();
            filters.indentNo = indentNo;
            filters.accountNo = accountNo;
            filters.cifNo = cifNo;
            filters.dispatchStartDate = dispatchStartDate;
            filters.dispatchEndDate = dispatchEndDate;
            filters.branchId = branchId;
            filters.regionId = regionId.HasValue ? regionId : _userProviderService.GetRegionId();
            filters.bfilBranchId = bfilBranchId.HasValue ? bfilBranchId : _userProviderService.GetBranchId();

            dynamic preparedObje = PrepareRequestObject(filters);
            var result = await _reportService.GetScannedReport(preparedObje.objPagination);

            var paged = result.Data as PagedList;

            var jsonData = new { draw = preparedObje.draw, recordsFiltered = paged.TotalCount, recordsTotal = paged.TotalCount, data = paged.Data };
            return Ok(jsonData);
        }
        #endregion

        #region Allocation Report
        public async Task<IActionResult> Allocation()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            await SetFilterComboBoxes();

            return View();
        }

        public async Task<IActionResult> GetAllocationReport(
            string indentNo = null,
            string accountNo = null,
            string cifNo = null,
            string dispatchStartDate = null,
            string dispatchEndDate = null,
            long? branchId = null,
            long? regionId = null,
            long? bfilBranchId = null
       )
        {
            dynamic filters = new ExpandoObject();
            filters.indentNo = indentNo;
            filters.accountNo = accountNo;
            filters.cifNo = cifNo;
            filters.dispatchStartDate = dispatchStartDate;
            filters.dispatchEndDate = dispatchEndDate;
            filters.branchId = branchId;
            filters.regionId = regionId.HasValue ? regionId : _userProviderService.GetRegionId();
            filters.bfilBranchId = bfilBranchId.HasValue ? bfilBranchId : _userProviderService.GetBranchId();

            dynamic preparedObje = PrepareRequestObject(filters);
            var result = await _reportService.GetAllocationReport(preparedObje.objPagination);

            var paged = result.Data as PagedList;

            var jsonData = new { draw = preparedObje.draw, recordsFiltered = paged.TotalCount, recordsTotal = paged.TotalCount, data = paged.Data };
            return Ok(jsonData);
        }
        #endregion

        #region Assigned Report
        public async Task<IActionResult> Assigned()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            await SetFilterComboBoxes();

            return View();
        }

        public async Task<IActionResult> GetAssignedReport(
            string indentNo = null,
            string accountNo = null,
            string cifNo = null,
            string dispatchStartDate = null,
            string dispatchEndDate = null,
            long? branchId = null,
            long? regionId = null,
            long? bfilBranchId = null
       )
        {
            dynamic filters = new ExpandoObject();
            filters.indentNo = indentNo;
            filters.accountNo = accountNo;
            filters.cifNo = cifNo;
            filters.dispatchStartDate = dispatchStartDate;
            filters.dispatchEndDate = dispatchEndDate;
            filters.branchId = branchId;
            filters.regionId = regionId.HasValue ? regionId : _userProviderService.GetRegionId();
            filters.bfilBranchId = bfilBranchId.HasValue ? bfilBranchId : _userProviderService.GetBranchId();

            dynamic preparedObje = PrepareRequestObject(filters);
            var result = await _reportService.GetAssignedReport(preparedObje.objPagination);

            var paged = result.Data as PagedList;

            var jsonData = new { draw = preparedObje.draw, recordsFiltered = paged.TotalCount, recordsTotal = paged.TotalCount, data = paged.Data };
            return Ok(jsonData);
        }
        #endregion

        #region Destruction Report
        public async Task<IActionResult> Destruction()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            await SetFilterComboBoxes();

            return View();
        }

        public async Task<IActionResult> GetDestructionReport(
            string indentNo = null,
            string accountNo = null,
            string cifNo = null,
            string dispatchStartDate = null,
            string dispatchEndDate = null,
            long? branchId = null,
            long? regionId = null,
            long? bfilBranchId = null
       )
        {
            dynamic filters = new ExpandoObject();
            filters.indentNo = indentNo;
            filters.accountNo = accountNo;
            filters.cifNo = cifNo;
            filters.dispatchStartDate = dispatchStartDate;
            filters.dispatchEndDate = dispatchEndDate;
            filters.branchId = branchId;
            filters.regionId = regionId.HasValue ? regionId : _userProviderService.GetRegionId();
            filters.bfilBranchId = bfilBranchId.HasValue ? bfilBranchId : _userProviderService.GetBranchId();

            dynamic preparedObje = PrepareRequestObject(filters);
            var result = await _reportService.GetDestructionReport(preparedObje.objPagination);

            var paged = result.Data as PagedList;

            var jsonData = new { draw = preparedObje.draw, recordsFiltered = paged.TotalCount, recordsTotal = paged.TotalCount, data = paged.Data };
            return Ok(jsonData);
        }
        #endregion

        #region TotalStock Report
        public async Task<IActionResult> TotalStock()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            await SetFilterComboBoxes();

            return View();
        }

        public async Task<IActionResult> GetTotalStockReport(
            long? regionId = null,
            long? bfilBranchId = null
       )
        {
            dynamic filters = new ExpandoObject();
            filters.regionId = regionId.HasValue ? regionId : _userProviderService.GetRegionId();
            filters.bfilBranchId = bfilBranchId.HasValue ? bfilBranchId : _userProviderService.GetBranchId();

            dynamic preparedObje = PrepareRequestObject(filters);
            var result = await _reportService.GetTotalStockReport(preparedObje.objPagination);

            var paged = result.Data as PagedList;

            var jsonData = new { draw = preparedObje.draw, recordsFiltered = paged.TotalCount, recordsTotal = paged.TotalCount, data = paged.Data };
            return Ok(jsonData);
        }
        #endregion

        #region Return Report
        public async Task<IActionResult> Return()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }
            await SetFilterComboBoxes();

            return View();
        }

        public async Task<IActionResult> GetReturnReport(string indentNo = null,
            string accountNo = null,
            string cifNo = null,
            string dispatchStartDate = null,
            string dispatchEndDate = null,
            long? branchId = null,
            long? regionId = null,
            long? bfilBranchId = null
       )
        {
            dynamic filters = new ExpandoObject();
            filters.indentNo = indentNo;
            filters.accountNo = accountNo;
            filters.cifNo = cifNo;
            filters.dispatchStartDate = dispatchStartDate;
            filters.dispatchEndDate = dispatchEndDate;
            filters.branchId = branchId;
            filters.regionId = regionId.HasValue ? regionId : _userProviderService.GetRegionId();
            filters.bfilBranchId = bfilBranchId.HasValue ? bfilBranchId : _userProviderService.GetBranchId();

            dynamic preparedObje = PrepareRequestObject(filters);
            var result = await _reportService.GetReturnReport(preparedObje.objPagination);

            var paged = result.Data as PagedList;

            var jsonData = new { draw = preparedObje.draw, recordsFiltered = paged.TotalCount, recordsTotal = paged.TotalCount, data = paged.Data };
            return Ok(jsonData);
        }
        #endregion

        private dynamic PrepareRequestObject(dynamic filters)
        {
            return new
            {
                draw = Request.Form["draw"].FirstOrDefault(),
                sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault(),
                skip = Request.Form["start"].FirstOrDefault() != null ? Convert.ToInt32(Request.Form["start"].FirstOrDefault()) : 0,
                recordsTotal = 0,
                currentPage = 0,
                objPagination = new Pagination
                {
                    PageNumber = 0,
                    PageSize = Request.Form["length"].FirstOrDefault() != null ? Convert.ToInt32(Request.Form["length"].FirstOrDefault()) : 0,
                    SortOrderBy = Request.Form["order[0][dir]"].FirstOrDefault(),
                    SortOrderColumn = Request.Form["search[value]"].FirstOrDefault(),
                    Filters = filters
                }
            };
        }

        private async Task SetFilterComboBoxes()
        {
            long? regionId = _userProviderService.GetRegionId();

            var branches = (await _branchService.GetDropdwon(regionId: regionId))
                           .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.BranchCode} - {b.Text} ", IsActive = b.IsActive }).ToList();

            var iblBranches = (await _iblBranchService.GetDropdwon())
                           .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.IblBranchCode} - {b.Text} ", IsActive = b.IsActive }).ToList();

            var c5Codes = (await _c5CodeService.GetDropdwon())
                          .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.Text} ", IsActive = b.IsActive }).ToList();

            ViewBag.Branches = GetSelectList(iblBranches, "All IBL Branch");
            ViewBag.SchemeCodes = GetSelectList(await _schemeCodeService.GetDropdwon(), "All Scheme Code");
            ViewBag.C5Codes = GetSelectList(c5Codes, "All C5 Code");
            ViewBag.CardTypes = GetSelectList(await _cardTypeService.GetDropdwon(), "All Card Type");
            ViewBag.BranchTypes = GetSelectList(await _branchTypeService.GetDropdwon(true), "All Address Type");
            ViewBag.BfilBranches = GetSelectList(branches, "All BFIL Branch");
            ViewBag.Regions = GetSelectList(await _regionService.GetDropdwon(id: regionId), "All Region");
        }

        private bool AuthorizeUser()
        {
            return true;
        }
    }
}
