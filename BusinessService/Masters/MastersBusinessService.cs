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
        public List<HSCodes> GetHSCodeAutoComplete(string SearchText)
        {
            MatersDataManager objMDM = new MatersDataManager();
            List<HSCodes> objHsCodeList = new List<HSCodes>();
            foreach (DataRow dr in objMDM.GetHSCodesAutoComplete(SearchText).Rows)
            {
                HSCodes obj = new HSCodes();
                obj.HSCode = Convert.ToString(dr["id"]);
                obj.Text = Convert.ToString(dr["text"]);
                objHsCodeList.Add(obj);
            }
            return objHsCodeList;
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
            DataTable dt = objMDM.CheckIsEmailExists(Email, callFor);
            return Convert.ToBoolean(dt.Rows[0][0]);
        }

        public List<RegulatoryBodiesMaster> GetRegulatoryBodies()
        {
            List<RegulatoryBodiesMaster> RBList = new List<RegulatoryBodiesMaster>();
            MatersDataManager objMDM = new MatersDataManager();
            DataTable dt = objMDM.GetRegulatoryBodies();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    RegulatoryBodiesMaster obj = new RegulatoryBodiesMaster();
                    obj.RegulatoryBodyId = Convert.ToInt32(dr["RegulatoryBodyId"]);
                    obj.Name = Convert.ToString(dr["Name"]);
                    obj.Email = Convert.ToString(dr["EmailId"]);
                    obj.Address = Convert.ToString(dr["Address"]);
                    RBList.Add(obj);
                }
            }

            return RBList;
        }
        public List<InternalStakeHolderMaster> GetInternalStakeHolder()
        {
            List<InternalStakeHolderMaster> ISList = new List<InternalStakeHolderMaster>();
            MatersDataManager objMDM = new MatersDataManager();
            DataTable dt = objMDM.GetInternalStakeholder();
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    InternalStakeHolderMaster obj = new InternalStakeHolderMaster();
                    obj.InternalStakeHolderId = Convert.ToInt32(dr["InternalStakeHolderId"]);
                    obj.Name = Convert.ToString(dr["Name"]);
                    obj.Email = Convert.ToString(dr["EmailId"]);
                    obj.OrganisationName = Convert.ToString(dr["OrgName"]);
                    obj.Designation = Convert.ToString(dr["Designation"]);
                    ISList.Add(obj);
                }
            }

            return ISList;
        }

    }
}
