using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessObjects.Notification;
using BusinessService.Notification;

namespace WTO.Controllers.WTO
{
    public class AddNotificationController : Controller
    {
        // GET: AddNotification
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Edit_Notification(Int64 Id)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                NotificationBusinessService obj = new NotificationBusinessService();
                return View("~/Views/WTO/AddNotification.cshtml", obj.PageLoad_EditNotification(Id));
            }
            else
                return RedirectToAction("Index", "Login");
        }
    }
}