using System;
using System.Collections.Generic;
using System.Data;
using BusinessObjects.MOM;
using DataServices.WTO;


namespace BusinessService.MOM
{
    public class MomBusinessService
    {
        public NotificationList_Mom GetNotificationList_Mom(Int64 Id)
        {
            NotificationList_Mom objMom = new NotificationList_Mom();
            MOMDataManager objDM = new MOMDataManager();
            DataSet ds = objDM.GetNotificationListForMom(Id);
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
                    objMom.Actions = ActionList;
                }
                #endregion

                #region "MoM Details"
                tblIndex++;
                if (ds.Tables.Count > tblIndex)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        objMom.MoMId = Convert.ToInt64(dr["MomId"]);
                        objMom.MeetingDate= Convert.ToString(dr["MeetingDate"]);
                    }
                }
                #endregion

                #region "Notifications List"
                tblIndex++;
                if (ds.Tables.Count > tblIndex)
                {
                    List<Notification_Mom> NotificationList = new List<Notification_Mom>();
                    int i = 1;
                    string hdnRelatedStakeHolder = string.Empty;
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        Notification_Mom objNotification = new Notification_Mom();
                        objNotification.ItemNumber = i;
                        objNotification.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objNotification.Title = Convert.ToString(dr["Title"]);
                        objNotification.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        objNotification.Country = Convert.ToString(dr["Country"]);
                        i++;
                        NotificationList.Add(objNotification);
                    }
                    objMom.NotificationList = NotificationList;
                }
                #endregion

                #region "Notification Actions List"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<MoMDetail> NotificationActionList = new List<MoMDetail>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        MoMDetail objAction = new MoMDetail();
                        objAction.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objAction.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objAction.IsUpdated = Convert.ToBoolean(dr["IsUpdated"]);
                        NotificationActionList.Add(objAction);
                    }
                    objMom.NotificationActions = NotificationActionList;
                }
                #endregion
            }
            return objMom;
        }
        public Int32 InsertUpdateMomData(Int64 UserId, AddMoM objAddMOM)
        {
            MOMDataManager objDM = new MOMDataManager();
            return objDM.InsertUpdateMomData(UserId, objAddMOM);
        }
        public MoMs GetMOMListData()
        {
            MoMs objMoM = new MoMs();
            MOMDataManager objDM = new MOMDataManager();
            DataSet ds= objDM.GetMOMListData();
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
                        objNotification.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                        i++;
                        NotificationList.Add(objNotification);
                    }
                    objMoM.MoMList= NotificationList;
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
                        objAction.Count= Convert.ToInt64(dr["ActionCount"]);
                        MoMActionList.Add(objAction);
                    }
                    objMoM.MoMActionList= MoMActionList;
                }
                #endregion
            }
            
            return objMoM;
        }
    }
}
