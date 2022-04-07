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
using TKMS.Abstraction.Models;
using TKMS.Service.Interfaces;
using TKMS.Web.Models;

namespace TKMS.Web.Controllers
{
    public class NotificationController : BaseController
    {
        private readonly INotificationService _notificationService;
        private readonly IUserProviderService _userProviderService;

        public NotificationController(
           INotificationService notificationService,
           IUserProviderService userProviderService
        )
        {
            _notificationService = notificationService;
            _userProviderService = userProviderService;
        }

        public async Task<IActionResult> Index()
        {
            if (!AuthorizeUser()) { return AccessDeniedView(); }

            return View(await _notificationService.GetUserNotifications(null));
        }


        public async Task<IActionResult> GetNotifications()
        {
            return PartialView("_Notification", await _notificationService.GetUserNotifications());
        }

        public async Task<IActionResult> GenerateNotifactions()
        {
            await _notificationService.GenerateNotifactions();
            return Ok();
        }

        private bool AuthorizeUser()
        {
            return _userProviderService.BM() || _userProviderService.BCM() ||
                   _userProviderService.ROKitManagement() || _userProviderService.Staff();
        }
    }
}
