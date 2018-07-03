using System;
using System.Collections.Generic;
using System.Data;
using BusinessObjects.MOM;
using DataServices.WTO;

namespace BusinessService.MOM
{
    public class MomBusinessService
    {
        public NotificationMOM GetNotificationList_Mom(Search_MoM obj)
        {
            NotificationMOM NotificationMoMList = new NotificationMOM();

            MOMDataManager objDM = new MOMDataManager();

            DataSet ds = objDM.GetNotificationListForMom(obj);
            if (ds != null)
            {
                int tblIndex = -1;

                #region "Notifications List"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex] != null && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<Notification_Mom> NotificationList = new List<Notification_Mom>();
                    int i = 1;
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        Notification_Mom objNotification = new Notification_Mom();
                        objNotification.ItemNumber = i;
                        objNotification.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objNotification.Title = Convert.ToString(dr["Title"]);
                        objNotification.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        objNotification.Country = Convert.ToString(dr["Country"]);
                        objNotification.SendResponseBy = Convert.ToString(dr["SendResponseBy"]);
                        objNotification.FinalDateofComments = Convert.ToString(dr["FinalDateOfComment"]);
                        objNotification.Description = Convert.ToString(dr["Description"]);
                        objNotification.MeetingNote = Convert.ToString(dr["MeetingNote"]);
                        objNotification.NotificationGroup = Convert.ToString(dr["NotificationGroup"]);
                        i++;
                        NotificationList.Add(objNotification);
                    }
                    NotificationMoMList.Notification_MomList = NotificationList;
                }
                #endregion

