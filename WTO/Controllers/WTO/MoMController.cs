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
        public ActionResult Add(Int64 Id)
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                MomBusinessService obj = new MomBusinessService();
                return View(obj.GetNotificationList_Mom(Id));
            }
            else
                return RedirectToAction("Index", "Login");
        }

        // POST: MoM/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: MoM/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MoM/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
