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
                        objD.Total = Convert.ToInt32(dr["TotalCount"]);
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
    }
}
