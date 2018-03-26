using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WTO.Controllers.WTO
{
    public class AddMOMController : Controller
    {
        // GET: AddMOM
        public ActionResult Index()
        {
            return View("~/Views/WTO/AddMOM.cshtml");
        }
    }
}