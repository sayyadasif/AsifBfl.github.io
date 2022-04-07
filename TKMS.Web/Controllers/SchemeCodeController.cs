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
    public class SchemeCodeController : BaseController
    {
        private readonly ISchemeCodeService _schemeCodeService;
        private readonly IC5CodeService _c5CodeService;
        private readonly IUserProviderService _userProviderService;

        public SchemeCodeController(
           ISchemeCodeService schemeCodeService,
           IC5CodeService c5CodeService,
           IUserProviderService userProviderService

        )
        {
            _schemeCodeService = schemeCodeService;
            _c5CodeService = c5CodeService;
            _userProviderService = userProviderService;
        }

        public async Task<IActionResult> Index()
        {
            if (!AuthorizeSchemeCode()) { return AccessDeniedView(); }

            return View();
        }

        public async Task<IActionResult> Manage(long id = 0)
        {
            if (!AuthorizeSchemeCode()) { return AccessDeniedView(); }

            var model = new SchemeCode();
            if (id > 0)
            {
                var schemeCodeResult = await _schemeCodeService.GetSchemeCodeById(id);
                if (schemeCodeResult.Success)
                {
                    model = schemeCodeResult.Data;
                    model.SelectedC5Codes = model.SchemeC5Codes.Select(s => s.C5CodeId).ToArray();
                }
            }

            await SetComboBoxes();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(SchemeCode model)
        {
            if (model.SelectedC5Codes == null || model.SelectedC5Codes.Length == 0)
            {
                SetNotification("Please select C5 Code", NotificationTypes.Error, "Scheme Code");
                await SetComboBoxes();
                return View(model);
            }


            ResponseModel schemeCodeResult;
            if (model.SchemeCodeId == 0)
            {
                schemeCodeResult = await _schemeCodeService.CreateSchemeCode(model);
            }
            else
            {
                schemeCodeResult = await _schemeCodeService.UpdateSchemeCode(model);
            }

            if (schemeCodeResult.Success)
            {
                SetNotification($"Scheme Code saved successfully!", NotificationTypes.Success, "Scheme Code");
                return RedirectToAction("Index", "SchemeCode");
            }

            SetNotification(schemeCodeResult.Message, NotificationTypes.Error, "Scheme Code");
            await SetComboBoxes();
            return View(model);
        }

        public async Task<IActionResult> GetSchemeCodes()
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
                filters.schemeCodeName = searchValue;

                var schemeCodeResult = await _schemeCodeService.GetSchemeCodePaged(
                    new Pagination
                    {
                        PageNumber = currentPage,
                        PageSize = pageSize,
                        SortOrderBy = sortColumnDirection,
                        SortOrderColumn = sortColumn,
                        Filters = filters
                    });
                var schemeCodePaged = schemeCodeResult.Data as PagedList;
                recordsTotal = schemeCodePaged.TotalCount;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = schemeCodePaged.Data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task SetComboBoxes()
        {
            ViewBag.C5Codes = await _c5CodeService.GetDropdwon();
        }

        private bool AuthorizeSchemeCode()
        {
            return _userProviderService.SystemAdmin();
        }
    }
}
