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
    public class C5CodeController : BaseController
    {
        private readonly IUserProviderService _userProviderService;
        private readonly IC5CodeService _c5CodeService;
        private readonly ICardTypeService _cardTypeService;

        public C5CodeController(
           IUserProviderService userProviderService,
           IC5CodeService c5CodeService,
           ICardTypeService cardTypeService
        )
        {
            _userProviderService = userProviderService;
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

            var model = new C5Code();
            if (id > 0)
            {
                var c5CodeResult = await _c5CodeService.GetC5CodeById(id);
                if (c5CodeResult.Success)
                {
                    model = c5CodeResult.Data;
                }
            }

            await SetComboBoxes();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(C5Code model)
        {
            if (!ModelState.IsValid)
            {
                SetNotification("Please enter valid form detail", NotificationTypes.Error, "C5Code");
                await SetComboBoxes();
                return View(model);
            }

            ResponseModel c5CodeResult;
            if (model.C5CodeId == 0)
            {
                c5CodeResult = await _c5CodeService.CreateC5Code(model);
            }
            else
            {
                c5CodeResult = await _c5CodeService.UpdateC5Code(model);
            }

            if (c5CodeResult.Success)
            {
                SetNotification($"C5Code saved successfully!", NotificationTypes.Success, "C5Code");
                await SetComboBoxes();
                return RedirectToAction("Index", "C5Code");
            }

            SetNotification(c5CodeResult.Message, NotificationTypes.Error, "C5Code");
            await SetComboBoxes();
            return View(model);
        }

        public async Task<IActionResult> GetC5Codes()
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
                filters.c5CodeName = searchValue;

                var c5CodeResult = await _c5CodeService.GetC5CodePaged(
                    new Pagination
                    {
                        PageNumber = currentPage,
                        PageSize = pageSize,
                        SortOrderBy = sortColumnDirection,
                        SortOrderColumn = sortColumn,
                        Filters = filters
                    });
                var c5CodePaged = c5CodeResult.Data as PagedList;
                recordsTotal = c5CodePaged.TotalCount;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = c5CodePaged.Data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task SetComboBoxes()
        {
            ViewBag.CardTypes = GetSelectList(await _cardTypeService.GetDropdwon(), "Select Card Type");
        }

        private bool AuthorizeUser()
        {
            return _userProviderService.SystemAdmin();
        }
    }
}
