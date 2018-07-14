﻿using System;
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
                    List<TranslatorDetails> objTranslatorList = new List<TranslatorDetails>();
                    TranslatorDetails objTI = new TranslatorDetails();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        objTI = new TranslatorDetails();
                        objTI.ItemNumber = Convert.ToInt64(dr["ItemNumber"]);
                        objTI.TranslatorId = Convert.ToInt64(dr["TranslatorId"]);
                        objTI.FirstName = Convert.ToString(dr["FirstName"]);
                        objTI.LastName = Convert.ToString(dr["LastName"]);
                        objTI.Email = Convert.ToString(dr["EmailId"]);
                        objTI.Mobile = Convert.ToString(dr["Mobile"]);
                        objTI.Status = Convert.ToInt16(dr["IsActive"]);
                        objTI.IsInUse = Convert.ToBoolean(dr["IsInUse"]);
                        objTI.Languages = Convert.ToString(dr["Languages"]);
                        objTI.LanguageIds = Convert.ToString(dr["LanguageIds"]);
                        objTI.IsWelcomeMailSent = Convert.ToBoolean(dr["IsWelcomeMailSent"]);
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

        public TranslatorDetails TranslatorDetails(Int64 Id)
        {
            TranslatorDetails obj = new TranslatorDetails();
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
                    obj.Status = Convert.ToInt16(ds.Tables[tblIndx].Rows[0]["IsActive"]);
                    obj.Languages = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Languages"]);
                    obj.LanguageIds = Convert.ToString(ds.Tables[tblIndx].Rows[0]["LanguageIds"]);
                    obj.Password = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Password"]);
                    obj.IsWelcomeMailSent = Convert.ToBoolean(ds.Tables[tblIndx].Rows[0]["IsWelcomeMailSent"]);
                }
            }
            return obj;
        }

        public Int64 AddTranslator(Int64 Id, AddTranslator obj)
        {
            CommonHelper objCH = new CommonHelper();
            long RndmNumbr = objCH.GetRandomNumber(100000, 999999);
            obj.Password = objCH.EncryptData(RndmNumbr);
            DataTable dt = objTDS.AddTranslator(Id, obj);
            if (dt != null && dt.Rows.Count > 0)
                return Convert.ToInt64(dt.Rows[0]["Id"]);
            else
                return 0;
        }

        public bool DeleteTranslator(Int64 Id)
        {
            return objTDS.DeleteTranslator(Id);
        }

        public TranslatorDetails SendWelcomeMail(Int64 Id, Int32 UserId)
        {
            TranslatorDetails obj = new TranslatorDetails();
            CommonHelper objCH = new CommonHelper();
            DataSet ds = objTDS.SendWelcomeMail(Id,UserId);
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
                    obj.Status = Convert.ToInt16(ds.Tables[tblIndx].Rows[0]["IsActive"]);
                    obj.Languages = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Languages"]);
                    obj.LanguageIds = Convert.ToString(ds.Tables[tblIndx].Rows[0]["LanguageIds"]);
                    obj.Password = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Password"]);
                    obj.IsWelcomeMailSent = Convert.ToBoolean(ds.Tables[tblIndx].Rows[0]["IsWelcomeMailSent"]);
                }
            }
            return obj;
        }

        public string MailbodyForTranslator(TranslatorDetails obj)
        {
            CommonHelper objCh = new CommonHelper();
            StringBuilder strMailbody = new StringBuilder();
            strMailbody.Append("<table>");
            strMailbody.Append("<tr><td>Dear " + obj.TranslatorName + ",</td></tr>");
            strMailbody.Append("<tr><td><br/>We are pleased to inform you that, you have been registered as translator for " + obj.Languages + " for SPS / TBT Notifications Management System of Department of Commerce. <br/><br/>Given below are your user ID and Temporary Password for your use:<br/><br/>User ID : " + obj.Email + "<br/>Temporary Password : " + objCh.DecryptData(obj.Password) + "<br/>Website :  <a href='http://testwto.chipsoftindia.in/translator/Login/'>http://testwto.chipsoftindia.in/</a></td></tr>");
            strMailbody.Append("<tr><td><br/><br/>When you log-in using this temporary password, you would be asked to reset the password. Once you reset your password using the temporary password provided to you, the password will be stored in encrypted format and will not be visible to any users or will not be sent through email. In case you would like to retrieve your password, then you can request Department of Commerce to generate a temporary password for you by sending email at WTO.chipsoft@gmail.com. Using this temporary password you can again reset the Password.Please use Internet Explorer version 8 or above as your Internet browser for better results.</td></tr>");
            strMailbody.Append("<tr><td><br/><br/> This is a system generated email from testwto.chipsoftindia.in.</td></tr>");
            strMailbody.Append("<tr><td><br/>For any information / clarification please contact mishra.ashvini@chipsoftindia.in. </td></tr>");
            strMailbody.Append("<tr><td><br />Kind Regards,<br />WTO Team</td></tr>");
            strMailbody.Append("</table>");
            return strMailbody.ToString();
        }
    }
}
