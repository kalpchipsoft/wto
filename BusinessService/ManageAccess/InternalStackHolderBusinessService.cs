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
   public class InternalStackHolderBusinessService
    {
        InternalStackHolderDataService objISH = new InternalStackHolderDataService();
        
        public PageLoad_InternalStackHolderList InternalStackholdersList()
        {
            CommonHelper objCH = new CommonHelper();
            PageLoad_InternalStackHolderList obj = new PageLoad_InternalStackHolderList();
            DataSet ds = objISH.GetInternalStackHoldersList();
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<InternalStackHolder> InternalStackHolderList = new List<InternalStackHolder>();
                    InternalStackHolder objIS = new InternalStackHolder();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        objIS = new InternalStackHolder();
                        objIS.ItemNumber = Convert.ToInt64(dr["ItemNumber"]);
                        objIS.InternalStakeholdersId = Convert.ToInt64(dr["InternalStakeholdersId"]);
                        objIS.Name = Convert.ToString(dr["Name"]);
                        objIS.Status = Convert.ToInt16(dr["IsActive"]);
                        objIS.IsInUse = Convert.ToBoolean(dr["IsInUse"]);
                        objIS.OrgName = Convert.ToString(dr["OrgName"]);
                        objIS.Designation = Convert.ToString(dr["Designation"]);
                        objIS.Emailid = Convert.ToString(dr["Emailid"]);
                        InternalStackHolderList.Add(objIS);
                    }
                    obj.InternalStackHolderList = InternalStackHolderList;
                }
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.TotalCount = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["TotalCount"]);
                }
            }
            return obj;
        }

        public InternalStackHolder InternalStackholderDetails(Int64 Id)
        {
            InternalStackHolder obj = new InternalStackHolder();
            CommonHelper objCH = new CommonHelper();
            DataSet ds = objISH.GetInternalStackHoldersDetails(Id);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.InternalStakeholdersId = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["InternalStakeholdersId"]);
                    obj.Name = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Name"]);
                    obj.Status = Convert.ToInt16(ds.Tables[tblIndx].Rows[0]["IsActive"]);
                    obj.Emailid = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Emailid"]);
                    obj.OrgName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["OrgName"]);
                    obj.Designation = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Designation"]);
                }
            }
            return obj;
        }

        public bool AddInternalStackHolder(Int64 Id, AddInternalStackHolder obj)
        {
            return objISH.AddInternalStackHolders(Id, obj);
        }

        public bool DeleteInternalStackHolder(Int64 Id)
        {
            return objISH.DeleteInternalStackHolders(Id);
        }
    }
}
