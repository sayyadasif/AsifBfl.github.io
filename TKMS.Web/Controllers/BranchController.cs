using Core.Repository.Models;
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
using TKMS.Abstraction.Enums;
using TKMS.Abstraction.Models;
using TKMS.Service.Interfaces;
using TKMS.Web.Models;

namespace TKMS.Web.Controllers
{
    public class BranchController : BaseController
    {
        private readonly IBranchService _branchService;
        private readonly IIblBranchService _iblBranchService;
        private readonly IBranchTypeService _branchTypeService;
        private readonly IRegionService _regionService;
        private readonly IAddressService _addressService;
        private readonly IFileService _fileService;
        private readonly IUserProviderService _userProviderService;
        private readonly ISchemeCodeService _schemeCodeService;
        private readonly IC5CodeService _c5CodeService;
        private readonly ICardTypeService _cardTypeService;

        public BranchController(
           IBranchService branchService,
           IIblBranchService iblBranchService,
           IBranchTypeService branchTypeService,
           IRegionService regionService,
           IAddressService addressService,
           IFileService fileService,
           IUserProviderService userProviderService,
            ISchemeCodeService schemeCodeService,
           IC5CodeService c5CodeService,
           ICardTypeService cardTypeService
        )
        {
            _branchService = branchService;
            _iblBranchService = iblBranchService;
            _branchTypeService = branchTypeService;
            _regionService = regionService;
            _addressService = addressService;
            _fileService = fileService;
            _userProviderService = userProviderService;
            _schemeCodeService = schemeCodeService;
            _c5CodeService = c5CodeService;
            _cardTypeService = cardTypeService;
        }

        public async Task<IActionResult> Index()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            return View();
        }

