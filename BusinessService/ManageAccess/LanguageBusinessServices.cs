using BusinessObjects.ManageAccess;
using DataServices.ManageAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BusinessService.ManageAccess
{
    public class LanguageBusinessServices
    {
        LanguageDataServices objServices = new LanguageDataServices();
        public LanguageList GetLanguageList()
        {
            // = new LanguageDetails();
            LanguageList objlst = new LanguageList();
            DataSet ds = objServices.GetLanguageList();
            if (ds!=null && ds.Tables.Count>0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<LanguageDetails> LanguageList = new List<LanguageDetails>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        LanguageDetails obj = new LanguageDetails();
                        obj.LanguageId = Convert.ToInt64(dr["LanguageId"]);
                        obj.Language = Convert.ToString(dr["Language"]);
                        obj.Status = Convert.ToInt16(dr["IsActive"]);
                        LanguageList.Add(obj);
                    }
                    objlst.languagelist = LanguageList;
                }
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    objlst.TotalCount = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["TotalCount"]);
                }
            }
            return objlst;
        }
        public LanguageDetails LanguageDetails(Int64 Id)
        {
            LanguageDetails obj = new LanguageDetails();        
            DataSet ds = objServices.GetLanguageDetails(Id);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.LanguageId = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["LanguageId"]);
                    obj.Language = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Language"]);
                    obj.Status = Convert.ToInt16(ds.Tables[tblIndx].Rows[0]["IsActive"]);
                }
            }
            return obj;
        }

        public bool AddLanguage(Int64 Id, LanguageDetails obj)
        {
            return objServices.AddLanguage(Id, obj);
        }

        public bool DeleteLanguage(Int64 Id)
        {
            return objServices.DeleteLanguage(Id);
        }
        public DataTable CheckDuplicateLanguageData(int id,string text)
        {
            return objServices.DuplicateLanguage(id, text);
        }
    }
}
