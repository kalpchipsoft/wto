using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessServices;
using BusinessObjects;

namespace WTO.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult Validate(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                LoginBusinessService obj = new LoginBusinessService();
                LoginResult objR = obj.ValidateUser(model);
                if (objR.StatusType == StatusType.SUCCESS)
                {
                    Session["UserId"] = objR.UserId;
                    return RedirectToAction("WTODashboard", "WTO");
                }
                else
                    ModelState.AddModelError("", objR.Message);
            }
            return View("Index", model);
        }

        public ActionResult LogOut()
        {
            Session["UserId"] = null;
            return View("Index");
        }
    }
}
