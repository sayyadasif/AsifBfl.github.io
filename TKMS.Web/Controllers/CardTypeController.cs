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
    public class CardTypeController : BaseController
    {
        private readonly IUserProviderService _userProviderService;
        private readonly ICardTypeService _cardTypeService;

        public CardTypeController(
           IUserProviderService userProviderService,
           ICardTypeService cardTypeService
        )
        {
            _userProviderService = userProviderService;
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

            var model = new CardType();
            if (id > 0)
            {
                var cardTypeResult = await _cardTypeService.GetCardTypeById(id);
                if (cardTypeResult.Success)
                {
                    model = cardTypeResult.Data;
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Manage(CardType model)
        {
            if (!ModelState.IsValid)
            {
                SetNotification("Please enter valid form detail", NotificationTypes.Error, "Card Type");
                return View(model);
            }

            ResponseModel cardTypeResult;
            if (model.CardTypeId == 0)
            {
                cardTypeResult = await _cardTypeService.CreateCardType(model);
            }
            else
            {
                cardTypeResult = await _cardTypeService.UpdateCardType(model);
            }

            if (cardTypeResult.Success)
            {
                SetNotification($"Card Type saved successfully!", NotificationTypes.Success, "Card Type");
                return RedirectToAction("Index", "CardType");
            }

            SetNotification(cardTypeResult.Message, NotificationTypes.Error, "Card Type");
            return View(model);
        }

        public async Task<IActionResult> GetCardTypes()
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
                filters.cardTypeName = searchValue;

                var cardTypeResult = await _cardTypeService.GetCardTypePaged(
                    new Pagination
                    {
                        PageNumber = currentPage,
                        PageSize = pageSize,
                        SortOrderBy = sortColumnDirection,
                        SortOrderColumn = sortColumn,
                        Filters = filters
                    });
                var cardTypePaged = cardTypeResult.Data as PagedList;
                recordsTotal = cardTypePaged.TotalCount;
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = cardTypePaged.Data };
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
