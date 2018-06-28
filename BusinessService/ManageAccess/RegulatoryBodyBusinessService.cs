using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataServices.ManageAccess;
using System.Data;
using BusinessObjects.ManageAccess;
using UtilitiesManagers;

namespace BusinessService.ManageAccess
{
    public  class RegulatoryBodyBusinessService
    {
        RegulatoryBodiesDataService objRBS = new RegulatoryBodiesDataService();
        public PageLoad_RegulatoryBodyList RegulatoryBodiesList()
        {
            CommonHelper objCH = new CommonHelper();
            PageLoad_RegulatoryBodyList obj = new PageLoad_RegulatoryBodyList();
            DataSet ds = objRBS.GetRegulatoryBodiesList();
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<RegulatoryBodies> RegulatoryBodiesList = new List<RegulatoryBodies>();
                    RegulatoryBodies objRB = new RegulatoryBodies();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        objRB = new RegulatoryBodies();
                        objRB.ItemNumber = Convert.ToInt64(dr["ItemNumber"]);
                        objRB.RegulatoryBodyId = Convert.ToInt64(dr["RegulatoryBodyId"]);
                        objRB.Name = Convert.ToString(dr["Name"]);
                        objRB.Status = Convert.ToInt16(dr["IsActive"]);
                        objRB.IsInUse = Convert.ToBoolean(dr["IsInUse"]);
                        objRB.Contact = Convert.ToString(dr["Contact"]);
                        objRB.Address = Convert.ToString(dr["Address"]);
                        objRB.Emailid = Convert.ToString(dr["Emailid"]);
                        RegulatoryBodiesList.Add(objRB);
                    }
                    obj.RegulatoryBodiesList = RegulatoryBodiesList;
                }
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.TotalCount = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["TotalCount"]);
                }
            }
            return obj;
        }

        public RegulatoryBodies RegulatoryBodiesDetails(Int64 Id)
        {
            RegulatoryBodies obj = new RegulatoryBodies();
            CommonHelper objCH = new CommonHelper();
            DataSet ds = objRBS.GetRegulatoryBodiesDetails(Id);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.RegulatoryBodyId = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["RegulatoryBodyId"]);
                    obj.Name = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Name"]);
                    obj.Status = Convert.ToInt16(ds.Tables[tblIndx].Rows[0]["IsActive"]);
                    obj.Emailid = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Emailid"]);
                    obj.Contact = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Contact"]);
                    obj.Address = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Address"]);
                }
            }
            return obj;
        }

        public bool AddRegulatoryBody(Int64 Id, AddRegulatoryBodies obj)
        {
            return objRBS.AddRegulatoryBodies(Id, obj);
        }

        public bool DeleteRegulatoryBody(Int64 Id)
        {
            return objRBS.DeleteRegulatoryBodies(Id);
        }
    }
}