                #region "Country List"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex] != null && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    int i = 1;
                    List<GetCountry> CountryList = new List<GetCountry>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        GetCountry objCountry = new GetCountry();
                        objCountry.CountryId = Convert.ToInt32(dr["CountryId"]);
                        objCountry.Country = Convert.ToString(dr["Country"]);
                        i++;
                        CountryList.Add(objCountry);
                    }
                    NotificationMoMList.CountryList = CountryList;
                }
                #endregion

                #region "Notification Process Dots Color & Tooltip Text"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex] != null && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<BusinessObjects.Notification.NotificationProcessDot> NPSList = new List<BusinessObjects.Notification.NotificationProcessDot>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        BusinessObjects.Notification.NotificationProcessDot objNPS = new BusinessObjects.Notification.NotificationProcessDot();
                        objNPS.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objNPS.ColorCode = Convert.ToString(dr["ColorCode"]);
                        objNPS.TooltipText = Convert.ToString(dr["TooltipText"]);
                        objNPS.Sequence = Convert.ToInt32(dr["Sequence"]);
                        NPSList.Add(objNPS);
                    }
                    NotificationMoMList.NotificationProcessDots = NPSList;
                }
                #endregion
            }
            return NotificationMoMList;
        }
        public Int32 InsertMomData(Int64 UserId, AddMeeting objAddMOM)
        {
            MOMDataManager objDM = new MOMDataManager();
            return objDM.InsertMeeting(UserId, objAddMOM);
        }
        public EditNotificationMeeting InsertRemoveActions(Int64 UserId, AddNotificationAction objAdd)
        {
            EditNotificationMeeting objE = new EditNotificationMeeting();
            MOMDataManager objDM = new MOMDataManager();
            DataSet ds = objDM.InsertRemoveActions(UserId, objAdd);
            if (ds != null)
            {
                int tblIndex = -1;

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        objE.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        objE.Title = Convert.ToString(dr["Title"]);
                        objE.Status = Convert.ToString(dr["Stage"]);
                        objE.MeetingNote = Convert.ToString(dr["MeetingNote"]);
                        objE.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                        objE.RetainedForNextDiscussion = Convert.ToBoolean(dr["RetainedForNextDiscussion"]);
                    }
                }

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<EditAction> ActionList = new List<EditAction>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        EditAction objNotificationAction = new EditAction();
                        objNotificationAction.NotificationActionId = Convert.ToInt64(dr["NotificationActionId"]);
                        objNotificationAction.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objNotificationAction.ActionName = Convert.ToString(dr["Action"]);
                        objNotificationAction.RequiredOn = Convert.ToString(dr["RequiredOn"]);
                        objNotificationAction.MailId = Convert.ToString(dr["MailId"]);
                        objNotificationAction.UpdatedOn = Convert.ToString(dr["UpdatedOn"]);
                        ActionList.Add(objNotificationAction);
                    }
                    objE.Actions = ActionList;
                }
            }
            return objE;
        }
        public MoMs GetMOMListData()
        {
            MoMs objMoM = new MoMs();
            MOMDataManager objDM = new MOMDataManager();
            DataSet ds = objDM.GetMOMListData();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int tblIndex = -1;

                #region "Master Actions List"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<BusinessObjects.MOM.Action> ActionList = new List<BusinessObjects.MOM.Action>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        BusinessObjects.MOM.Action objAction = new BusinessObjects.MOM.Action();
                        objAction.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objAction.ActionName = Convert.ToString(dr["Action"]);
                        ActionList.Add(objAction);
                    }
                    objMoM.Actions = ActionList;
                }
                #endregion

                #region "MoMs Notification List"
                tblIndex++;
                if (ds.Tables.Count > tblIndex)
                {
                    List<MoM> NotificationList = new List<MoM>();
                    int i = 1;
                    string hdnRelatedStakeHolder = string.Empty;
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        MoM objNotification = new MoM();
                        objNotification.ItemNumber = i;
                        objNotification.MoMId = Convert.ToInt64(dr["MomId"]);
                        objNotification.NotificationCount = Convert.ToInt32(dr["NotificationCount"]);
                        objNotification.PendingCount = Convert.ToInt32(dr["PendingCount"]);
                        objNotification.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                        i++;
                        NotificationList.Add(objNotification);
                    }
                    objMoM.MoMList = NotificationList;
                }
                #endregion

                #region "Notification Actions List"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<MoMAction> MoMActionList = new List<MoMAction>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        MoMAction objAction = new MoMAction();
                        objAction.MoMId = Convert.ToInt64(dr["MomId"]);
                        objAction.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objAction.PendingCount = Convert.ToInt64(dr["PendingCount"]);
                        objAction.TotalCount = Convert.ToInt64(dr["TotalCount"]);
                        MoMActionList.Add(objAction);
                    }
                    objMoM.MoMActionList = MoMActionList;
                }
                #endregion
            }

            return objMoM;
        }
        public EditMeeting EditMoM(Nullable<Int64> Id, Search_MoM obj)
        {
            EditMeeting objE = new EditMeeting();
            MOMDataManager objDM = new MOMDataManager();
            DataSet ds = objDM.EditMeeting(Id, obj);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int tblIndex = -1;

                #region "Master Actions List"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<BusinessObjects.MOM.Action> ActionList = new List<BusinessObjects.MOM.Action>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        BusinessObjects.MOM.Action objAction = new BusinessObjects.MOM.Action();
                        objAction.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objAction.ActionName = Convert.ToString(dr["Action"]);
                        ActionList.Add(objAction);
                    }
                    objE.Actions = ActionList;
                }
                #endregion

                #region "MoM Details"
                tblIndex++;
                if (ds.Tables.Count > tblIndex)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        objE.MoMId = Convert.ToInt64(dr["MomId"]);
                        objE.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                    }
                }
                #endregion

                #region "Notifications List"
                tblIndex++;
                if (ds.Tables.Count > tblIndex)
                {
                    List<Notification_Mom> NotificationList = new List<Notification_Mom>();
                    int i = 1;
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        Notification_Mom objNotification = new Notification_Mom();
                        objNotification.ItemNumber = i;
                        objNotification.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objNotification.Title = Convert.ToString(dr["Title"]);
                        objNotification.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        objNotification.Country = Convert.ToString(dr["Country"]);
                        objNotification.MeetingNote = Convert.ToString(dr["MeetingNote"]);
                        objNotification.IsUpdate = Convert.ToBoolean(dr["IsUpdate"]);
                        objNotification.Description = Convert.ToString(dr["Description"]);
                        objNotification.RowNum = Convert.ToInt64(dr["ROWNum"]);
                        objNotification.TotalRow = Convert.ToInt64(dr["TotalRow"]);
                        objNotification.NotificationGroup= Convert.ToString(dr["NotificationGroup"]);
                        i++;
                        NotificationList.Add(objNotification);
                    }
                    objE.Notifications = NotificationList;
                }
                #endregion

                #region "Notification Actions List"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<NotificationAction> NotificationActionList = new List<NotificationAction>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationAction objAction = new NotificationAction();
                        objAction.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objAction.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objAction.MailId = Convert.ToString(dr["MailId"]);
                        NotificationActionList.Add(objAction);
                    }
                    objE.NotificationActions = NotificationActionList;
                }
                #endregion
            }
            return objE;
        }
        public EditNotificationMeeting EditMeetingActions(Int64 Id)
        {
            EditNotificationMeeting objE = new EditNotificationMeeting();
            MOMDataManager objDM = new MOMDataManager();
            DataSet ds = objDM.EditActions(Id);
            if (ds != null)
            {
                int tblIndex = -1;

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        objE.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        objE.Title = Convert.ToString(dr["Title"]);
                        objE.Status = Convert.ToString(dr["Stage"]);
                        objE.MeetingNote = Convert.ToString(dr["MeetingNote"]);
                        objE.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                        objE.RetainedForNextDiscussion = Convert.ToBoolean(dr["RetainedForNextDiscussion"]);
                    }
                }

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<EditAction> ActionList = new List<EditAction>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        EditAction objNotificationAction = new EditAction();
                        objNotificationAction.NotificationActionId = Convert.ToInt64(dr["NotificationActionId"]);
                        objNotificationAction.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objNotificationAction.ActionName = Convert.ToString(dr["Action"]);
                        objNotificationAction.RequiredOn = Convert.ToString(dr["RequiredOn"]);
                        objNotificationAction.MailId = Convert.ToString(dr["MailId"]);
                        objNotificationAction.UpdatedOn = Convert.ToString(dr["UpdatedOn"]);
                        ActionList.Add(objNotificationAction);
                    }
                    objE.Actions = ActionList;
                }
            }

            return objE;
        }
        public EditMeeting UpdateMeetingDate(Int64? Id, string MeetingDate)
        {
            EditMeeting objE = new EditMeeting();
            MOMDataManager objDM = new MOMDataManager();
            DataSet ds = objDM.UpdateMeetingDate(Id, MeetingDate);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int tblIndex = -1;
                #region "MoM Details"
                tblIndex++;
                if (ds.Tables.Count > tblIndex)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        objE.IsExistFlag = Convert.ToInt32(dr["FLAG"]);
                        objE.MoMId = Convert.ToInt64(dr["MomId"]);
                        objE.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                    }
                }
                #endregion
            }
            return objE;
        }
    }
}
