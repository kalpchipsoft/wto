using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using BusinessObjects.Notification;
using BusinessService.Notification;
using WTO.Handler;
using System.IO;
using System.Web;
using BusinessObjects.ManageAccess;
using iTextSharp.text;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

namespace WTO.Controllers.API.WTO
{
    public class DashboardController : ApiController
    {
        [HttpPost]
        public IHttpActionResult WTOGetHSCodeData(DashboardSearch obj1)
        {
            DashboardBusinessService obj = new DashboardBusinessService();
            return Ok(obj.GetHSCodeGraphData(obj1));
        }
        [HttpPost]
        public IHttpActionResult WTOGetHSCodeDataByCountry(DashboardSearch obj1)
        {
            DashboardBusinessService obj = new DashboardBusinessService();
            return Ok(obj.GetHsCodeGraphDataCountryWise(obj1));
        }
    }
}