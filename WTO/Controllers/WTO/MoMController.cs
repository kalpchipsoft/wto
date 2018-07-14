using BusinessObjects.MOM;
using BusinessService.MOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WTO.Controllers.WTO
{
    public class MoMController : Controller
    {
        // GET: MoM
        public ActionResult Index()
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                MomBusinessService obj = new MomBusinessService();
                return View(obj.GetMOMListData());
            }
            else
                return RedirectToAction("Index", "Login");
        }

        // GET: MoM/Details/5
        public ActionResult Add(Search_MoM objS)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                MomBusinessService obj = new MomBusinessService();
                return View(obj.GetNotificationList_Mom(objS));
            }
            else
                return RedirectToAction("Index", "Login");
        }
        // POST: MoM/Edit/5
        public ActionResult Edit(Nullable<Int64> Id, Search_MoM objS)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                ViewBag.Id = Id;

                if (Id == 0)
                    Id = null;
                MomBusinessService obj = new MomBusinessService();
                return View(obj.EditMoM(Id, objS));
            }
            else
                return RedirectToAction("Index", "Login");
        }
        public ActionResult GetMOMSummary(Int64 Id)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                //ViewBag.Note = Note;
                MomBusinessService objAM = new MomBusinessService();
                return PartialView("~/Views/Partial/MOMSummary.cshtml", objAM.MeetingSummary(Id));
            }
            else
                return PartialView("RedirectToLogin");
        }

    }
}