        public async Task<IActionResult> Manage(long id = 0)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            var model = new Branch();
            if (id > 0)
            {
                var branchResult = await _branchService.GetBranchById(id);
                if (branchResult.Success)
                {
                    model = branchResult.Data;
                    var addressResult = await _addressService.GetAddressById(model.AddressId);
                    if (addressResult.Success)
                    {
                        model.Address = addressResult.Data;
                    }
                }
            }
            await SetComboBoxes(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(Branch model)
        {
            if (!ModelState.IsValid)
            {
                SetNotification("Please enter valid form detail", NotificationTypes.Error, "Branch");
                await SetComboBoxes(model);
                return View(model);
            }

            ResponseModel branchResult;
            if (model.BranchId == 0)
            {
                branchResult = await _branchService.CreateBranch(model);
            }
            else
            {
                branchResult = await _branchService.UpdateBranch(model);
            }

            if (branchResult.Success)
            {
                SetNotification($"Branch saved successfully!", NotificationTypes.Success, "ReBranchion");
                return RedirectToAction("Index", "Branch");
            }

            await SetComboBoxes(model);
            SetNotification(branchResult.Message, NotificationTypes.Error, "Branch");
            return View(model);
        }


        [HttpPost]
        public async Task<ResponseModel> UploadBranches(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new ResponseModel { Success = false, Message = "Please select the file!", StatusCode = StatusCodes.Status400BadRequest };

            var path = await _fileService.CreateFile(file);

            var branches = new List<Branch>();
            var existBranches = await _branchService.GetDropdwon();
            var regions = await _regionService.GetDropdwon();
            var iblBranches = await _iblBranchService.GetDropdwon();
            var schemeCodes = (await _schemeCodeService.GetAllSchemeCode()).Data as List<SchemeCode>;
            var c5Codes = await _c5CodeService.GetDropdwon();
            var branchFileError = string.Empty;

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        int index = 0;
                        int branchCodeIndex = 0;
                        int branchNameIndex = 1;
                        int addressIndex = 2;
                        int pincodeIndex = 3;
                        int regionNameIndex = 4;
                        int ifscCodeIndex = 5;
                        int schemeCodeIndex = 6;
                        int c5CodeIndex = 7;
                        int cardTypeIndex = 8;
                        int iblBranchCodeIndex = 9;

                        while (reader.Read()) //Each row of the file
                        {
                            if (index == 0)
                            {
                            }
                            else
                            {
                                var branchCode = reader.GetValue(branchCodeIndex)?.ToString().Trim() ?? "";
                                var branchName = reader.GetValue(branchNameIndex)?.ToString().Trim() ?? "";
                                var addressDetail = reader.GetValue(addressIndex)?.ToString().Trim() ?? "";
                                var pincode = reader.GetValue(pincodeIndex)?.ToString().Trim() ?? "";
                                var regionName = reader.GetValue(regionNameIndex)?.ToString().Trim() ?? "";
                                var ifscCode = reader.GetValue(ifscCodeIndex)?.ToString().Trim() ?? "";
                                var schemeCode = reader.GetValue(schemeCodeIndex)?.ToString().Trim() ?? "";
                                var c5Code = reader.GetValue(c5CodeIndex)?.ToString().Trim() ?? "";
                                var cardType = reader.GetValue(cardTypeIndex)?.ToString().Trim() ?? "";
                                var iblBranchCode = reader.GetValue(iblBranchCodeIndex)?.ToString().Trim() ?? "";

                                if (string.IsNullOrEmpty(branchCode) && string.IsNullOrEmpty(branchName) &&
                                    string.IsNullOrEmpty(addressDetail) && string.IsNullOrEmpty(pincode) &&
                                    string.IsNullOrEmpty(regionName) && string.IsNullOrEmpty(ifscCode) &&
                                    string.IsNullOrEmpty(schemeCode) && string.IsNullOrEmpty(c5Code) &&
                                    string.IsNullOrEmpty(cardType) && string.IsNullOrEmpty(iblBranchCode))
                                {
                                    continue;
                                }

                                var rowErrorMessage = string.Empty;

                                if (string.IsNullOrEmpty(branchCode))
                                {
                                    rowErrorMessage += "<li>Branch Code has no content</li>";
                                }
                                else if (existBranches.Any(r => r.BranchCode == branchCode))
                                {
                                    rowErrorMessage += "<li>Branch Code already exists</li>";
                                }
                                else if (branches.Any(r => r.BranchCode == branchCode))
                                {
                                    rowErrorMessage += $"<li>Branch Code: {branchCode} duplicate in file</li>";
                                }

                                if (string.IsNullOrEmpty(branchName))
                                {
                                    rowErrorMessage += "<li>Branch Name has no content</li>";
                                }
                                else if (existBranches.Any(r => r.Text == branchName))
                                {
                                    rowErrorMessage += "<li>Branch Name already exists</li>";
                                }
                                else if (branches.Any(r => r.BranchName == branchName))
                                {
                                    rowErrorMessage += $"<li>Branch Name: {branchName} duplicate in file</li>";
                                }

                                if (string.IsNullOrEmpty(addressDetail))
                                {
                                    rowErrorMessage += "<li>Address has no content</li>";
                                }

                                if (string.IsNullOrEmpty(pincode))
                                {
                                    rowErrorMessage += "<li>Pincode has no content</li>";
                                }

                                if (string.IsNullOrEmpty(regionName))
                                {
                                    rowErrorMessage += "<li>Region Name has no content</li>";
                                }

                                if (string.IsNullOrEmpty(ifscCode))
                                {
                                    rowErrorMessage += "<li>IFSC Code has no content</li>";
                                }

                                var _region = regions.FirstOrDefault(r => r.Text == regionName);
                                if (!string.IsNullOrEmpty(regionName) && _region == null)
                                {
                                    rowErrorMessage += $"<li>Region Name: {regionName} does not exists</li>";
                                }

                                var _schemeCode = schemeCodes.FirstOrDefault(sc => sc.SchemeCodeName == schemeCode);

                                if (string.IsNullOrEmpty(schemeCode))
                                {
                                    rowErrorMessage += "<li>Scheme Code has no content</li>";
                                }
                                else if (_schemeCode == null)
                                {
                                    rowErrorMessage += "<li>Scheme Code did not match</li>";
                                }

                                var _c5Code = c5Codes.FirstOrDefault(c => c.Text == c5Code);

                                if (string.IsNullOrEmpty(c5Code))
                                {
                                    rowErrorMessage += "<li>C5 Code has no content</li>";
                                }
                                else if (_c5Code == null)
                                {
                                    rowErrorMessage += "<li>C5 Code did not match</li>";
                                }
                                else if (_schemeCode != null && _c5Code != null && !_schemeCode.SchemeC5Codes.Select(s => s.C5CodeId).Contains(_c5Code.Id))
                                {
                                    rowErrorMessage += "<li>C5 Code did not match with Scheme Code</li>";
                                }

                                if (string.IsNullOrEmpty(cardType))
                                {
                                    rowErrorMessage += "<li>Card Type has no content</li>";
                                }
                                else if (_c5Code != null && _c5Code.CardTypeName != cardType)
                                {
                                    rowErrorMessage += "<li>Card Type did not match with C5 Code</li>";
                                }

                                var _iblBranch = iblBranches.FirstOrDefault(c => c.IblBranchCode == iblBranchCode);

                                if (string.IsNullOrEmpty(iblBranchCode))
                                {
                                    rowErrorMessage += "<li>IBL Branch Code has no content</li>";
                                }
                                else if (_iblBranch == null)
                                {
                                    rowErrorMessage += "<li>IBL Branch Code did not match</li>";
                                }

                                if (!string.IsNullOrEmpty(rowErrorMessage))
                                {
                                    rowErrorMessage = $"<ul>Row {index} has error.<br>{rowErrorMessage}</ul>";
                                    branchFileError += rowErrorMessage;
                                    index++;
                                    continue;
                                }

                                branches.Add(new Branch
                                {
                                    BranchCode = branchCode,
                                    BranchName = branchName,
                                    BranchTypeId = branchCode.StartsWith("HO") ? BranchTypes.HO.GetHashCode() :
                                                   branchCode.StartsWith("RO") ? BranchTypes.RO.GetHashCode() :
                                                                                 BranchTypes.Branch.GetHashCode(),
                                    IfscCode = ifscCode,
                                    IblBranchId = _iblBranch.Id,
                                    RegionId = _region.Id,
                                    SchemeCodeId = _schemeCode.SchemeCodeId,
                                    C5CodeId = _c5Code.Id,
                                    CardTypeId = _c5Code.CardTypeId,
                                    Address = new Address { AddressDetail = addressDetail, PinCode = pincode }
                                });
                            }
                            index++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                branchFileError = "Please upload valid content for Branches!";
            }

            _fileService.DeleteFile(path);

            if (!string.IsNullOrEmpty(branchFileError))
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status400BadRequest, Message = branchFileError };
            }

