using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WTO.Controllers.WTO
{
    public class MOMListController : Controller
    {
        // GET: MOMList
        public ActionResult Index()
        {
            return View("~/Views/WTO/AddMOMList.cshtml");
        }
    }
}