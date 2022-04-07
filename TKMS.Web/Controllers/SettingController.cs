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
    public class SettingController : BaseController
    {
        private readonly ISettingService _settingService;
        private readonly IUserProviderService _userProviderService;

        public SettingController(
            ISettingService settingService,
            IUserProviderService userProviderService
        )
        {
            _settingService = settingService;
            _userProviderService = userProviderService;
        }

        public async Task<IActionResult> Index()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            var settings = await _settingService.GetAllSetting(true);
            return View(settings.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Index(List<Setting> model)
        {
            var result = await _settingService.UpdateSettings(model);
            SetNotification(result.Message, result.Success ? NotificationTypes.Success : NotificationTypes.Error, "Settings");
            return View(model);
        }

        private bool AuthorizeUser()
        {
            return _userProviderService.SystemAdmin();
        }
    }
}
