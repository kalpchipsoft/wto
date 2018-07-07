using System.Web.Http;
using BusinessObjects.Notification;
using BusinessService.Notification;

namespace WTO.Controllers.API.WTO
{
    public class NotificationListController : ApiController
    {
        [HttpPost]
        public IHttpActionResult GetNotificationsList(Search_Notification obj)
        {
            if (obj.PageIndex == 0)
                obj.PageIndex = 1;
            if (obj.PageSize == 0)
                obj.PageSize = 10;
            NotificationListBusinessService objBS = new NotificationListBusinessService();
            return Ok(objBS.GetNotifications(obj));
        }
        [HttpPost]
        public IHttpActionResult GetNotificationCountryList(Search_NotificationCountries obj)
        {
            if (obj.PageIndex == 0)
                obj.PageIndex = 1;
            if (obj.PageSize == 0)
                obj.PageSize = 10;
            NotificationListBusinessService objBS = new NotificationListBusinessService();
            return Ok(objBS.GetNotificationCountries(obj));
        }
        [HttpPost]
        public IHttpActionResult GetStakeholderMailSentResponse(Search_StakeholderMailSentReceive obj)
        {
            if (obj.PageIndex == 0)
                obj.PageIndex = 1;
            if (obj.PageSize == 0)
                obj.PageSize = 10;
            NotificationListBusinessService objBS = new NotificationListBusinessService();
            return Ok(objBS.GetStakeholderMailSentResponse(obj));
        }

    }
}
