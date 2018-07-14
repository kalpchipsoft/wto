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
                        objNotification.IsActive = Convert.ToBoolean(dr["IsActive"]);
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
                        objE.IsActive = Convert.ToBoolean(dr["IsActive"]);
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
                        objNotification.NotificationGroup = Convert.ToString(dr["NotificationGroup"]);
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
        public EditNotificationMeeting EditMeetingActions(Int64 Id, Int64 MeetingId)
        {
            EditNotificationMeeting objE = new EditNotificationMeeting();
            MOMDataManager objDM = new MOMDataManager();
            DataSet ds = objDM.EditActions(Id, MeetingId);
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
                        //objE.Status = Convert.ToString(dr["Stage"]);
                        objE.MeetingNote = Convert.ToString(dr["MeetingNote"]);
                        objE.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                        objE.RetainedForNextDiscussion = Convert.ToBoolean(dr["RetainedForNextDiscussion"]);
                        objE.NotificationGroup = Convert.ToString(dr["NotificationGroup"]);
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

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<PrevioiusMeeting> PrevioiusMeetingList = new List<PrevioiusMeeting>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        PrevioiusMeeting PrevioiusMeeting = new PrevioiusMeeting();
                        PrevioiusMeeting.MeetingId = Convert.ToInt64(dr["MeetingId"]);
                        PrevioiusMeeting.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                        PrevioiusMeetingList.Add(PrevioiusMeeting);
                    }
                    objE.PrevioiusMeetings = PrevioiusMeetingList;
                }

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<PrevioiusMeetingAction> PrevioiusMeetingActionList = new List<PrevioiusMeetingAction>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        PrevioiusMeetingAction PrevioiusMeetingAction = new PrevioiusMeetingAction();
                        PrevioiusMeetingAction.MeetingId = Convert.ToInt64(dr["MeetingId"]);
                        PrevioiusMeetingAction.NotificationActionId = Convert.ToInt64(dr["NotificationActionId"]);
                        PrevioiusMeetingAction.Action = Convert.ToString(dr["Action"]);
                        PrevioiusMeetingAction.ActionStatus = Convert.ToString(dr["ActionStatus"]);
                        PrevioiusMeetingActionList.Add(PrevioiusMeetingAction);
                    }
                    objE.PrevioiusMeetingActions = PrevioiusMeetingActionList;
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
        public bool EndMeeting(Int64? Id, string Observation)
        {
            MOMDataManager objDM = new MOMDataManager();
            bool result = objDM.EndMeeting(Id, Observation);
            return result;
        }
        public string ValidateMeetingDate(string date, Nullable<Int64> MoMId)
        {
            MOMDataManager objDM = new MOMDataManager();
            DataTable dt = objDM.ValidateMeetingDate(date, MoMId);
            string result = string.Empty;

            if (dt != null && dt.Rows.Count > 0)
                result = Convert.ToString(dt.Rows[0]["Message"]);

            return result;
        }
        public MoMs MeetingSummary(Int64 Id)
        {
            MoMs objMoM = new MoMs();
            MOMDataManager objDM = new MOMDataManager();
            DataSet ds = objDM.MeetingSummary(Id);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int tblIndex = -1;

                #region "Total Of MOM Notification"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<MoM> MOMNotification = new List<MoM>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        MoM objMOMList = new MoM();
                        objMOMList.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                        objMOMList.SPSNotificationCount = Convert.ToInt32(dr["SPSNotification"]);
                        objMOMList.TBTNotificationCount = Convert.ToInt32(dr["TBTNotification"]);
                        objMOMList.Observation = Convert.ToString(dr["Observation"]);
                        MOMNotification.Add(objMOMList);
                    }
                    objMoM.MoMList = MOMNotification;
                }
                #endregion

                #region "TBT Notification"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<Notification_Mom> MOMNotificationList = new List<Notification_Mom>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        Notification_Mom objList = new Notification_Mom();
                        objList.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objList.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        objList.DateOfNotification = Convert.ToString(dr["DateOfNotification"]);
                        objList.Country = Convert.ToString(dr["Country"]);
                        objList.Description = Convert.ToString(dr["Description"]);
                        objList.Action = Convert.ToString(dr["Action"]);
                        MOMNotificationList.Add(objList);
                    }
                    objMoM.TBTNotificationList = MOMNotificationList;
                }
                #endregion

                #region "SPS Notification"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<Notification_Mom> MOMNotificationList = new List<Notification_Mom>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        Notification_Mom objList = new Notification_Mom();
                        objList.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objList.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        objList.DateOfNotification = Convert.ToString(dr["DateOfNotification"]);
                        objList.Country = Convert.ToString(dr["Country"]);
                        objList.Description = Convert.ToString(dr["Description"]);
                        objList.Action = Convert.ToString(dr["Action"]);
                        MOMNotificationList.Add(objList);
                    }
                    objMoM.SPSNotificationList = MOMNotificationList;
                }
                #endregion
                #region "Regulatory Notification"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<Notification_Mom> MOMNotificationList = new List<Notification_Mom>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        Notification_Mom objList = new Notification_Mom();
                        objList.ItemNumber = Convert.ToInt32(dr["RowNumber"]);
                        objList.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objList.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        objList.Country = Convert.ToString(dr["Country"]);
                        objList.Description = Convert.ToString(dr["Description"]);
                        objList.Regulatory = Convert.ToString(dr["Regulators"]).Replace("|", ", ");
                        MOMNotificationList.Add(objList);
                    }
                    objMoM.RegulatoryNotificationList = MOMNotificationList;
                }
                #endregion
                #region "Regulatory Notification"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<Notification_Mom> MOMNotificationList = new List<Notification_Mom>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        Notification_Mom objList = new Notification_Mom();
                        objList.ItemNumber = Convert.ToInt32(dr["RowNumber"]);
                        objList.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objList.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        objList.Country = Convert.ToString(dr["Country"]);
                        objList.Description = Convert.ToString(dr["Description"]);
                        MOMNotificationList.Add(objList);
                    }
                    objMoM.PolicyNotificationList = MOMNotificationList;
                }
                #endregion
            }

            return objMoM;
        }
        public NotificationActions GetNotificationActions(Int64 Id, int ActionId)
        {
            NotificationActions objNA = new NotificationActions();
            MOMDataManager objDM = new MOMDataManager();
            DataSet ds = objDM.EditActions(Id, ActionId);
            if (ds != null)
            {
                int tblIndex = -1;

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        objNA.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objNA.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        objNA.NotificationTitle = Convert.ToString(dr["Title"]);
                        objNA.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                        objNA.IsUpdate = Convert.ToBoolean(dr["IsUpdate"]);
                        objNA.MeetingNotes = Convert.ToString(dr["MeetingNote"]);
                        objNA.RetainedForNextDiscussion = Convert.ToBoolean(dr["RetainedForNextDiscussion"]);
                    }
                }

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<NotificationActionDetail> NotificationActionList = new List<NotificationActionDetail>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationActionDetail objNotificationAction = new NotificationActionDetail();
                        objNotificationAction.NotificationActionId = Convert.ToInt64(dr["NotificationActionId"]);
                        objNotificationAction.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objNotificationAction.ActionName = Convert.ToString(dr["Action"]);
                        objNotificationAction.RequiredOn = Convert.ToString(dr["RequiredOn"]);
                        objNotificationAction.EnteredOn = Convert.ToString(dr["EnteredOn"]);
                        objNotificationAction.UpdatedOn = Convert.ToString(dr["UpdatedOn"]);
                        objNotificationAction.MailId = Convert.ToInt64(dr["MailId"]);
                        objNotificationAction.MailTo = Convert.ToString(dr["MailTo"]);
                        NotificationActionList.Add(objNotificationAction);
                    }
                    objNA.Actions = NotificationActionList;
                }
            }

            return objNA;
        }
        public string GetMeetingNotes(int NotificationId)
        {
            MOMDataManager objDM = new MOMDataManager();
            string MeetingNote = "";
            DataTable dt = objDM.GetMeetingNotes(NotificationId);
            if (dt != null && dt.Rows.Count > 0)
                MeetingNote = Convert.ToString(dt.Rows[0]["MeetingNote"]);

            return MeetingNote;
        }
        public string UpdateMeetingNote(SaveNote obj)
        {
            MOMDataManager objDM = new MOMDataManager();
            string MeetingNote = "";
            DataTable dt = objDM.UpdateMeetingNote(obj);
            if (dt != null && dt.Rows.Count > 0)
                MeetingNote = Convert.ToString(dt.Rows[0]["MeetingNote"]);

            return MeetingNote;
        }
        public AddNotificationAction_Output EditNotificationAction(Int64 NotificationActionId)
        {
            AddNotificationAction_Output objO = new AddNotificationAction_Output();
            MOMDataManager objDM = new MOMDataManager();
            DataTable dt = objDM.EditNotificationAction(NotificationActionId);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    objO.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                    objO.NotificationActionId = Convert.ToInt64(dr["NotificationActionId"]);
                    objO.ActionId = Convert.ToInt32(dr["ActionId"]);
                    objO.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                    objO.DateofNotification = Convert.ToString(dr["DatefNotification"]);
                    objO.ResponseCount = Convert.ToInt32(dr["ResponseCount"]);
                    objO.MailCount = Convert.ToInt32(dr["MailCount"]);
                    objO.MailTo = Convert.ToString(dr["EnquiryEmailId"]);
                }

                if (objO.ActionId == 1 || objO.ActionId == 2)
                {
                    Notification_Template_Search objS = new Notification_Template_Search();
                    objS.TemplateType = "Mail";
                    objS.TemplateFor = objO.ActionId == 1 ? "BrieftoRegulators" : "PolicyBrief";
                    objS.NotificationActionId = objO.NotificationActionId;
                    objO.MailDetails = GetNotificationSMSMailTemplate(objO.NotificationId, objS);
                }
            }
            return objO;
        }
        public SendActionMail_Output SaveAndSendMailforAction(long Id, ActionMailDetails obj)
        {
            SendActionMail_Output objOutput = new SendActionMail_Output();
            MOMDataManager objDM = new MOMDataManager();
            DataSet ds = objDM.SendActionMail(Id, obj);
            if (ds != null)
            {
                int tblIndex = -1;

                tblIndex++;
                if (ds.Tables.Count > tblIndex)
                {
                    using (DataTable dt = ds.Tables[tblIndex])
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            objOutput.Subject = Convert.ToString(dr["Subject"]);
                            objOutput.Body = Convert.ToString(dr["Message"]);

                            objOutput.MailTo = Convert.ToString(dr["MailTo"]);
                            objOutput.ReplyTo = Convert.ToString(dr["ReplyTo"]);
                            objOutput.CC = Convert.ToString(dr["CC"]);
                            objOutput.BCC = Convert.ToString(dr["BCC"]);
                            objOutput.DisplayName = Convert.ToString(dr["DisplayName"]);
                        }
                        if (obj.Attachments.Count > 0 && obj.Attachments != null)
                        {
                            foreach (Attachment objA in obj.Attachments)
                            {
                                if (objA.Content != "")
                                {
                                    try
                                    {
                                        byte[] bytes = null;
                                        if (objA.Content.IndexOf(',') >= 0)
                                        {
                                            var myString = objA.Content.Split(new char[] { ',' });
                                            bytes = Convert.FromBase64String(myString[1]);
                                        }
                                        else
                                            bytes = Convert.FromBase64String(objA.Content);

                                        if (objA.FileName.Length > 0 && bytes.Length > 0)
                                        {
                                            string filePath = HttpContext.Current.Server.MapPath("/Attachments/MailAttachment/" + Convert.ToString(dt.Rows[0]["MailId"]) + "_" + objA.FileName);
                                            File.WriteAllBytes(filePath, bytes);
                                        }
                                    }
                                    catch (Exception ex) { }
                                }
                            }
                        }
                    }
                }

                tblIndex++;
                if (ds.Tables.Count > tblIndex)
                {
                    using (DataTable dt = ds.Tables[tblIndex])
                    {
                        List<EditAttachment> AttachmentList = new List<EditAttachment>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            EditAttachment objA = new EditAttachment();
                            objA.FileName = Convert.ToString(dr["Attachment"]);
                            objA.Path = Convert.ToString(dr["DocumentPath"]);
                            AttachmentList.Add(objA);
                        }
                        objOutput.Attachments = AttachmentList;
                    }
                }

            }
            return objOutput;
        }
        public NotificationActionDetail ViewAction(long Id)
        {
            NotificationActionDetail objNA = new NotificationActionDetail();
            MOMDataManager objDM = new MOMDataManager();
            DataSet ds = objDM.ViewActions(Id);
            if (ds != null)
            {
                int tblIndex = -1;

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        objNA.NotificationActionId = Convert.ToInt64(dr["NotificationActionId"]);
                        objNA.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objNA.Action = Convert.ToString(dr["Action"]);
                        objNA.RequiredOn = Convert.ToString(dr["RequiredOn"]);
                        objNA.EnteredOn = Convert.ToString(dr["EnteredOn"]);
                        objNA.UpdatedOn = Convert.ToString(dr["UpdatedOn"]);
                        objNA.MailId = Convert.ToInt64(dr["MailId"]);
                        objNA.MailTo = Convert.ToString(dr["MailTo"]);

                        Notification_Template objT = new Notification_Template();
                        objT.Subject = Convert.ToString(dr["Subject"]);
                        objT.Message = Convert.ToString(dr["Message"]);
                        objNA.MailDetails = objT;

                        objNA.ResponseId = Convert.ToInt64(dr["ResponseId"]);
                    }
                }

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<EditAttachment> AttachmentList = new List<EditAttachment>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        EditAttachment objA = new EditAttachment();
                        objA.FileName = Convert.ToString(dr["FileName"]);
                        objA.Path = Convert.ToString(dr["FilePath"]);
                        AttachmentList.Add(objA);
                    }
                    objNA.Attachments = AttachmentList;
                }
            }

            return objNA;
        }
    }
}