            return await _branchService.CreateBranches(branches);
        }

        public async Task<IActionResult> GetBranches()
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
                filters.branchName = searchValue;
                filters.branchCode = searchValue;

                var branchResult = await _branchService.GetBranchPaged(
                    new Pagination
                    {
                        PageNumber = currentPage,
                        PageSize = pageSize,
                        SortOrderBy = sortColumnDirection,
                        SortOrderColumn = sortColumn,
                        Filters = filters
                    });
                var branchPaged = branchResult.Data as PagedList;
                recordsTotal = branchPaged.TotalCount;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = branchPaged.Data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetC5Codes(long schemeCodeId)
        {
            var c5Codes = await _c5CodeService.GetDropdwon(schemeCodeId: schemeCodeId);
            return Ok(new
            {
                c5Codes,
                cardType = c5Codes.Count == 1 ? await _cardTypeService.GetDropdwon(id: c5Codes.FirstOrDefault().CardTypeId) : null
            });
        }

        public async Task<IActionResult> GetCardType(long c5CodeId)
        {
            return Ok(await _cardTypeService.GetDropdwon(c5CodeId: c5CodeId));
        }

        private async Task SetComboBoxes(Branch branch)
        {
            var iblBranches = (await _iblBranchService.GetDropdwon())
                          .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.IblBranchCode} - {b.Text} ", IsActive = b.IsActive }).ToList();
            ViewBag.IblBranches = GetSelectList(iblBranches, "Select IBL Branch");
            ViewBag.BranchTypes = GetSelectList(await _branchTypeService.GetDropdwon(), "Select Branch Type");
            ViewBag.Regions = GetSelectList(await _regionService.GetDropdwon(), "Select Region");
            ViewBag.SchemeCodes = GetSelectList(await _schemeCodeService.GetDropdwon(), "Select Scheme Code");
            var c5Codes = branch.BranchId > 0 ? (await _c5CodeService.GetDropdwon(schemeCodeId: branch.SchemeCodeId > 0 ? branch.SchemeCodeId : null))
                        .Select(b => new DropdownModel { Id = b.Id, Text = b.Text, IsActive = b.IsActive }).ToList() : new List<DropdownModel>();
            ViewBag.C5Codes = GetSelectList(c5Codes, "Select C5 Code");
            ViewBag.CardTypes = GetSelectList(branch.BranchId > 0 ? await _cardTypeService.GetDropdwon(id: branch.CardTypeId > 0 ? branch.CardTypeId : null) : new List<DropdownModel>(), "Select Card Type");
        }

        private bool AuthorizeUser()
        {
            return _userProviderService.SystemAdmin();
        }
    }
}
