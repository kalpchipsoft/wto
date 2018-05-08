using System;
using System.Web.Mvc;
using BusinessObjects.Notification;
using BusinessService.Notification;
using BusinessObjects.MOM;
using BusinessService.MOM;

namespace WTO.Controllers
{
    public class WTOController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddNotification()
        {
            return View();
        }
        public ActionResult NotificationList()
        {
            return View();
        }
        public ActionResult NotificationListView()
        {
            return View();
        }
        public ActionResult WTODashboard()
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                Dashboard objModel = new Dashboard();
                objModel.UserId = Convert.ToInt64(Session["UserId"]);
                DashboardBusinessService obj = new DashboardBusinessService();
                return View(obj.GetDashboard_PendingCounts(objModel));
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult ManageAccess()
        {
            return View();
        }
    }
}
