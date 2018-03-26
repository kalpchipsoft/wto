using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessObjects.ManageAccess;
using BusinessService.ManageAccess;

namespace WTO.Controllers.WTO
{
    public class StakeHolderController : Controller
    {
        // GET: StakeHolder
        public ActionResult Index()
        {
            return View();
        }

    }
}