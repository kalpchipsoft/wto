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
    public class TranslatorBusinessService
    {
        TranslatorDataService objTDS = new TranslatorDataService();
        public PageLoad_TranslatorList TranslatorsList()
        {
            CommonHelper objCH = new CommonHelper();
            PageLoad_TranslatorList obj = new PageLoad_TranslatorList();
            DataSet ds = objTDS.GetTranslatorsList();
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<TranslatorInfo> objTranslatorList = new List<TranslatorInfo>();
                    TranslatorInfo objTI = new TranslatorInfo();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        objTI = new TranslatorInfo();
                        objTI.ItemNumber = Convert.ToInt64(dr["ItemNumber"]);
                        objTI.TranslatorId = Convert.ToInt64(dr["TranslatorId"]);
                        objTI.FirstName = Convert.ToString(dr["FirstName"]);
                        objTI.LastName = Convert.ToString(dr["LastName"]);
                        objTI.Email = Convert.ToString(dr["EmailId"]);
                        objTI.Mobile = Convert.ToString(dr["Mobile"]);
                        objTI.Status = Convert.ToInt16(dr["Active"]);
                        objTI.IsInUse = Convert.ToBoolean(dr["IsInUse"]);
                        objTranslatorList.Add(objTI);
                    }
                    obj.TranslatorList = objTranslatorList;
                }
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.TotalCount = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["TotalCount"]);
                }
            }
            return obj;
        }

        public TranslatorInfo TranslatorDetails(Int64 Id)
        {
            TranslatorInfo obj = new TranslatorInfo();
            CommonHelper objCH = new CommonHelper();
            DataSet ds = objTDS.GetTranslatorDetails(Id);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.TranslatorId = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["TranslatorId"]);
                    obj.FirstName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["FirstName"]);
                    obj.LastName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["LastName"]);
                    obj.Email = Convert.ToString(ds.Tables[tblIndx].Rows[0]["EmailId"]);
                    obj.Mobile = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Mobile"]);
                    obj.Status = Convert.ToInt16(ds.Tables[tblIndx].Rows[0]["Active"]);
                }
            }
            return obj;
        }

        public bool AddTranslator(TranslatorInfo obj)
        {
            return objTDS.AddTranslator(obj);
        }

        public bool DeleteTranslator(Int64 Id)
        {
            return objTDS.DeleteTranslator(Id);
        }
    }
}
