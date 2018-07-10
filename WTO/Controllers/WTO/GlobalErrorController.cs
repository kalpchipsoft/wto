using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using BusinessObjects;
using BusinessService;
using BusinessObjects.Notification;
using BusinessService.Notification;


namespace WTO.Controllers.WTO
{
    public class GlobalErrorController : Controller
    {
        GlobalErrorBussinessServices obj = new GlobalErrorBussinessServices();
        // GET: GlobalError
        public ActionResult Index()
        {
            //throw new Exception("something went wrong");
            return View();
        }

        [HandleError(ExceptionType = typeof(Exception), View = "~/Views/GlobalError/Index.cshtml")]
        public ActionResult Error(string excepMsg, string source)
        {
            obj.GlobalError(excepMsg, source);
            try
            {
                if (excepMsg == "")

                {
                    Response.Redirect("~/Views/GlobalError/Index.cshtml");
                }
            }

            catch
            {

                Response.Redirect("~/Views/GlobalError/Index.cshtml");

            }
            return View("~/Views/GlobalError/Index.cshtml");
        }


    }
}