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
        public ActionResult WTODashboard()
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                Dashboard_PendingCounts objP = new Dashboard_PendingCounts();
                return View(objP);
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult ManageAccess()
        {
            return View();
        }
        public ActionResult NotificationView(Int64 Id, Nullable<Int64> MomId, string CallFor, Nullable<Int64> R, string D, Nullable<Int64> Total)
        {
            NotificationBusinessService obj = new NotificationBusinessService();
            return View(obj.GetViewNotificationDetails(Id, MomId, CallFor, R, D, Total));
        }
        public ActionResult NotificationSummary(Nullable<Int64> MomId, string CallFor, Nullable<Int64> R, string D, Nullable<Int64> Total)
        {
            NotificationBusinessService obj = new NotificationBusinessService();
            ViewNotification objViewDetails = new ViewNotification();
            objViewDetails = obj.NotificationSummary(MomId, CallFor, R, D);
            if (objViewDetails != null)
            {
                return RedirectToAction("NotificationView", new { Id = objViewDetails.NotificationId, MomId = objViewDetails.MomId, CallFor = objViewDetails.CallFor, R = objViewDetails.RowNum, D = D, Total = Total });
            }
            else
            {
                return View();
            }
        }
        public ActionResult WTODashboardRequestResponse()
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                DashboardBusinessService objDBS = new DashboardBusinessService();
                DashboardSearch obj = new DashboardSearch();
                return View("~/Views/Partial/Dashboard/DashboardRequestResponse.cshtml", objDBS.GetNotificationCountRequestResponse(obj));
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult GetDashboardAction(DashboardSearch obj1)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                DashboardBusinessService obj = new DashboardBusinessService();
                return View("~/Views/Partial/Dashboard/DashboardAction.cshtml", obj.GetDashboardAction(obj1));
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult WTODashboardProcessingStatus()
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                DashboardBusinessService objDBS = new DashboardBusinessService();
                Dashboard obj = new Dashboard();
                obj.UserId = Convert.ToInt64(Session["UserId"]);
                return View("~/Views/Partial/Dashboard/DashboardProcessingStatus.cshtml", objDBS.GetNotificationCountProcessingStatus(obj));
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult WTOGetHSCodeGraphData()
        {
            DashboardBusinessService obj = new DashboardBusinessService();
            DashboardSearch obj1 = new DashboardSearch();
            return View("~/Views/Partial/Dashboard/DashboardHSCode.cshtml", obj.GetHSCodeGraphData(obj1));
        }
        public ActionResult WTOGetHSCodeDataByCountry()
        {
            DashboardBusinessService obj = new DashboardBusinessService();
            DashboardSearch obj1 = new DashboardSearch();
            return View("~/Views/Partial/Dashboard/DashboardHSCodeByCountry.cshtml", obj.GetHsCodeGraphDataCountryWise(obj1));
        }
        public ActionResult GetNotificationGraphData(DashboardSearch obj1) 
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                DashboardBusinessService obj = new DashboardBusinessService();
                obj1.DateFrom = System.DateTime.Now.ToString("dd MMM yyyy");
                return View("~/Views/Partial/Dashboard/DashboardNotificationHistory.cshtml", obj.GetNotificationGraphDataMonthly(obj1));
            }
            else
                return RedirectToAction("Index", "Login");
        }
    }
}
