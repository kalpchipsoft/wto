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

        [HttpPost]
        public IHttpActionResult getNotifications(Search_MoM obj)
        {
            MomBusinessService objAM = new MomBusinessService();
            return Ok(objAM.GetNotificationList_Mom(obj));
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

        [HttpPost]
        public IHttpActionResult Edit(Nullable<Int64> Id, Search_MoM objS)
        {
            if (Id == 0)
                Id = null;
            MomBusinessService obj = new MomBusinessService();
            return Ok(obj.EditMoM(Id, objS));
        }

        [HttpPost]
        public IHttpActionResult EndMeeting(Int64? Id)
        {
            MomBusinessService objAM = new MomBusinessService();
            return Ok(objAM.EndMeeting(Id));
        }

        [HttpPost]
        public IHttpActionResult CheckIfOpenMeetingExists(string date)
        {
            MomBusinessService objAM = new MomBusinessService();
            return Ok(objAM.CheckIfOpenMeetingExists(date));
        }
    }
}