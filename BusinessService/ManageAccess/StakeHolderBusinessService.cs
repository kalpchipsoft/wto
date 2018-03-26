using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BusinessObjects.ManageAccess;
using DataServices.ManageAccess;
using UtilitiesManagers;

namespace BusinessService.ManageAccess
{
    public class StakeHolderBusinessService
    {

        StakeHolderDataService objSDS = new StakeHolderDataService();
        public PageLoad_StakeHolderList StakeHoldersList()
        {
            CommonHelper objCH = new CommonHelper();
            PageLoad_StakeHolderList obj = new PageLoad_StakeHolderList();
            DataSet ds = objSDS.GetStakeHoldersList();
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<StakeHolderInfo> objStakeHolderList = new List<StakeHolderInfo>();
                    StakeHolderInfo objSHI = new StakeHolderInfo();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        objSHI = new StakeHolderInfo();
                        objSHI.ItemNumber = Convert.ToInt64(dr["ItemNumber"]);
                        objSHI.StakeHolderId = Convert.ToInt64(dr["StakeHolderId"]);
                        objSHI.FirstName = Convert.ToString(dr["FirstName"]);
                        objSHI.LastName = Convert.ToString(dr["LastName"]);
                        objSHI.Email = Convert.ToString(dr["EmailId"]);
                        objSHI.Mobile = Convert.ToString(dr["Mobile"]);
                        objSHI.Status = Convert.ToInt16(dr["Active"]);
                        objSHI.IsInUse = Convert.ToBoolean(dr["IsInUse"]);
                        objStakeHolderList.Add(objSHI);
                    }
                    obj.StakeHolderList = objStakeHolderList;
                }
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.TotalCount = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["TotalCount"]);
                }
            }
            return obj;
        }

        public StakeHolderInfo StakeHolderDetails(Int64 Id)
        {
            StakeHolderInfo obj = new StakeHolderInfo();
            CommonHelper objCH = new CommonHelper();
            DataSet ds = objSDS.GetStakeHolderDetails(Id);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.StakeHolderId = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["StakeHolderId"]);
                    obj.FirstName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["FirstName"]);
                    obj.LastName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["LastName"]);
                    obj.Email = Convert.ToString(ds.Tables[tblIndx].Rows[0]["EmailId"]);
                    obj.Mobile = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Mobile"]);
                    obj.Status = Convert.ToInt16(ds.Tables[tblIndx].Rows[0]["Active"]);
                    obj.OrgName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["OrgName"]);
                    obj.Address = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Address"]);
                    obj.City = Convert.ToString(ds.Tables[tblIndx].Rows[0]["City"]);
                    obj.State = Convert.ToString(ds.Tables[tblIndx].Rows[0]["State"]);
                    obj.PIN = Convert.ToString(ds.Tables[tblIndx].Rows[0]["PIN"]);
                }
            }
            return obj;
        }

        public bool AddStakeHolder(StakeHolderInfo obj)
        {
            return objSDS.AddStakeHolder(obj);
        }

        public bool DeleteStakeHolder(Int64 Id)
        {
            return objSDS.DeleteStakeHolder(Id);
        }
    }
}
