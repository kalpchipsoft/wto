using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessObjects.Notification;
using BusinessService.Notification;

namespace WTO.Controllers.API.WTO
{
    public class AddUpdateNotificationController : ApiController
    {
        [HttpPost]
        public IHttpActionResult InsertUpdate_Notification(AddNotification obj)
        {
            NotificationBusinessService objAN = new NotificationBusinessService();
            return Ok(objAN.InsertUpdateNotification(obj));
        }
    }
}
