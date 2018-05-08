using System;
using System.Web.Mvc;
using BusinessService.Notification;
using BusinessObjects.Notification;

namespace WTO.Controllers.WTO
{
    public class NotificationListController : Controller
    {
        // GET: NotificationList
        public ActionResult Index(Search_Notification obj)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                NotificationListBusinessService objBS = new NotificationListBusinessService();
                return View("~/Views/WTO/NotificationList.cshtml", objBS.PageLoad_NotificationsList(obj));
            }
            else
                return RedirectToAction("Index", "Login");
        }
    }
}