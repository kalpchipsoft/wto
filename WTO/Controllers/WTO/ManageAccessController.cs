using System;
using System.Web.Mvc;
using BusinessService.ManageAccess;

namespace WTO.Controllers.WTO
{
    public class ManageAccessController : Controller
    {
        // GET: ManageAccess
        public ActionResult Index()
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                return View("~/Views/WTO/ManageAccess.cshtml");
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult GetUserList()
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                UserBusinessService obj = new UserBusinessService();
                return View("~/Views/Partial/ManageAccess/User.cshtml", obj.UsersList());
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult GetCountryList()
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                CountryBusinessService obj = new CountryBusinessService();
                return View("~/Views/Partial/ManageAccess/Country.cshtml", obj.CountriesList());
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult GetStakeHolderList()
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                StakeHolderBusinessService obj = new StakeHolderBusinessService();
                return View("~/Views/Partial/ManageAccess/StakeHolder.cshtml", obj.StakeHoldersList());
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult GetTranslatorList()
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                TranslatorBusinessService obj = new TranslatorBusinessService();
                return View("~/Views/Partial/ManageAccess/Translator.cshtml", obj.TranslatorsList());
            }
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult GetTemplateList()
        {
            if (Convert.ToString(Session["UserId"]).Trim().Length > 0)
            {
                TemplateBussinessService obj = new TemplateBussinessService();
                return View("~/Views/Partial/ManageAccess/Template.cshtml", obj.TemplateList());
            }
            else
                return RedirectToAction("Index", "Login");
        }
    }
}