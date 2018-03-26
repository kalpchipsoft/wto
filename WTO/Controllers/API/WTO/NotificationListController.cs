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
    public class NotificationListController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetNotificationsList(GetNotificationList obj)
        {
            if (obj.PageIndex == 0)
                obj.PageIndex = 1;
            if (obj.PageSize == 0)
                obj.PageSize = 10;
            NotificationListBusinessService objBS = new NotificationListBusinessService();
            return Ok(objBS.GetNotifications(obj));
        }
    }
}
