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
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IBranchService _branchService;
        private readonly IRoleService _roleService;
        private readonly IRoleTypeService _roleTypeService;
        private readonly IFileService _fileService;
        private readonly IUserProviderService _userProviderService;

        public UserController(
           IUserService userService,
           IBranchService branchService,
           IRoleService roleService,
           IRoleTypeService roleTypeService,
           IFileService fileService,
           IUserProviderService userProviderService

        )
        {
            _userService = userService;
            _branchService = branchService;
            _roleService = roleService;
            _roleTypeService = roleTypeService;
            _fileService = fileService;
            _userProviderService = userProviderService;
        }

        public async Task<IActionResult> Index()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            return View();
        }

        public async Task<IActionResult> Manage(long id = 0)
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            var model = new User();
            if (id > 0)
            {
                var userResult = await _userService.GetUserById(id);
                if (userResult.Success)
                {
                    model = userResult.Data;
                    model.OldPassword = model.Password;
                    model.SelectedRoles = model.UserRoles.Select(u => u.RoleId).ToArray();
                    model.UserRoles = await SetUserRoles(model.RoleTypeId);
                }
            }

            await SetComboBoxes();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(User model)
        {
            if (model.SelectedRoles == null || model.SelectedRoles.Length == 0)
            {
                SetNotification("Please select Role", NotificationTypes.Error, "User");
                await SetComboBoxes();
                model.UserRoles = await SetUserRoles(model.RoleTypeId);
                return View(model);
            }

            if (string.IsNullOrEmpty(model.Password))
            {
                model.Password = string.Empty;
            }

            ResponseModel userResult;
            if (model.UserId == 0)
            {
                userResult = await _userService.CreateUser(model);
            }
            else
            {
                if (!model.IsUpdatePassword)
                {
                    model.OldPassword = model.Password;
                }
                userResult = await _userService.UpdateUser(model);
            }

            if (userResult.Success)
            {
                SetNotification($"User saved successfully!", NotificationTypes.Success, "User");
                return RedirectToAction("Index", "User");
            }

            SetNotification(userResult.Message, NotificationTypes.Error, "User");
            await SetComboBoxes();
            model.UserRoles = await SetUserRoles(model.RoleTypeId);
            return View(model);
        }


        [HttpPost]
        public async Task<ResponseModel> UploadUsers(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return new ResponseModel { Success = false, Message = "Please select the file!", StatusCode = StatusCodes.Status400BadRequest };

            var path = await _fileService.CreateFile(file);

            var users = new List<User>();
            var existUsers = (await _userService.GetAllUser()).Data as List<User>;
            var branches = await _branchService.GetDropdwon();
            var userFileError = string.Empty;

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        Random random = new Random(); // Temporary to set Roles

                        int index = 0;
                        int staffIndex = 0;
                        int nameIndex = 1;
                        int branchCodeIndex = 2;
                        int branchNameIndex = 3;
                        int regionNameIndex = 4;
                        int roleTypeIndex = 5;
                        int roleIndex = 6;
                        int mobileIndex = 7;
                        int emailIndex = 8;

                        while (reader.Read()) //Each row of the file
                        {
                            if (index == 0)
                            {
                            }
                            else
                            {
                                var staffId = reader.GetValue(staffIndex)?.ToString().Trim() ?? "";
                                var fullName = reader.GetValue(nameIndex)?.ToString().Trim() ?? "";
                                var branchCode = reader.GetValue(branchCodeIndex)?.ToString().Trim() ?? "";
                                var branchName = reader.GetValue(branchNameIndex)?.ToString().Trim() ?? "";
                                var regionName = reader.GetValue(regionNameIndex)?.ToString().Trim() ?? "";
                                var roleType = reader.GetValue(roleTypeIndex)?.ToString().Trim() ?? "";
                                var role = reader.GetValue(roleIndex)?.ToString().Trim() ?? "";
                                var mobile = reader.GetValue(mobileIndex)?.ToString().Trim() ?? "";
                                var email = reader.GetValue(emailIndex)?.ToString().Trim() ?? "";

                                if (string.IsNullOrEmpty(staffId) && string.IsNullOrEmpty(fullName) &&
                                    string.IsNullOrEmpty(branchCode) && string.IsNullOrEmpty(branchName) &&
                                    string.IsNullOrEmpty(regionName) && string.IsNullOrEmpty(mobile) &&
                                    string.IsNullOrEmpty(roleType) && string.IsNullOrEmpty(role) &&
                                    string.IsNullOrEmpty(email))
                                {
                                    continue;
                                }

                                var rowErrorMessage = string.Empty;

                                if (string.IsNullOrEmpty(staffId))
                                {
                                    rowErrorMessage += "<li>Emp Staff Id has no content</li>";
                                }
                                else if (existUsers.Any(r => r.StaffId == staffId))
                                {
                                    rowErrorMessage += "<li>Emp Staff Id already exists</li>";
                                }
                                else if (users.Any(r => r.StaffId == staffId))
                                {
                                    rowErrorMessage += $"<li>Emp Staff Id: {staffId} duplicate in file</li>";
                                }

                                if (string.IsNullOrEmpty(fullName))
                                {
                                    rowErrorMessage += "<li>Emp Legal Name has no content</li>";
                                }

                                if (string.IsNullOrEmpty(branchCode))
                                {
                                    rowErrorMessage += "<li>Branch Code has no content</li>";
                                }
                                var branch = branches.FirstOrDefault(b => b.BranchCode == branchCode);
                                if (!string.IsNullOrEmpty(branchCode) && branch == null)
                                {
                                    rowErrorMessage += "<li>Branch does not exists</li>";
                                }

                                if (string.IsNullOrEmpty(mobile))
                                {
                                    rowErrorMessage += "<li>Mobile Number has no content</li>";
                                }

                                if (!Enum.TryParse(roleType, out RoleTypes _roleType))
                                {
                                    rowErrorMessage += "<li>Role Type did not match</li>";
                                }

                                if (!Enum.TryParse(role, out Roles _role))
                                {
                                    rowErrorMessage += "<li>Role did not match</li>";
                                }

                                if (!string.IsNullOrEmpty(rowErrorMessage))
                                {
                                    rowErrorMessage = $"<ul>Row {index} has error.<br>{rowErrorMessage}</ul>";
                                    userFileError += rowErrorMessage;
                                    index++;
                                    continue;
                                }

                                users.Add(new User
                                {
                                    StaffId = staffId,
                                    FullName = fullName,
                                    Email = email,
                                    MobileNo = mobile,
                                    BranchId = branch.Id,
                                    RoleTypeId = _roleType.GetHashCode(),
                                    Password = _role == Roles.IBLCPU || _role == Roles.SystemAdmin ? "fsZr1MgSqWQ90S2OpSK5Nv8tos4OJGicLwazZC9P+Vc=" : "", //Test123
                                    Salt = _role == Roles.IBLCPU || _role == Roles.SystemAdmin ? "i68Y2pfIN+P3HA==" : "",
                                    UserRoles = new List<UserRole> { new UserRole { RoleId = _role.GetHashCode() } }

                                });
                            }
                            index++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                userFileError = "Please upload valid content for Users!";
            }

            if (!string.IsNullOrEmpty(userFileError))
            {
                return new ResponseModel { Success = false, StatusCode = StatusCodes.Status400BadRequest, Message = userFileError };
            }

            _fileService.DeleteFile(path);

            return await _userService.CreateUsers(users);
        }

        public async Task<IActionResult> GetRoles(long roleTypeId)
        {
            return Ok(await _roleService.GetDropdwon(roleTypeId: roleTypeId));
        }

        public async Task<IActionResult> GetUsers()
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
                filters.fullName = searchValue;
                filters.staffId = searchValue;

                var userResult = await _userService.GetUserPaged(
                    new Pagination
                    {
                        PageNumber = currentPage,
                        PageSize = pageSize,
                        SortOrderBy = sortColumnDirection,
                        SortOrderColumn = sortColumn,
                        Filters = filters
                    });
                var userPaged = userResult.Data as PagedList;
                recordsTotal = userPaged.TotalCount;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = userPaged.Data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task SetComboBoxes()
        {
            var branches = (await _branchService.GetDropdwon())
                           .Select(b => new DropdownModel { Id = b.Id, Text = $"{b.BranchCode} - {b.Text} ", IsActive = b.IsActive }).ToList();

            ViewBag.Branches = GetSelectList(branches, "Select Branch");
            ViewBag.RoleTypes = GetSelectList(await _roleTypeService.GetDropdwon(), "Select Role Type");
        }

        private async Task<List<UserRole>> SetUserRoles(long roleTypeId)
        {
            var roles = await _roleService.GetDropdwon(roleTypeId: roleTypeId);
            var list = new List<UserRole>();
            foreach (var role in roles)
            {
                list.Add(new UserRole { RoleId = role.Id, Role = new Role { RoleName = role.Text, RoleId = role.Id } });
            }
            return list;
        }

        private bool AuthorizeUser()
        {
            return _userProviderService.SystemAdmin();
        }
    }
}
