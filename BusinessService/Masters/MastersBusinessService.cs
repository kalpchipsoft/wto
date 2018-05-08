using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataServices.Masters;
using BusinessObjects.ManageAccess;
using BusinessObjects.Masters;

namespace BusinessService.Masters
{
    public class MastersBusinessService
    {
        public DataTable GetHSCode()
        {
            MatersDataManager objMDM = new MatersDataManager();
            return objMDM.GetHSCodes();
        }

        public List<TranslatorInfo> GetTranslater(Int64 LanguageId)
        {
            List<TranslatorInfo> TranslatorList = new List<TranslatorInfo>();
            MatersDataManager objMDM = new MatersDataManager();
            DataSet ds = objMDM.GetTranslaters(LanguageId);
            if (ds != null && ds.Tables.Count > 0)
            {
                if (ds.Tables[0] != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (Convert.ToBoolean(dr["IsActive"]))
                        {
                            TranslatorInfo obj = new TranslatorInfo();
                            obj.TranslatorId = Convert.ToInt32(dr["TranslatorId"]);
                            obj.FirstName = Convert.ToString(dr["FirstName"]);
                            obj.LastName = Convert.ToString(dr["LastName"]);
                            TranslatorList.Add(obj);
                        }
                    }
                }
            }

            return TranslatorList;
        }

        public List<Language> GetLanguages()
        {
            List<Language> LanguageList = new List<Language>();
            MatersDataManager objMDM = new MatersDataManager();
            DataTable dt = objMDM.GetLanguages();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Language obj = new Language();
                    obj.LanguageId = Convert.ToInt32(dr["LanguageId"]);
                    obj.LanguageName = Convert.ToString(dr["Language"]);
                    LanguageList.Add(obj);
                }
            }

            return LanguageList;
        }

        public bool CheckIsEmailExists(string Email, string callFor)
        {
            MatersDataManager objMDM = new MatersDataManager();
            DataTable dt = objMDM.CheckIsEmailExists(Email,callFor);
            return Convert.ToBoolean(dt.Rows[0][0]);
        }
    }
}
