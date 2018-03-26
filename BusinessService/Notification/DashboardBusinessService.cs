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
        public Result GetPageLoadCount(Dashboard obj)
        {
            Result objR = new Result();
            objR.Discussion = GetDiscussionCounts(obj);
            objR.Actions = GetActionsCounts(obj);
            return objR;
        }
        public Result_DiscussionCounts GetDiscussionCounts(Dashboard obj)
        {
            Result_DiscussionCounts objR = new Result_DiscussionCounts();
            DashboardDataManager objDDM = new DashboardDataManager();
            DataTable dt = objDDM.GetDashboard_DiscussionCounts(obj);
            if (dt != null && dt.Rows.Count > 0)
            {
                objR.PendingDoc = Convert.ToInt64(dt.Rows[0]["PendingDoc"]);
                objR.PendingTrans = Convert.ToInt64(dt.Rows[0]["PendingTrans"]);
                objR.ToSendToStakeholder = Convert.ToInt64(dt.Rows[0]["ToSendToStakeholder"]);
                objR.PendingDiscuss = Convert.ToInt64(dt.Rows[0]["PendingDiscuss"]);

                objR.StatusType = StatusType.SUCCESS;
                objR.MessageType = MessageType.NO_MESSAGE;
            }
            else
            {
                objR.StatusType = StatusType.FAILURE;
                objR.MessageType = MessageType.TRY_AGAIN;
            }

            return objR;
        }

        public Result_ActionsCounts GetActionsCounts(Dashboard obj)
        {
            Result_ActionsCounts objR = new Result_ActionsCounts();
            DashboardDataManager objDDM = new DashboardDataManager();
            ActionCountsType objType;
            DataSet ds = objDDM.GetDashboard_ActionsCounts(obj);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if(ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    objType = new ActionCountsType();
                    objType.OverDue = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["OverDue_Response"]);
                    objType.Total = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["Total_Response"]);
                    objR.Response = objType;
                }
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    objType = new ActionCountsType();
                    objType.OverDue = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["OverDue_BriefToReg"]);
                    objType.Total = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["Total_BriefToReg"]);
                    objR.BriefToReg = objType;
                }
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    objType = new ActionCountsType();
                    objType.OverDue = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["OverDue_BriefToDoc"]);
                    objType.Total = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["Total_BriefToDoc"]);
                    objR.BriefToDoc = objType;
                }

                objR.StatusType = StatusType.SUCCESS;
                objR.MessageType = MessageType.NO_MESSAGE;
            }
            else
            {
                objR.StatusType = StatusType.FAILURE;
                objR.MessageType = MessageType.TRY_AGAIN;
            }

            return objR;
        }
    }
}
