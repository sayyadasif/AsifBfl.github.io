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
    public class IblBranchController : BaseController
    {
        private readonly IUserProviderService _userProviderService;
        private readonly IIblBranchService _iblBranchService;
        private readonly IBranchService _branchService;
        private readonly IFileService _fileService;

        public IblBranchController(
           IUserProviderService userProviderService,
           IIblBranchService iblBranchService,
           IBranchService branchService,
           IFileService fileService
        )
        {
            _userProviderService = userProviderService;
            _iblBranchService = iblBranchService;
            _branchService = branchService;
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

            var model = new IblBranch();
            if (id > 0)
            {
                var iblBranchResult = await _iblBranchService.GetIblBranchById(id);
                if (iblBranchResult.Success)
                {
                    model = iblBranchResult.Data;
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(IblBranch model)
        {
            if (!ModelState.IsValid)
            {
                SetNotification("Please enter valid form detail", NotificationTypes.Error, "IBL Branch");
                return View(model);
            }

            ResponseModel iblBranchResult;
            if (model.IblBranchId == 0)
            {
                iblBranchResult = await _iblBranchService.CreateIblBranch(model);
            }
            else
            {
                iblBranchResult = await _iblBranchService.UpdateIblBranch(model);
            }

            if (iblBranchResult.Success)
            {
                SetNotification($"IBL Branch saved successfully!", NotificationTypes.Success, "IBL Branch");
                return RedirectToAction("Index", "IblBranch");
            }

            SetNotification(iblBranchResult.Message, NotificationTypes.Error, "IBL Branch");
            return View(model);
        }

        [HttpPost]
        public async Task<ResponseModel> UploadIblBranches(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new ResponseModel { Success = false, Message = "Please select the file!", StatusCode = StatusCodes.Status400BadRequest };

            var path = await _fileService.CreateFile(file);

            var iblBranches = new List<IblBranch>();
            var existIblBranches = await _iblBranchService.GetDropdwon();
            var iblBranchFileError = string.Empty;

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        int index = 0;
                        int iblBranchCodeIndex = 0;
                        int iblBranchnameIndex = 1;

                        while (reader.Read()) //Each row of the file
                        {
                            if (index == 0)
                            {
                            }
                            else
                            {
                                var iblBranchCode = reader.GetValue(iblBranchCodeIndex)?.ToString().Trim() ?? "";
                                var iblBranchName = reader.GetValue(iblBranchnameIndex)?.ToString().Trim() ?? "";

                                if (string.IsNullOrEmpty(iblBranchCode) && string.IsNullOrEmpty(iblBranchName))
                                {
                                    continue;
                                }

                                var rowErrorMessage = string.Empty;

                                if (string.IsNullOrEmpty(iblBranchCode))
                                {
                                    rowErrorMessage += "<li>IBL Branch Code has no content</li>";
                                }
                                else if (existIblBranches.Any(r => r.IblBranchCode == iblBranchCode))
                                {
                                    rowErrorMessage += "<li>IBL Branch Code already exists</li>";
                                }
                                else if (iblBranches.Any(r => r.IblBranchCode == iblBranchCode))
                                {
                                    rowErrorMessage += $"<li>IBL Branch Code {iblBranchCode} duplicate in file</li>";
                                }

                                if (string.IsNullOrEmpty(iblBranchName))
                                {
                                    rowErrorMessage += "<li>IBL Branch Name has no content</li>";
                                }
                                else if (existIblBranches.Any(r => r.Text == iblBranchName))
                                {
                                    rowErrorMessage += "<li>IBL Branch Name already exists</li>";
                                }
                                else if (iblBranches.Any(r => r.IblBranchName == iblBranchName))
                                {
                                    rowErrorMessage += $"<li>IBL Branch Name: {iblBranchName} duplicate in file</li>";
                                }

                                if (!string.IsNullOrEmpty(rowErrorMessage))
                                {
                                    rowErrorMessage = $"<ul>Row {index} has error.<br>{rowErrorMessage}</ul>";
                                    iblBranchFileError += rowErrorMessage;
                                    index++;
                                    continue;
                                }

                                iblBranches.Add(new IblBranch
                                {
                                    IblBranchName = iblBranchName,
                                    IblBranchCode = iblBranchCode,
                                });
                            }
                            index++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                iblBranchFileError = "Please upload valid content for IBL Branches!";
            }

            _fileService.DeleteFile(path);

            if (!string.IsNullOrEmpty(iblBranchFileError))
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status400BadRequest, Message = iblBranchFileError };
            }

            return await _iblBranchService.CreateIblBranches(iblBranches);
        }

        [HttpPost]
        public async Task<ResponseModel> UploadIblBranchMapping(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new ResponseModel { Success = false, Message = "Please select the file!", StatusCode = StatusCodes.Status400BadRequest };

            var path = await _fileService.CreateFile(file);

            var iblBranchMapping = new List<IblBranchMapping>();
            var iblBranches = await _iblBranchService.GetDropdwon();
            var bfilBranches = await _branchService.GetDropdwon();
            var iblBranchFileError = string.Empty;

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        int index = 0;
                        int bfilBranchCodeIndex = 0;
                        int iblBranchCodeIndex = 1;
                        int bfilBranchNameIndex = 2;
                        int iblBranchNameIndex = 3;

                        while (reader.Read()) //Each row of the file
                        {
                            if (index == 0)
                            {
                            }
                            else
                            {
                                var bfilBranchCode = reader.GetValue(bfilBranchCodeIndex)?.ToString().Trim() ?? "";
                                var iblBranchCode = reader.GetValue(iblBranchCodeIndex)?.ToString().Trim() ?? "";
                                var bfilBranchname = reader.GetValue(bfilBranchNameIndex)?.ToString().Trim() ?? "";
                                var iblBranchName = reader.GetValue(iblBranchNameIndex)?.ToString().Trim() ?? "";

                                if (string.IsNullOrEmpty(bfilBranchCode) && string.IsNullOrEmpty(iblBranchCode) &&
                                    string.IsNullOrEmpty(bfilBranchname) && string.IsNullOrEmpty(iblBranchName))
                                {
                                    continue;
                                }

                                var rowErrorMessage = string.Empty;

                                var _bfilBranch = bfilBranches.SingleOrDefault(r => r.BranchCode == bfilBranchCode);

                                if (string.IsNullOrEmpty(bfilBranchCode))
                                {
                                    rowErrorMessage += "<li>BFIL Branch Code has no content</li>";
                                }
                                else if (_bfilBranch == null)
                                {
                                    rowErrorMessage += $"<li>BFIL Branch Code does not exists</li>";
                                }

                                var _iblBranch = iblBranches.SingleOrDefault(r => r.IblBranchCode == iblBranchCode);

                                if (string.IsNullOrEmpty(iblBranchCode))
                                {
                                    rowErrorMessage += "<li>IBL Branch Code has no content</li>";
                                }
                                else if (_iblBranch == null)
                                {
                                    rowErrorMessage += $"<li>IBL Branch Code does not exists</li>";
                                }

                                if (string.IsNullOrEmpty(bfilBranchname))
                                {
                                    rowErrorMessage += "<li>BFIL Branch Name has no content</li>";
                                }
                                else if (_bfilBranch != null && _bfilBranch.Text != bfilBranchname)
                                {
                                    rowErrorMessage += $"<li>BFIL Branch Name did not match</li>";
                                }

                                if (string.IsNullOrEmpty(iblBranchName))
                                {
                                    rowErrorMessage += "<li>IBL Branch Name has no content</li>";
                                }
                                else if (_iblBranch != null && _iblBranch.Text != iblBranchName)
                                {
                                    rowErrorMessage += $"<li>IBL Branch Name did not match</li>";
                                }


                                if (!string.IsNullOrEmpty(rowErrorMessage))
                                {
                                    rowErrorMessage = $"<ul>Row {index} has error.<br>{rowErrorMessage}</ul>";
                                    iblBranchFileError += rowErrorMessage;
                                    index++;
                                    continue;
                                }

                                iblBranchMapping.Add(new IblBranchMapping
                                {
                                    IblBranchId = _iblBranch.Id,
                                    BranchId = _bfilBranch.Id,
                                });
                            }
                            index++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                iblBranchFileError = "Please upload valid content for IBL Branch Mapping!";
            }

            _fileService.DeleteFile(path);

            if (!string.IsNullOrEmpty(iblBranchFileError))
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status400BadRequest, Message = iblBranchFileError };
            }

            return await _branchService.IblBranchMapping(iblBranchMapping);
        }

        public async Task<IActionResult> GetIblBranches()
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
                filters.iblBranchName = searchValue;
                filters.iblBranchCode = searchValue;

                var iblBranchResult = await _iblBranchService.GetIblBranchPaged(
                    new Pagination
                    {
                        PageNumber = currentPage,
                        PageSize = pageSize,
                        SortOrderBy = sortColumnDirection,
                        SortOrderColumn = sortColumn,
                        Filters = filters
                    });
                var iblBranchPaged = iblBranchResult.Data as PagedList;
                recordsTotal = iblBranchPaged.TotalCount;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = iblBranchPaged.Data };
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
