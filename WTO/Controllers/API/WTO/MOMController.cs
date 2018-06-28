using System.Web.Http;
using BusinessObjects.MOM;
using BusinessService.MOM;
using System;

namespace WTO.Controllers.API.WTO
{
    public class MoMController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Insert(Int64 Id, AddMeeting obj)
        {
            MomBusinessService objAM = new MomBusinessService();
            return Ok(objAM.InsertMomData(Id, obj));
        }

        [HttpPost]
        public IHttpActionResult AddUpdateNotificationAction(Int64 Id, AddNotificationAction obj)
        {
            MomBusinessService objAM = new MomBusinessService();
            return Ok(objAM.InsertRemoveActions(Id, obj));
        }

        [HttpGet]
        public IHttpActionResult getNotifications(string callFor, int CountryId, string NotificationNo, string NotificationId, string SelectedNotificationId)
        {
            MomBusinessService objAM = new MomBusinessService();
            return Ok(objAM.GetNotificationList_Mom(callFor, CountryId, NotificationNo, NotificationId, SelectedNotificationId));
        }

        [HttpGet]
        public IHttpActionResult EditAction(Int64 Id)
        {
            MomBusinessService objAM = new MomBusinessService();
            return Ok(objAM.EditMeetingActions(Id));
        }

        [HttpPost]
        public IHttpActionResult UpdateMeetingDate(Int64? Id, string MeetingDate)
        {
            MomBusinessService objAM = new MomBusinessService();
            return Ok(objAM.UpdateMeetingDate(Id, MeetingDate));
        }
    }
}