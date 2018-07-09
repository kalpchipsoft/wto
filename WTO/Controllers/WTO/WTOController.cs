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
        public ActionResult WTODashboardRequestResponse(DashboardSearch obj)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                if (obj.DateFrom == null)
                    obj.DateFrom = "";
                if (obj.DateTo == null)
                    obj.DateTo = "";
                ViewBag.FromDate = obj.DateFrom;
                ViewBag.ToDate = obj.DateTo;
                DashboardBusinessService objDBS = new DashboardBusinessService();
                return View("~/Views/Partial/Dashboard/DashboardRequestResponse.cshtml", objDBS.GetNotificationCountRequestResponse(obj));
            }
            else
                return PartialView("RedirectToLogin");
        }
        public ActionResult GetDashboardAction(DashboardSearch obj)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                DashboardBusinessService objDBS = new DashboardBusinessService();
                return View("~/Views/Partial/Dashboard/DashboardAction.cshtml", objDBS.GetDashboardAction(obj));
            }
            else
                return PartialView("RedirectToLogin");
        }
        public ActionResult WTODashboardProcessingStatus(DashboardSearch obj)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                DashboardBusinessService objDBS = new DashboardBusinessService();
                return View("~/Views/Partial/Dashboard/DashboardProcessingStatus.cshtml", objDBS.GetNotificationCountProcessingStatus(obj));
            }
            else
                return PartialView("RedirectToLogin");
        }
        public ActionResult WTOGetHSCodeGraphData()
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                DashboardBusinessService obj = new DashboardBusinessService();
                DashboardSearch obj1 = new DashboardSearch();
                return View("~/Views/Partial/Dashboard/DashboardHSCode.cshtml", obj.GetHSCodeGraphData(obj1));
            }
            else
                return PartialView("RedirectToLogin");
        }
        public ActionResult WTOGetHSCodeDataByCountry()
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                DashboardBusinessService obj = new DashboardBusinessService();
                DashboardSearch obj1 = new DashboardSearch();
                return View("~/Views/Partial/Dashboard/DashboardHSCodeByCountry.cshtml", obj.GetHsCodeGraphDataCountryWise(obj1));
            }
            else
                return PartialView("RedirectToLogin");
        }
        public ActionResult GetNotificationGraphData(DashboardSearch obj1)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                DashboardBusinessService obj = new DashboardBusinessService();
                DateTime d = DateTime.Now;
                d = d.AddMonths(-1);
                obj1.DateFrom = d.ToString("dd MMM yyyy");
                return View("~/Views/Partial/Dashboard/DashboardNotificationHistory.cshtml", obj.GetNotificationGraphDataMonthly(obj1));
            }
            else
                return PartialView("RedirectToLogin");
        }

        public ActionResult NotificationStakeholderList(Search_StakeholderMailSentReceive obj)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                if (obj.PageIndex == 0)
                    obj.PageIndex = 1;
                if (obj.PageSize == 0)
                    obj.PageSize = 10;
                if (obj.FromDate == null)
                    obj.FromDate = "";
                if (obj.ToDate == null)
                    obj.ToDate = "";
                if (obj.Status == null)
                    obj.Status = "0";
                ViewBag.FromDate = obj.FromDate;
                ViewBag.ToDate = obj.ToDate;
                ViewBag.Status = obj.Status;
                NotificationListBusinessService objNBS = new NotificationListBusinessService();
                return View(objNBS.GetStakeholderMailSentResponse(obj));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult NotifyingMemberList(Search_NotificationCountries obj)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                if (obj.PageIndex == 0)
                    obj.PageIndex = 1;
                if (obj.PageSize == 0)
                    obj.PageSize = 10;
                if (obj.FromDate == null)
                    obj.FromDate = "";
                if (obj.ToDate == null)
                    obj.ToDate = "";
                if (obj.CountryName == null)
                    obj.CountryName = "";
                if (obj.Hscode == null)
                    obj.Hscode = "";
                ViewBag.FromDate = obj.FromDate;
                ViewBag.ToDate = obj.ToDate;
                ViewBag.Hscode = obj.Hscode.ToUpper() == "ALL" ? "" : obj.Hscode;
                NotificationListBusinessService objNBS = new NotificationListBusinessService();
                return View(objNBS.PageLoad_NotificationCountries(obj));
            }
            else
                return RedirectToAction("Index", "Login");
        }

    }
}
