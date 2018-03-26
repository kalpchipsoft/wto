using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessObjects.Notification;
using BusinessService.Notification;

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
                return View(obj.GetPageLoadCount(objModel));
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult AddMOMList()
        {
            return View();
        }
        public ActionResult AddMOM()
        {
            return View();
        }
        public ActionResult ManageAccess()
        {
            return View();
        }
    }
}
