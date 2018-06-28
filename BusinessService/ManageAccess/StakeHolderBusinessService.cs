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
                        objSHI.StakeHolderName = Convert.ToString(dr["StakeHolderName"]);
                        objSHI.Email = Convert.ToString(dr["EmailId"]);
                        objSHI.Status = Convert.ToInt16(dr["IsActive"]);
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
                    obj.StakeHolderName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["StakeHolderName"]);
                    obj.Email = Convert.ToString(ds.Tables[tblIndx].Rows[0]["EmailId"]);
                    obj.Status = Convert.ToInt16(ds.Tables[tblIndx].Rows[0]["IsActive"]);
                    obj.OrgName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["OrgName"]);
                    obj.HSCodes = Convert.ToString(ds.Tables[tblIndx].Rows[0]["SelectedHSCodes"]);
                    obj.Designation = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Designation"]);
                }
            }
            return obj;
        }

        public bool AddStakeHolder(Int64 Id,AddStakeHolder obj)
        {
            return objSDS.AddStakeHolder(Id,obj);
        }

        public bool DeleteStakeHolder(Int64 Id)
        {
            return objSDS.DeleteStakeHolder(Id);
        }
    }
}
