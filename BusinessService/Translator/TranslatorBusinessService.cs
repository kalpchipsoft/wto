using System;
using System.Data;
using BusinessObjects.Translator;
using UtilitiesManagers;
using DataServices.Translator;
using System.Collections.Generic;
using System.IO;

namespace BusinessService.Translator
{
    public class TranslatorBusinessService
    {
        public LoginResult ValidateTranslator(Login model)
        {
            LoginResult objR = new LoginResult();
            TranslatorDataService objTDS = new TranslatorDataService();
            DataTable dt = objTDS.ValidateTranslator(model.UserName, 0);
            if (dt != null && dt.Rows.Count > 0)
            {
                CommonHelper objCH = new CommonHelper();
                if (model.Password == objCH.DecryptData(Convert.ToString(dt.Rows[0]["Password"])))
                {
                    objR.TranslatorId = Convert.ToInt64(dt.Rows[0]["TranslatorId"]);
                    objR.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                    objR.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                    objR.LoginCount = Convert.ToInt32(dt.Rows[0]["LoginCount"]);

                    objR.StatusType = BusinessObjects.StatusType.SUCCESS;
                    objR.MessageType = BusinessObjects.MessageType.NO_MESSAGE;
                }
                else
                {
                    objR.StatusType = BusinessObjects.StatusType.FAILURE;
                    objR.MessageType = BusinessObjects.MessageType.WRONG_PASSWORD;
                }
            }
            else
            {
                objR.StatusType = BusinessObjects.StatusType.FAILURE;
                objR.MessageType = BusinessObjects.MessageType.WRONG_USERNAME;
            }
            return objR;
        }

        public int Login(long Id)
        {
            TranslatorDataService objTDS = new TranslatorDataService();
            return (objTDS.LoginTranslator(Id));
        }

        public BusinessObjects.CommonResponseModel ChangePassword(ChangePassword obj)
        {
            BusinessObjects.CommonResponseModel objR = new BusinessObjects.CommonResponseModel();
            TranslatorDataService objTDS = new TranslatorDataService();
            DataTable dt = objTDS.ValidateTranslator(null, obj.TranslatorId);
            if (dt != null && dt.Rows.Count > 0)
            {
                CommonHelper objCH = new CommonHelper();
                if (obj.OldPassword == objCH.DecryptData(Convert.ToString(dt.Rows[0]["Password"])))
                {
                    int Result = objTDS.UpdatePassword(obj.TranslatorId, objCH.EncryptData(obj.NewPassword));
                    if (Result > 0)
                    {
                        objR.StatusType = BusinessObjects.StatusType.SUCCESS;
                        objR.MessageType = BusinessObjects.MessageType.NO_MESSAGE;
                    }
                }
                else
                {
                    objR.StatusType = BusinessObjects.StatusType.FAILURE;
                    objR.MessageType = BusinessObjects.MessageType.WRONG_PASSWORD;
                }
            }
            else
            {
                objR.StatusType = BusinessObjects.StatusType.FAILURE;
                objR.MessageType = BusinessObjects.MessageType.WRONG_USERID;
            }
            return objR;
        }

        public List<NotificationDocument> Documents(SearchDocument obj)
        {
            List<NotificationDocument> DocumentList = new List<NotificationDocument>();
            TranslatorDataService objTDS = new TranslatorDataService();
            DataSet ds = objTDS.GetDocumentList(obj);
            if (ds != null)
            {
                int tblIndex = -1;

                tblIndex++;
                if (ds.Tables.Count > tblIndex)
                {
                    using (DataTable dt = ds.Tables[tblIndex])
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            NotificationDocument objND = new NotificationDocument();
                            objND.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                            objND.NotificationDocumentId = Convert.ToInt64(dr["NotificationDocumentId"]);
                            objND.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                            objND.SendToTranslaterOn = Convert.ToString(dr["SendToTranslaterOn"]);
                            objND.TranslationDueBy = Convert.ToString(dr["TranslationDueBy"]);

                            EditAttachment objE = new EditAttachment();
                            if (Convert.ToString(dr["NotificationDocumentName"]) != "")
                            {
                                objE.DisplayName = Convert.ToString(dr["NotificationDocumentName"]);
                                objE.FileName = Convert.ToString(dr["NotificationDocument"]);
                                objE.Path = "/Attachments/NotificationDocument/" + Convert.ToInt64(dr["NotificationDocumentId"]) + "_" + Convert.ToString(dr["NotificationDocument"]);
                                objND.UntranslatedDocument = objE;
                            }

                            objE = new EditAttachment();
                            if (Convert.ToString(dr["TranslatedDocumentName"]) != "")
                            {
                                objE.DisplayName = Convert.ToString(dr["TranslatedDocumentName"]);
                                objE.FileName = Convert.ToString(dr["TranslatedDocument"]);
                                objE.Path = "/Attachments/NotificationDocument_Translated/" + Convert.ToInt64(dr["NotificationDocumentId"]) + "_" + Convert.ToString(dr["TranslatedDocument"]);
                                objND.TranslatedDocument = objE;
                            }

                            DocumentList.Add(objND);
                        }
                    }
                }
            }
            return DocumentList;
        }

        public int SaveTranslatedDocument(long Id, UploadDocument obj)
        {
            TranslatorDataService objTDS = new TranslatorDataService();
            return (objTDS.SaveTranslatedDocument(Id, obj));
        }
    }
}
