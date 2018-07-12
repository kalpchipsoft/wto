using System;
using System.Collections.Generic;
using System.Data;
using BusinessObjects;
using BusinessObjects.Masters;
using BusinessObjects.Notification;
using DataServices.WTO;

namespace BusinessService.Notification
{
    public class NotificationListBusinessService
    {
        public NotificationList PageLoad_NotificationsList(Search_Notification obj)
        {
            NotificationList objR = new NotificationList();
            NotificationListDataManager objDM = new NotificationListDataManager();
            DataSet ds = objDM.GetPageLoad_NotificationsList(obj);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;

                #region "Country List"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<Country> CountryList = new List<Country>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        Country objCountry = new Country();
                        objCountry.CountryId = Convert.ToInt64(dr["CountryId"]);
                        objCountry.CountryCode = Convert.ToString(dr["CountryCode"]);
                        objCountry.Name = Convert.ToString(dr["Country"]);
                        CountryList.Add(objCountry);
                    }
                    objR.CountryList = CountryList;
                }
                #endregion

                #region "Status List"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<StatusMaster> StatusList = new List<StatusMaster>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        StatusMaster objStatus = new StatusMaster();
                        objStatus.StatusId = Convert.ToInt32(dr["StatusId"]);
                        objStatus.Status = Convert.ToString(dr["Status"]);
                        StatusList.Add(objStatus);
                    }
                    objR.StatusList = StatusList;
                }
                #endregion

                #region "Action List"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<ActionMaster> ActionList = new List<ActionMaster>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        ActionMaster objAction = new ActionMaster();
                        objAction.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objAction.Action = Convert.ToString(dr["Action"]);
                        ActionList.Add(objAction);
                    }
                    objR.ActionList = ActionList;
                }
                #endregion

                #region "Action Status List"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<Status> StatusList = new List<Status>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        Status objStatus = new Status();
                        objStatus.StatusId = Convert.ToInt32(dr["StatusId"]);
                        objStatus.StatusName = Convert.ToString(dr["Status"]);
                        StatusList.Add(objStatus);
                    }

                    objR.ActionStatusList = StatusList;
                }
                #endregion

                #region "Notification List"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<BusinessObjects.Notification.Notification> ItemsList = new List<BusinessObjects.Notification.Notification>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        BusinessObjects.Notification.Notification objItems = new BusinessObjects.Notification.Notification();
                        objItems.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objItems.ItemNumber = Convert.ToInt64(dr["ItemNumber"]);
                        objItems.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        objItems.NotificationDate = Convert.ToString(dr["DateOfNotification"]);
                        objItems.FinalDateOfComments = Convert.ToString(dr["FinalDateOfComment"]);
                        objItems.Country = Convert.ToString(dr["Country"]);
                        objItems.Title = Convert.ToString(dr["Title"]);
                        objItems.MailCount = Convert.ToInt32(dr["MailCount"]);
                        objItems.ResponseCount = Convert.ToInt32(dr["ResponseCount"]);
                        objItems.Actions = Convert.ToString(dr["Actions"]);
                        objItems.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                        objItems.IsInMeeting = Convert.ToBoolean(dr["IsInMeeting"]);
                        ItemsList.Add(objItems);
                    }
                    objR.Notifications = ItemsList;
                }
                #endregion

                #region "Notification Count"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    objR.TotalCount = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["TotalCount"]);
                }
                #endregion

                #region "Notification Process Dots Color & Tooltip Text"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<NotificationProcessDot> NPSList = new List<NotificationProcessDot>();
                    foreach(DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        NotificationProcessDot objNPS = new NotificationProcessDot();
                        objNPS.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objNPS.ColorCode = Convert.ToString(dr["ColorCode"]);
                        objNPS.TooltipText = Convert.ToString(dr["TooltipText"]);
                        objNPS.Sequence = Convert.ToInt32(dr["Sequence"]);
                        NPSList.Add(objNPS);
                    }
                    objR.NotificationProcessDots = NPSList;
                }
                #endregion

                #region "Notification Action Dots Color & Tooltip Text"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<NotificationActionDot> NASList = new List<NotificationActionDot>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        NotificationActionDot objNAS = new NotificationActionDot();
                        objNAS.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objNAS.ColorCode = Convert.ToString(dr["ColorCode"]);
                        objNAS.TooltipText = Convert.ToString(dr["TooltipText"]);
                        objNAS.Sequence = Convert.ToInt32(dr["Sequence"]);
                        NASList.Add(objNAS);
                    }
                    objR.NotificationActionDots = NASList;
                }
                #endregion
            }

            return objR;
        }

        public Notifications GetNotifications(Search_Notification obj)
        {
            Notifications objR = new Notifications();
            NotificationListDataManager objDM = new NotificationListDataManager();
            DataSet ds = objDM.Notifications(obj);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<BusinessObjects.Notification.Notification> ItemsList = new List<BusinessObjects.Notification.Notification>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        BusinessObjects.Notification.Notification objItems = new BusinessObjects.Notification.Notification();
                        objItems.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objItems.ItemNumber = Convert.ToInt64(dr["ItemNumber"]);
                        objItems.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        objItems.NotificationDate = Convert.ToString(dr["DateOfNotification"]);
                        objItems.FinalDateOfComments = Convert.ToString(dr["FinalDateOfComment"]);
                        objItems.Country = Convert.ToString(dr["Country"]);
                        objItems.Title = Convert.ToString(dr["Title"]);
                        objItems.MailCount = Convert.ToInt32(dr["MailCount"]);
                        objItems.ResponseCount = Convert.ToInt32(dr["ResponseCount"]);
                        objItems.Actions = Convert.ToString(dr["Actions"]);
                        objItems.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                        objItems.IsInMeeting = Convert.ToBoolean(dr["IsInMeeting"]);
                        ItemsList.Add(objItems);
                    }
                    objR.ItemsList = ItemsList;
                }

                #region "Paging"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    objR.TotalCount = Convert.ToString(ds.Tables[tblIndx].Rows[0]["TotalCount"]);
                    objR.Pager = new Pager(Convert.ToInt32(objR.TotalCount), Convert.ToInt16(obj.PageIndex));
                }
                #endregion

                #region "Notification Process Dots Color & Tooltip Text"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<NotificationProcessDot> NPSList = new List<NotificationProcessDot>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        NotificationProcessDot objNPS = new NotificationProcessDot();
                        objNPS.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objNPS.ColorCode = Convert.ToString(dr["ColorCode"]);
                        objNPS.TooltipText = Convert.ToString(dr["TooltipText"]);
                        objNPS.Sequence = Convert.ToInt32(dr["Sequence"]);
                        NPSList.Add(objNPS);
                    }
                    objR.NotificationProcessDots = NPSList;
                }
                #endregion

                #region "Notification Action Dots Color & Tooltip Text"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<NotificationActionDot> NASList = new List<NotificationActionDot>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        NotificationActionDot objNAS = new NotificationActionDot();
                        objNAS.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objNAS.ColorCode = Convert.ToString(dr["ColorCode"]);
                        objNAS.TooltipText = Convert.ToString(dr["TooltipText"]);
                        objNAS.Sequence = Convert.ToInt32(dr["Sequence"]);
                        NASList.Add(objNAS);
                    }
                    objR.NotificationActionDots = NASList;
                }
                #endregion
            }
            return objR;
        }

        public DataTable ExportNotificationList(Search_Notification obj)
        {
            NotificationList objR = new NotificationList();
            NotificationListDataManager objDM = new NotificationListDataManager();
            DataSet ds = objDM.ExportNotificationList(obj);
            DataTable dt = new DataTable();
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                #region "Notification List"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    dt = ds.Tables[tblIndx];
                    //dt.Columns.Remove("ItemNumber");
                    //dt.Columns.Remove("NotificationId");
                    //dt.Columns.Remove("Actions");
                    //dt.Columns.Remove("StatusFlag");
                    //dt.Columns.Remove("StatusName");
                }
                #endregion
            }
            return dt;
        }

        #region "Notification Country List"
        public NotificationCountries PageLoad_NotificationCountries(Search_NotificationCountries obj)
        {
            NotificationCountries objNotificationCountries = new NotificationCountries();
            NotificationListDataManager objDM = new NotificationListDataManager();
            DataSet ds = objDM.PageLoad_CountriesNotifications(obj);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                #region "Country List"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<Country> CountryList = new List<Country>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        Country objCountry = new Country();
                        objCountry.CountryId = Convert.ToInt64(dr["CountryId"]);
                        objCountry.CountryCode = Convert.ToString(dr["CountryCode"]);
                        objCountry.Name = Convert.ToString(dr["Country"]);
                        CountryList.Add(objCountry);
                    }
                    objNotificationCountries.CountryList = CountryList;
                }
                #endregion
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<BusinessObjects.Notification.CountriesNotificationList> ItemsList = new List<BusinessObjects.Notification.CountriesNotificationList>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        CountriesNotificationList objItems = new CountriesNotificationList();
                        objItems.CountryId = Convert.ToString(dr["CountryId"]);
                        objItems.NotificationCount = Convert.ToString(dr["NotificationCount"]);
                        objItems.CountryCode = Convert.ToString(dr["CountryCode"]);
                        objItems.CountryName = Convert.ToString(dr["Country"]);
                        ItemsList.Add(objItems);
                    }
                    objNotificationCountries.objNotificationCountries = ItemsList;
                }
                #region "Paging"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    objNotificationCountries.TotalCount = Convert.ToString(ds.Tables[tblIndx].Rows[0]["TotalCount"]);
                    objNotificationCountries.Pager = new Pager(Convert.ToInt32(objNotificationCountries.TotalCount), Convert.ToInt16(obj.PageIndex));
                }
                #endregion
            }
            return objNotificationCountries;
        }
        public NotificationCountries GetNotificationCountries(Search_NotificationCountries obj)
        {
            NotificationCountries objNotificationCountries = new NotificationCountries();
            NotificationListDataManager objDM = new NotificationListDataManager();
            DataSet ds = objDM.CountriesNotifications(obj);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<BusinessObjects.Notification.CountriesNotificationList> ItemsList = new List<BusinessObjects.Notification.CountriesNotificationList>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        CountriesNotificationList objItems = new CountriesNotificationList();
                        objItems.CountryId = Convert.ToString(dr["CountryId"]);
                        objItems.NotificationCount = Convert.ToString(dr["NotificationCount"]);
                        objItems.CountryCode = Convert.ToString(dr["CountryCode"]);
                        objItems.CountryName = Convert.ToString(dr["Country"]);
                        ItemsList.Add(objItems);
                    }
                    objNotificationCountries.objNotificationCountries = ItemsList;
                }

                #region "Paging"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    objNotificationCountries.TotalCount = Convert.ToString(ds.Tables[tblIndx].Rows[0]["TotalCount"]);
                    objNotificationCountries.Pager = new Pager(Convert.ToInt32(objNotificationCountries.TotalCount), Convert.ToInt16(obj.PageIndex));
                }
                #endregion
            }
            return objNotificationCountries;
        }
        #endregion
        #region "Stakeholder Mail sent and Response List"
        public StakeholderMailSentReceive GetStakeholderMailSentResponse(Search_StakeholderMailSentReceive obj)
        {
            StakeholderMailSentReceive objStakeholderMailSentReceive = new StakeholderMailSentReceive();
            NotificationListDataManager objDM = new NotificationListDataManager();
            DataSet ds = objDM.GetStakeholderMailSentResponse(obj);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<RelatedStakeHolders> ItemsList = new List<RelatedStakeHolders>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        RelatedStakeHolders objItems = new RelatedStakeHolders();
                        objItems.StakeHolderId = Convert.ToInt32(dr["StakeHolderId"]);
                        objItems.FullName = Convert.ToString(dr["FullName"]);
                        objItems.OrgName = Convert.ToString(dr["OrgName"]);
                        objItems.Designation = Convert.ToString(dr["Designation"]);
                        objItems.MailCount = Convert.ToInt32(dr["MailCount"]);
                        objItems.ResponseCount = Convert.ToInt32(dr["ResponseCount"]);
                        objItems.HSCodes = Convert.ToString(dr["HSCodes"]);
                        ItemsList.Add(objItems);
                    }
                    objStakeholderMailSentReceive.objRelatedStakeHolders = ItemsList;
                }

                #region "Paging"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    objStakeholderMailSentReceive.TotalCount = Convert.ToString(ds.Tables[tblIndx].Rows[0]["TotalCount"]);
                    objStakeholderMailSentReceive.Pager = new Pager(Convert.ToInt32(objStakeholderMailSentReceive.TotalCount), Convert.ToInt16(obj.PageIndex));
                }
                #endregion
            }
            return objStakeholderMailSentReceive;
        }
        #endregion

        #region "Add Notification in Meeting"
        public bool SaveMeetingNotification(Int32 Id, string MeetingDate, string CreatedBy)
        {
            bool result = false;
            NotificationDataManager objDM = new NotificationDataManager();
            result = objDM.SaveMeetingNotification(Id, MeetingDate, CreatedBy);
            return result;
        }
        #endregion "Add Notification in Meeting"

    }
}
