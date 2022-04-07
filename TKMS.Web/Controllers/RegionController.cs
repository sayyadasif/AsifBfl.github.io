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
    public class RegionController : BaseController
    {
        private readonly IUserProviderService _userProviderService;
        private readonly IRegionService _regionService;
        private readonly IAddressService _addressService;
        private readonly IFileService _fileService;

        public RegionController(
           IUserProviderService userProviderService,
           IRegionService regionService,
           IAddressService addressService,
           IFileService fileService
        )
        {
            _userProviderService = userProviderService;
            _regionService = regionService;
            _addressService = addressService;
            _fileService = fileService;
        }

        public async Task<IActionResult> Index()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            return View();
        }

        public async Task<IActionResult> Manage(long id = 0)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            var model = new Region();
            if (id > 0)
            {
                var regionResult = await _regionService.GetRegionById(id);
                if (regionResult.Success)
                {
                    model = regionResult.Data;
                    var addressResult = await _addressService.GetAddressById(model.AddressId);
                    if (addressResult.Success)
                    {
                        model.Address = addressResult.Data;
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(Region model)
        {
            if (!ModelState.IsValid)
            {
                SetNotification("Please enter valid form detail", NotificationTypes.Error, "Region");
                return View(model);
            }

            ResponseModel regionResult;
            if (model.RegionId == 0)
            {
                regionResult = await _regionService.CreateRegion(model);
            }
            else
            {
                regionResult = await _regionService.UpdateRegion(model);
            }

            if (regionResult.Success)
            {
                SetNotification($"Region saved successfully!", NotificationTypes.Success, "Region");
                return RedirectToAction("Index", "Region");
            }

            SetNotification(regionResult.Message, NotificationTypes.Error, "Region");
            return View(model);
        }

        [HttpPost]
        public async Task<ResponseModel> UploadRegions(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new ResponseModel { Success = false, Message = "Please select the file!", StatusCode = StatusCodes.Status400BadRequest };

            var path = await _fileService.CreateFile(file);

            var regions = new List<Region>();
            var existRegions = (await _regionService.GetAllRegion()).Data as List<Region>;
            var regionFileError = string.Empty;

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        int index = 0;
                        int roidIndex = 0;
                        int nameIndex = 1;
                        int zoneIndex = 2;
                        int addressIndex = 3;
                        int stateIndex = 4;
                        int pincodeIndex = 5;
                        int regionIndex = 6;

                        while (reader.Read()) //Each row of the file
                        {
                            if (index == 0)
                            {
                            }
                            else
                            {
                                var roid = reader.GetValue(roidIndex)?.ToString().Trim() ?? "";
                                var regionName = reader.GetValue(nameIndex)?.ToString().Trim() ?? "";
                                var zone = reader.GetValue(zoneIndex)?.ToString().Trim() ?? "";
                                var address = reader.GetValue(addressIndex)?.ToString().Trim() ?? "";
                                var state = reader.GetValue(stateIndex)?.ToString().Trim() ?? "";
                                var pincode = reader.GetValue(pincodeIndex)?.ToString().Trim() ?? "";
                                var region = reader.GetValue(regionIndex)?.ToString().Trim() ?? "";

                                if (string.IsNullOrEmpty(roid) && string.IsNullOrEmpty(regionName) && string.IsNullOrEmpty(zone) &&
                                    string.IsNullOrEmpty(address) && string.IsNullOrEmpty(state) &&
                                    string.IsNullOrEmpty(pincode) && string.IsNullOrEmpty(region))
                                {
                                    continue;
                                }

                                var rowErrorMessage = string.Empty;

                                if (string.IsNullOrEmpty(roid))
                                {
                                    rowErrorMessage += "<li>RO ID has no content</li>";
                                }
                                else if (existRegions.Any(r => r.SystemRoId == roid))
                                {
                                    rowErrorMessage += "<li>RO ID already exists</li>";
                                }
                                else if (regions.Any(r => r.SystemRoId == roid))
                                {
                                    rowErrorMessage += $"<li>RO ID: {regionName} duplicate in file</li>";
                                }

                                if (string.IsNullOrEmpty(regionName))
                                {
                                    rowErrorMessage += "<li>Region Name has no content</li>";
                                }
                                else if (existRegions.Any(r => r.RegionName == regionName))
                                {
                                    rowErrorMessage += "<li>Region Name already exists</li>";
                                }
                                else if (regions.Any(r => r.RegionName == regionName))
                                {
                                    rowErrorMessage += $"<li>Region Name: {regionName} duplicate in file</li>";
                                }

                                if (string.IsNullOrEmpty(zone))
                                {
                                    rowErrorMessage += "<li>Zone Name has no content</li>";
                                }

                                if (string.IsNullOrEmpty(address))
                                {
                                    rowErrorMessage += "<li>RO Address has no content</li>";
                                }

                                if (string.IsNullOrEmpty(state))
                                {
                                    rowErrorMessage += "<li>State has no content</li>";
                                }

                                if (string.IsNullOrEmpty(pincode))
                                {
                                    rowErrorMessage += "<li>Pincode has no content</li>";
                                }

                                if (string.IsNullOrEmpty(region))
                                {
                                    rowErrorMessage += "<li>Region has no content</li>";
                                }

                                if (!string.IsNullOrEmpty(rowErrorMessage))
                                {
                                    rowErrorMessage = $"<ul>Row {index} has error.<br>{rowErrorMessage}</ul>";
                                    regionFileError += rowErrorMessage;
                                    index++;
                                    continue;
                                }

                                regions.Add(new Region
                                {
                                    RegionName = regionName,
                                    SystemRoId = roid,
                                    Address = new Address
                                    {
                                        AddressDetail = address,
                                        PinCode = pincode,
                                        Region = region,
                                        State = state,
                                        Zone = zone
                                    }
                                });
                            }
                            index++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                regionFileError = "Please upload valid content for Regions!";
            }

            _fileService.DeleteFile(path);

            if (!string.IsNullOrEmpty(regionFileError))
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status400BadRequest, Message = regionFileError };
            }

            return await _regionService.CreateRegions(regions);
        }

        public async Task<IActionResult> GetRegions()
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
                filters.regionName = searchValue;
                filters.systemRoId = searchValue;
                
                var regionResult = await _regionService.GetRegionPaged(
                    new Pagination
                    {
                        PageNumber = currentPage,
                        PageSize = pageSize,
                        SortOrderBy = sortColumnDirection,
                        SortOrderColumn = sortColumn,
                        Filters = filters
                    });
                var regionPaged = regionResult.Data as PagedList;
                recordsTotal = regionPaged.TotalCount;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = regionPaged.Data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool AuthorizeUser()
        {
            return _userProviderService.SystemAdmin();
        }
    }
}
