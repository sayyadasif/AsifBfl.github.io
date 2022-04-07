using Core.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Enums;

namespace TKMS.Web.Controllers
{
    [ResponseCache(Location = ResponseCacheLocation.None, Duration = 0, NoStore = true)]
    [Authorize]

    public class BaseController : Controller
    {
        protected ActionResult AccessDeniedView()
        {
            return RedirectToAction("AccessDenied", "Home");
        }

        protected void SetNotification(string message, NotificationTypes type, string title)
        {
            TempData["Notification"] = JsonConvert.SerializeObject(new AlertNotificationModel
            {
                Message = message,
                Title = title,
                Type = type
            });
        }

        public List<SelectListItem> GetSelectList(List<DropdownModel> items, string defaultText = "", List<long> selected = null)
        {
            var list = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(defaultText))
            {
                list.Add(new SelectListItem()
                {
                    Text = defaultText,
                    Value = "",
                    Selected = true
                });
            }

            foreach (var item in items)
            {
                list.Add(new SelectListItem()
                {
                    Text = item.Text,
                    Value = item.Id.ToString(),
                    Selected = selected != null ? selected.IndexOf(item.Id) != -1 : false
                });
            }

            return list;
        }
    }
}
