using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BusinessObjects;
using BusinessObjects.Notification;
using BusinessService.Notification;
using DataServices.WTO;

namespace BusinessService.Notification
{
    public class NotificationListBusinessService
    {
        public NotificationList_Masters PageLoad_NotificationsMasters(string CallFrom)
        {
            NotificationList_Masters objR = new NotificationList_Masters();
            NotificationListDataManager objDM = new NotificationListDataManager();
            DataSet ds = objDM.Masters();
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<Country> CountryList = new List<Country>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        Country objCountry = new Country();
                        objCountry.CountryId = Convert.ToInt64(dr["CountryId"]);
                        objCountry.Name = Convert.ToString(dr["Country"]);
                        CountryList.Add(objCountry);
                    }
                    objR.CountryList = CountryList;
                }
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<NotificationStatusList> StatusList = new List<NotificationStatusList>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        NotificationStatusList objStatus = new NotificationStatusList();
                        objStatus.StatusId = Convert.ToInt32(dr["StageId"]);
                        objStatus.StatusName = Convert.ToString(dr["StageName"]);
                        StatusList.Add(objStatus);
                    }
                    objR.StatusList = StatusList;
                }
            }
            if (CallFrom != null && CallFrom != "" && CallFrom == "AddNotification")
                objR.CallFrom = "AddNotification";
            else
                objR.CallFrom = "";

            return objR;
        }

        public Notifications GetNotifications(GetNotificationList obj)
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
                    List<NotificationList> ItemsList = new List<NotificationList>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        NotificationList objItems = new NotificationList();
                        objItems.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objItems.ItemNumber = Convert.ToInt64(dr["ItemNumber"]);
                        objItems.Noti_Number = Convert.ToString(dr["Number"]);
                        objItems.Noti_Date = Convert.ToString(dr["DateOfNotification"]);
                        objItems.FinalDateOfComments = Convert.ToString(dr["FinalDateOfComment"]);
                        objItems.Noti_Country = Convert.ToString(dr["Country"]);
                        objItems.Title = Convert.ToString(dr["Title"]);
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
