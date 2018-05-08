using System;
using System.Web.Mvc;
using BusinessObjects;
using BusinessObjects.Masters;
using BusinessServices;
using BusinessObjects.Notification;

namespace WTO.Controllers
{
    public class PartialController : Controller
    {
        public ActionResult UserInfo()
        {
            LoginBusinessService obj = new LoginBusinessService();
            UserInfo objUI = new UserInfo();
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                objUI = obj.GetUserDetails(Convert.ToInt64(Session["UserId"]));
            }
            return View(objUI);
        }

        public ActionResult Pagination(int TotalItems = 10, int CurrentPage = 1)
        {
            PagerTotalCount objPager = new PagerTotalCount();
            objPager.TotalCount = Convert.ToString(TotalItems);
            Pager obj = new Pager(TotalItems, CurrentPage, 10);
            objPager.Pager = obj;
            return View(objPager);
        }
    }
}