using System;
using System.Web.Mvc;
using BusinessService.Notification;
using BusinessObjects.Notification;

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
                EditNotification NotificationDetails = new EditNotification();
                return View("~/Views/WTO/AddNotification.cshtml", obj.PageLoad_EditNotification(Id));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult GetNotificationStakeholders(Int64 Id)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                NotificationBusinessService obj = new NotificationBusinessService();
                return View("~/Views/Partial/Notification/NotificationStakeholders.cshtml", obj.GetNotificationStakeholders(Id));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult GetStakeholderConversation(Int64 NotificationId, Int64 StakeholderId)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                NotificationBusinessService obj = new NotificationBusinessService();
                return View("~/Views/Partial/StakeHolderConversation.cshtml", obj.GetConversation(NotificationId, StakeholderId));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult GetNotificationActions(Int64 Id)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                NotificationBusinessService obj = new NotificationBusinessService();
                return View("~/Views/Partial/Notification/NotificationActions.cshtml", obj.GetNotificationActions(Id));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult GetNotificationMails(Int64 Id)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                NotificationBusinessService obj = new NotificationBusinessService();
                return View("~/Views/Partial/Notification/NotificationMails.cshtml", obj.GetNotificationMails(Id));
            }
            else
                return RedirectToAction("Index", "Login");
        }
    }
}