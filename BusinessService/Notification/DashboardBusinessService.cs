using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BusinessObjects;
using BusinessObjects.Notification;
using DataServices.WTO;
using UtilitiesManagers;

namespace BusinessService.Notification
{
    public class DashboardBusinessService
    {
        public Dashboard_PendingCounts GetDashboard_PendingCounts(Dashboard obj)
        {
            Dashboard_PendingCounts objR = new Dashboard_PendingCounts();
            DashboardDataManager objDDM = new DashboardDataManager();
            DataSet ds = objDDM.GetDashboard_PendingCounts(obj);
            if (ds != null)
            {
                int tblIndex = -1;

                tblIndex++;
                if(ds.Tables.Count>tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<Count_Discussion> Count_DiscussionList = new List<Count_Discussion>();
                    foreach(DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        Count_Discussion objD = new Count_Discussion();
                        objD.StatusId = Convert.ToInt32(dr["StatusId"]);
                        objD.Status = Convert.ToString(dr["Status"]);
                        objD.PendingFromUser = Convert.ToInt64(dr["PendingFromUser"]);
                        objD.TotalPending = Convert.ToInt64(dr["TotalPending"]);
                        objD.CssColor = Convert.ToString(dr["CssColor"]);
                        objD.Icon = Convert.ToString(dr["IconImage"]);
                        Count_DiscussionList.Add(objD);
                    }
                    objR.PendingDiscussions = Count_DiscussionList;
                }

                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<Count_Action> Count_ActionList = new List<Count_Action>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        Count_Action objD = new Count_Action();
                        objD.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objD.Action = Convert.ToString(dr["Action"]);
                        objD.Total = Convert.ToInt32(dr["TotalCount"]);
                        objD.OverDue = Convert.ToInt32(dr["DueCount"]);
                        objD.CssColor = Convert.ToString(dr["CssColor"]);
                        Count_ActionList.Add(objD);
                    }
                    objR.PendingActions = Count_ActionList;
                }

            }
            return objR;
        }
        public List<HSCodeGraphData> GetHSCodeGraphData(DashboardSearch obj)
        {
            List<HSCodeGraphData> objHSCodeGraphList = new List<HSCodeGraphData>();
            DashboardDataManager objDDM = new DashboardDataManager();
            DataSet ds = objDDM.GetHsCodeGraphData(obj);
            if (ds != null)
            {
                int tblIndex = -1;

                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        HSCodeGraphData objD = new HSCodeGraphData();
                        objD.NotificationCount = Convert.ToInt32(dr["NotificationCount"]);
                        objD.HSCode = Convert.ToString(dr["HSCode"]);
                        objD.Text = Convert.ToString(dr["Text"]);
                        objHSCodeGraphList.Add(objD);
                    }
                }
            }
            return objHSCodeGraphList;
        }
        public HSCodeCountry GetHsCodeGraphDataCountryWise(DashboardSearch obj)
        {
            HSCodeCountry objHSCodeCountry = new HSCodeCountry();
            List<HSCodeCountryData> objHSCodeCountryList = new List<HSCodeCountryData>();
            DashboardDataManager objDDM = new DashboardDataManager();
            DataSet ds = objDDM.GetHsCodeGraphDataCountryWise(obj);
            if (ds != null)
            {
                int tblIndex = -1;

                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        HSCodeCountryData objD = new HSCodeCountryData();
                        objD.NotificationCount = Convert.ToInt32(dr["NotificationCount"]);
                        objD.Country = Convert.ToString(dr["Country"]);
                        objD.ColorCode = Convert.ToString(dr["ColorCode"]);
                        objD.CountryCode = Convert.ToString(dr["CountryCode"]);
                        objHSCodeCountryList.Add(objD);
                    }
                    objHSCodeCountry.objHSCodeCountryData = objHSCodeCountryList;
                }
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    objHSCodeCountry.CountryCount = Convert.ToString(ds.Tables[tblIndex].Rows[0]["TotalCountries"]);
                    objHSCodeCountry.HSCode = Convert.ToString(ds.Tables[tblIndex].Rows[0]["HSCode"]);
                }
            }
            return objHSCodeCountry;
        }
        public List<NotificationGraphData> GetNotificationGraphData(DashboardSearch obj)
        {
            List<NotificationGraphData> objNotificationGraphList = new List<NotificationGraphData>();
            DashboardDataManager objDDM = new DashboardDataManager();
            DataSet ds = objDDM.GetNotificationGraphData(obj);
            if (ds != null)
            {
                int tblIndex = -1;
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationGraphData objD = new NotificationGraphData();
                        objD.NotificationCount = Convert.ToInt32(dr["NotificationCount"]);
                        objD.InProcessCount = Convert.ToInt32(dr["InProcessCount"]);
                        objD.UnderActionCount = Convert.ToInt32(dr["UnderActionCount"]);
                        objD.ClosedCount = Convert.ToInt32(dr["ClosedCount"]);
                        objD.LapsedCount = Convert.ToInt32(dr["LapsedCount"]);
                        objD.MonthName = Convert.ToString(dr["MonthName"]);
                        objNotificationGraphList.Add(objD);
                    }
                }
            }
            return objNotificationGraphList;
        }
        public NotificationRequestResponse GetNotificationCountRequestResponse(DashboardSearch obj)
        {
            NotificationRequestResponse objNotificationRequestResponse = new NotificationRequestResponse();
            DashboardDataManager objDDM = new DashboardDataManager();
            DataSet ds = objDDM.GetNotificationCountRequestResponse(obj);
            if (ds != null)
            {
                int tblIndex = -1;
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<NotificationTextCount> objRequestandResponseList = new List<NotificationTextCount>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationTextCount objD = new NotificationTextCount();                   
                        objD.Text = Convert.ToString(dr["Text"]);
                        objD.Count = Convert.ToString(dr["NotificationCount"]);
                        objRequestandResponseList.Add(objD);
                    }
                    objNotificationRequestResponse.objRequestForFullText = objRequestandResponseList;
                }
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<NotificationTextCount> objRequestandResponseList = new List<NotificationTextCount>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationTextCount objD = new NotificationTextCount();
                        objD.Text = Convert.ToString(dr["Text"]);
                        objD.Count = Convert.ToString(dr["NotificationCount"]);
                        objRequestandResponseList.Add(objD);
                    }
                    objNotificationRequestResponse.objRequestForTranslation = objRequestandResponseList;
                }
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<NotificationTextCount> objRequestandResponseList = new List<NotificationTextCount>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationTextCount objD = new NotificationTextCount();
                        objD.Text = Convert.ToString(dr["Text"]);
                        objD.Count = Convert.ToString(dr["NotificationCount"]);
                        objRequestandResponseList.Add(objD);
                    }
                    objNotificationRequestResponse.objRequestForStakeholderResponse = objRequestandResponseList;
                }
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    objNotificationRequestResponse.ClosedCount = Convert.ToString(ds.Tables[tblIndex].Rows[0]["NotificationCount"]);
                }
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    objNotificationRequestResponse.LapsedCount = Convert.ToString(ds.Tables[tblIndex].Rows[0]["NotificationCount"]);
                }
            }
            return objNotificationRequestResponse;
        }
        public List<NotificationPendingCount_Action> GetDashboardAction(DashboardSearch obj)
        {
            List<NotificationPendingCount_Action> objNotificationDashboardAction = new List<NotificationPendingCount_Action>();
            DashboardDataManager objDDM = new DashboardDataManager();
            DataSet ds = objDDM.GetDashboardPendingCount_Action(obj);
            if (ds != null)
            {
                int tblIndex = -1;
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationPendingCount_Action objD = new NotificationPendingCount_Action();
                        objD.Action = Convert.ToString(dr["Action"]);
                        objD.ColorCode = Convert.ToString(dr["ColorCode"]);
                        objD.Total = Convert.ToInt32(dr["Total"]);
                        objD.Completed = Convert.ToInt32(dr["Completed"]);
                        objD.Pending = Convert.ToInt32(dr["Pending"]);
                        objD.Overdue = Convert.ToInt32(dr["Overdue"]);
                        objD.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objNotificationDashboardAction.Add(objD);
                    }
                }
            }
            return objNotificationDashboardAction;
        }
        public NotificationProcessingStatus GetNotificationCountProcessingStatus(DashboardSearch obj)
        {
            NotificationProcessingStatus objNotificationProcessingStatus = new NotificationProcessingStatus();
            DashboardDataManager objDDM = new DashboardDataManager();
            DataSet ds = objDDM.GetNotificationCountProcessingStatus(obj);
            if (ds != null)
            {
                int tblIndex = -1;
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<NotificationTextCount> objProcessingStatusList = new List<NotificationTextCount>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationTextCount objD = new NotificationTextCount();
                        objD.Text = Convert.ToString(dr["Text"]);
                        objD.Count = Convert.ToString(dr["NotificationCount"]);
                        objProcessingStatusList.Add(objD);
                    }
                    objNotificationProcessingStatus.objPendingFullText = objProcessingStatusList;
                }
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<NotificationTextCount> objProcessingStatusList = new List<NotificationTextCount>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationTextCount objD = new NotificationTextCount();
                        objD.Text = Convert.ToString(dr["Text"]);
                        objD.Count = Convert.ToString(dr["NotificationCount"]);
                        objProcessingStatusList.Add(objD);
                    }
                    objNotificationProcessingStatus.objPendingTranslation = objProcessingStatusList;
                }
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<NotificationTextCount> objProcessingStatusList = new List<NotificationTextCount>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationTextCount objD = new NotificationTextCount();
                        objD.Text = Convert.ToString(dr["Text"]);
                        objD.Count = Convert.ToString(dr["NotificationCount"]);
                        objProcessingStatusList.Add(objD);
                    }
                    objNotificationProcessingStatus.objToSendtoStakeholder = objProcessingStatusList;
                }
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<NotificationTextCount> objProcessingStatusList = new List<NotificationTextCount>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationTextCount objD = new NotificationTextCount();
                        objD.Text = Convert.ToString(dr["Text"]);
                        objD.Count = Convert.ToString(dr["NotificationCount"]);
                        objProcessingStatusList.Add(objD);
                    }
                    objNotificationProcessingStatus.objToDiscuss = objProcessingStatusList;
                }
            }
            return objNotificationProcessingStatus;
        }
        public List<NotificationGraphData> GetNotificationGraphDataWeekly(DashboardSearch obj)
        {
            List<NotificationGraphData> objNotificationGraphList = new List<NotificationGraphData>();
            DashboardDataManager objDDM = new DashboardDataManager();
            DataSet ds = objDDM.GetNotificationGraphDataWeekly(obj);
            if (ds != null)
            {
                int tblIndex = -1;
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationGraphData objD = new NotificationGraphData();
                        objD.NotificationCount = Convert.ToInt32(dr["NotificationCount"]);
                        objD.InProcessCount = Convert.ToInt32(dr["InProcessCount"]);
                        objD.UnderActionCount = Convert.ToInt32(dr["UnderActionCount"]);
                        objD.ClosedCount = Convert.ToInt32(dr["ClosedCount"]);
                        objD.LapsedCount = Convert.ToInt32(dr["LapsedCount"]);
                        objD.MonthName = Convert.ToString(dr["WeekName"]);
                        objNotificationGraphList.Add(objD);
                    }
                }
            }
            return objNotificationGraphList;
        }
        public List<NotificationGraphData> GetNotificationGraphDataMonthly(DashboardSearch obj)
        {
            List<NotificationGraphData> objNotificationGraphList = new List<NotificationGraphData>();
            DashboardDataManager objDDM = new DashboardDataManager();
            DataSet ds = objDDM.GetNotificationGraphDataMonthly(obj);
            if (ds != null)
            {
                int tblIndex = -1;
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationGraphData objD = new NotificationGraphData();
                        objD.NotificationCount = Convert.ToInt32(dr["NotificationCount"]);
                        objD.InProcessCount = Convert.ToInt32(dr["InProcessCount"]);
                        objD.UnderActionCount = Convert.ToInt32(dr["UnderActionCount"]);
                        objD.ClosedCount = Convert.ToInt32(dr["ClosedCount"]);
                        objD.LapsedCount = Convert.ToInt32(dr["LapsedCount"]);
                        objD.MonthName = Convert.ToString(dr["MONTHNAME"]);
                        objNotificationGraphList.Add(objD);
                    }
                }
            }
            return objNotificationGraphList;
        }
    }
}
