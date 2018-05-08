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
                        objItems.DiscussionStatus = Convert.ToInt32(dr["DiscussionStatus"]);
                        objItems.Actions = Convert.ToString(dr["Actions"]);
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
                        objItems.DiscussionStatus = Convert.ToInt32(dr["DiscussionStatus"]);
                        objItems.Actions = Convert.ToString(dr["Actions"]);
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
            }
            return objR;
        }
    }
}
