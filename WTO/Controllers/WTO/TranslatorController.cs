using BusinessObjects.Translator;
using BusinessService.Translator;
using System;
using System.Web.Mvc;

namespace WTO.Controllers.WTO
{
    public class TranslatorController : Controller
    {
        // GET: Translator
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Validate(Login model)
        {
            if (ModelState.IsValid)
            {
                TranslatorBusinessService obj = new TranslatorBusinessService();
                LoginResult objR = obj.ValidateTranslator(model);
                if (objR.StatusType == BusinessObjects.StatusType.SUCCESS)
                {
                    int result = obj.Login(objR.TranslatorId);
                    if (result > 0)
                    {
                        Session["TranslatorId"] = objR.TranslatorId;
                        Session["TranslatorName"] = objR.TranslatorName;
                        if (objR.LoginCount > 0)
                            return RedirectToAction("List", "Translator");
                        else
                            return RedirectToAction("List", "Translator", new { Id = objR.LoginCount });
                    }
                }
                else
                    ModelState.AddModelError("", objR.Message);
            }
            return View("Login", model);
        }

        public ActionResult List(SearchDocument objS)
        {
            if (Convert.ToString(Session["TranslatorId"]).Trim().Length > 0)
            {
                TranslatorBusinessService obj = new TranslatorBusinessService();
                if (objS.TranslatorId == 0)
                    objS.TranslatorId = Convert.ToInt64(Session["TranslatorId"]);
                return View(obj.Documents(objS));
            }
            else
                return View("Login");
        }

        public ActionResult Logout()
        {
            Session["TranslatorId"] = null;
            return View("Login");
        }
    }
}