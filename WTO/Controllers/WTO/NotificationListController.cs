using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessObjects.Notification;
using BusinessService.Notification;

namespace WTO.Controllers.WTO
{
    public class NotificationListController : Controller
    {
        // GET: NotificationList
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetNotificationsMasters(string CallFrom)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                NotificationListBusinessService objBS = new NotificationListBusinessService();
                return View("~/Views/WTO/NotificationList.cshtml", objBS.PageLoad_NotificationsMasters(CallFrom));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        //public ActionResult GetNotificationsList(GetNotificationList obj)
        //{
        //    if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
        //    {
        //        if (obj.PageIndex == 0)
        //            obj.PageIndex = 1;
        //        if (obj.PageSize == 0)
        //            obj.PageSize = 10;
        //        NotificationListBusinessService objBS = new NotificationListBusinessService();
        //        return View("~/Views/Partial/Partial_NotificationList.cshtml", objBS.GetNotifications(obj));
        //    }
        //    else
        //        return RedirectToAction("Index", "Login");
        //}
    }
}