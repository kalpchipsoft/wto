using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BusinessObjects;
using BusinessObjects.Masters;
using BusinessObjects.Notification;
using DataServices.WTO;
using System.IO;
using System.Web;
using iTextSharp.text;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using iTextSharp.text.pdf;
using Ionic.Zip;

namespace BusinessService.Notification
{
    public class NotificationBusinessService
    {
        public AddNoti_Result InsertUpdateNotification(AddNotification obj)
        {
            AddNoti_Result objR = new AddNoti_Result();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.InsertUpdate_Notification(obj);
            if (ds != null && ds.Tables.Count > 0)
            {
                objR.StatusType = StatusType.SUCCESS;
                objR.MessageType = MessageType.NO_MESSAGE;

                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    objR.NotificationId = Convert.ToInt64(ds.Tables[0].Rows[0]["NotificationId"]);
                    if (obj.NotificationAttachment != null && obj.NotificationAttachment.Content != "")
                    {
                        try
                        {
                            byte[] bytes = null;
                            if (obj.NotificationAttachment.Content.IndexOf(',') >= 0)
                            {
                                var myString = obj.NotificationAttachment.Content.Split(new char[] { ',' });
                                bytes = Convert.FromBase64String(myString[1]);
                            }
                            else
                                bytes = Convert.FromBase64String(obj.NotificationAttachment.Content);

                            if (obj.NotificationAttachment.FileName.Length > 0 && bytes.Length > 0)
                            {
                                string filePath = HttpContext.Current.Server.MapPath("/Attachments/NotificationAttachment/" + objR.NotificationId + "_" + obj.NotificationAttachment.FileName);
                                File.WriteAllBytes(filePath, bytes);
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            else
            {
                objR.StatusType = StatusType.FAILURE;
                objR.MessageType = MessageType.TRY_AGAIN;
            }

            return objR;
        }

        public EditNotification PageLoad_EditNotification(Int64 Id)
        {
            EditNotification objR = new EditNotification();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.Edit_Notification(Id);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;

                #region "Country Master"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<Country> CountryList = new List<Country>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        Country objCountry = new Country();
                        objCountry.CountryId = Convert.ToInt64(dr["CountryId"]);
                        objCountry.CountryCode = Convert.ToString(dr["CountryCode"]);
                        objCountry.Name = Convert.ToString(dr["Country"]);
                        CountryList.Add(objCountry);
                    }
                    objR.CountryList = CountryList;
                }
                #endregion

                #region "Stakeholders Master"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    int i = 1;
                    List<StakeHolderMaster> StakeHolderList = new List<StakeHolderMaster>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        StakeHolderMaster objStakeHolder = new StakeHolderMaster();
                        objStakeHolder.ItemNumber = i;
                        objStakeHolder.StakeHolderId = Convert.ToInt64(dr["StakeholderId"]);
                        objStakeHolder.FirstName = Convert.ToString(dr["FirstName"]);
                        objStakeHolder.LastName = Convert.ToString(dr["LastName"]);
                        objStakeHolder.HSCodes = Convert.ToString(dr["HSCodes"]);
                        i++;
                        StakeHolderList.Add(objStakeHolder);
                    }
                    objR.StakeHoldersList = StakeHolderList;
                }
                #endregion

                #region "Notification Details"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        EditAttachment objF = new EditAttachment();
                        objR.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objR.NotificationType = Convert.ToString(dr["NotificationType"]);
                        objR.NotificationStatus = Convert.ToInt16(dr["NotificationStatus"]);
                        objR.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        objR.DateofNotification = Convert.ToString(dr["DateOfNotification"]);
                        objR.FinalDateOfComments = Convert.ToString(dr["FinalDateOfComment"]);
                        objR.SendResponseBy = Convert.ToString(dr["SendResponseBy"]);
                        objR.CountryId = Convert.ToInt32(dr["CountryId"]);
                        objR.Title = Convert.ToString(dr["Title"]);
                        objR.ResponsibleAgency = Convert.ToString(dr["AgencyResponsible"]);
                        objR.Articles = Convert.ToString(dr["UnderArticle"]);
                        objR.ProductsCovered = Convert.ToString(dr["ProductCovered"]);
                        objR.Description = Convert.ToString(dr["Description"]);
                        objR.HSCodes = Convert.ToString(dr["HSCode"]);
                        objR.EnquiryEmailId = Convert.ToString(dr["EnquiryEmailId"]);
                        objR.EnquiryEmailSentOn = Convert.ToString(dr["MailSentToEnquiryDeskOn"]);
                        if (Convert.ToString(dr["NotificationAttachment"]) != "")
                        {
                            objF.FileName = Convert.ToString(dr["NotificationAttachment"]);
                            objF.Path = "/Attachments/NotificationAttachment/" + Id + "_" + Convert.ToString(dr["NotificationAttachment"]);
                            objR.NotificationAttachment = objF;
                        }

                        objR.DoesHaveDetails = dr["DoesHaveDetails"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dr["DoesHaveDetails"]);

                        objF = new EditAttachment();
                        if (Convert.ToString(dr["NotificationDocument"]) != "")
                        {
                            objF.FileName = Convert.ToString(dr["NotificationDocumentName"]);
                            objF.Path = "/Attachments/NotificationDocument/" + Id + "_" + Convert.ToString(dr["NotificationDocument"]);
                            objR.NotificationDoc = objF;
                        }
                        objR.NotificationDocName = Convert.ToString(dr["NotificationDocumentName"]);
                        objR.ObtainDocBy = Convert.ToString(dr["ObtainDocumentBy"]);
                        objR.LanguageId = Convert.ToInt32(dr["LanguageId"]);
                        objR.TranslaterId = Convert.ToInt32(dr["TranslatorId"]);
                        objR.RemainderToTranslaterOn = Convert.ToString(dr["TranslationReminder"]);
                        objR.TranslationDueOn = Convert.ToString(dr["TranslationDueBy"]);

                        if (Convert.ToString(dr["TranslatedDocument"]) != "")
                        {
                            objF = new EditAttachment();
                            objF.FileName = Convert.ToString(dr["TranslatedDocumentName"]);
                            objF.Path = "/Attachments/NotificationDocument_Translated/" + Id + "_" + Convert.ToString(dr["TranslatedDocument"]);
                            objR.TranslatedDoc = objF;
                        }
                        objR.TranslatedDocName = Convert.ToString(dr["TranslatedDocumentName"]);
                        objR.SentToTranslaterOn = Convert.ToString(dr["SendToTranslaterOn"]);
                        objR.TranslatedDocUploadedOn = Convert.ToString(dr["TranslatedDocUploadedOn"]);
                        objR.SkippedToDiscussion = dr["SkippedToDiscussion"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dr["SkippedToDiscussion"]);
                        objR.Status = Convert.ToInt32(dr["Status"]);
                        objR.Stakeholders = Convert.ToString(dr["SelectedStakeholders"]);
                        objR.StakeholderResponseDueBy = Convert.ToString(dr["StakeholderResponseDueBy"]);
                        objR.NotificationDiscussedOn = Convert.ToString(dr["NotificationDiscussedOn"]);
                    }
                }
                #endregion

                #region "HSCodes Master"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<HSCodes> SelectedHSCodesList = new List<HSCodes>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        HSCodes objHSCodes = new HSCodes();
                        objHSCodes.HSCode = Convert.ToString(dr["HSCode"]);
                        objHSCodes.Text = Convert.ToString(dr["Description"]);
                        SelectedHSCodesList.Add(objHSCodes);
                    }
                    objR.SelectedHSCodes = SelectedHSCodesList;
                }
                #endregion

                #region "Notification Status"
                List<NotificationStatus> NotificationStatusList = new List<NotificationStatus>();
                NotificationStatus objNotificationStatus = new NotificationStatus();
                objNotificationStatus.Id = 1;
                objNotificationStatus.Type = "Draft";
                NotificationStatusList.Add(objNotificationStatus);
                objNotificationStatus = new NotificationStatus();
                objNotificationStatus.Id = 2;
                objNotificationStatus.Type = "Final";
                NotificationStatusList.Add(objNotificationStatus);
                objR.NotificationStatusList = NotificationStatusList;
                #endregion
            }
            return objR;
        }

        public List<RelatedStakeHolders> GetNotificationStakeholders(Int64 Id)
        {
            List<RelatedStakeHolders> RelatedStakeHoldersList = new List<RelatedStakeHolders>();
            NotificationDataManager objDM = new NotificationDataManager();
            DataTable dt = objDM.RelatedStakeholders(Id);
            if (dt != null && dt.Rows.Count > 0)
            {
                int i = 1;
                string hdnRelatedStakeHolder = string.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    RelatedStakeHolders objStakeHolder = new RelatedStakeHolders();
                    objStakeHolder.ItemNumber = i;
                    objStakeHolder.StakeHolderId = Convert.ToInt64(dr["StakeholderId"]);
                    objStakeHolder.FirstName = Convert.ToString(dr["FirstName"]);
                    objStakeHolder.LastName = Convert.ToString(dr["LastName"]);
                    objStakeHolder.HSCodes = Convert.ToString(dr["HSCodes"]);
                    objStakeHolder.MailCount = Convert.ToInt32(dr["MailCount"]);
                    objStakeHolder.ResponseCount = Convert.ToInt32(dr["ResponseCount"]);
                    hdnRelatedStakeHolder += ',' + Convert.ToString(dr["StakeholderId"]);
                    i++;
                    RelatedStakeHoldersList.Add(objStakeHolder);
                }
            }
            return RelatedStakeHoldersList;
        }

        public AddNoti_Result InsertDeleteRelatedStakeholders(Int64 Id, string SelectedStakeHolder, bool IsDelete)
        {
            AddNoti_Result objR = new AddNoti_Result();
            NotificationDataManager objDM = new NotificationDataManager();
            DataTable dt = objDM.InsertDelete_RelatedStakeholders(Id, SelectedStakeHolder, IsDelete);
            if (dt != null && dt.Rows.Count > 0)
            {
                objR.StatusType = StatusType.SUCCESS;
                objR.MessageType = MessageType.NO_MESSAGE;
            }
            else
            {
                objR.StatusType = StatusType.FAILURE;
                objR.MessageType = MessageType.TRY_AGAIN;
            }

            return objR;
        }

        public SendToTranslater_Output SendDocumentToTranslater(SendToTranslater obj)
        {
            SendToTranslater_Output objOutput = new SendToTranslater_Output();
            NotificationDataManager objDM = new NotificationDataManager();
            DataTable dt = objDM.SendDocumentToTranslater(obj);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    objOutput.TranslatorId = Convert.ToInt64(dr["TranslatorId"]);
                    objOutput.FirstName = Convert.ToString(dr["FirstName"]);
                    objOutput.LastName = Convert.ToString(dr["LastName"]);
                    objOutput.TranslaterEmailId = Convert.ToString(dr["EmailId"]);
                    objOutput.Language = Convert.ToString(dr["Language"]);
                    objOutput.CreatedBy = Convert.ToString(dr["CreatedBy"]);
                    objOutput.CreatorEmailId = Convert.ToString(dr["CreaterEmailId"]);
                    objOutput.TranslationDueOn = Convert.ToString(dr["TranslationDueOn"]);
                }
            }
            return objOutput;
        }

        public string MailbodyForTranslater(SendToTranslater_Output obj)
        {
            StringBuilder strMailbody = new StringBuilder();
            //Send Mail to Creator
            strMailbody.Append("<table style='text-align : left;font-family : Arial; font-size : 10pt;'>");
            strMailbody.Append("<tr><td>Dear " + obj.TranslatorName + ",<br/><br/></td></tr>");
            strMailbody.Append("<tr><td>" + obj.CreatedBy + " has been send a document in " + obj.Language.Trim() + " language for translation .</td></tr>");
            strMailbody.Append("<tr><td>Please Translate and send the translated document till date " + obj.TranslationDueOn + " at " + obj.CreatorEmailId + "</td></tr>");
            strMailbody.Append("<tr><td><br/>Click <a href='http://testwto.chipsoftindia.in/translator/Login/Email?" + obj.TranslaterEmailId + "' target='_new'>here</a> for login to upload translated document.</td></tr>");
            strMailbody.Append("<tr><td><br/>Regards,<br/>WTO Team</td></tr></table>");
            return strMailbody.ToString();
        }

        public SendMailStakeholders_Output SaveAndSendMailToStakeholders(SendMailStakeholders obj)
        {
            SendMailStakeholders_Output objOutput = new SendMailStakeholders_Output();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.SendMailToStakeHolders(obj);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndex = -1;

                tblIndex++;
                if (ds.Tables.Count > tblIndex)
                {
                    using (DataTable dt = ds.Tables[tblIndex])
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                StackholderMail objM = new StackholderMail();
                                objM.MailId = Convert.ToInt32(dr["MailId"]);
                                objM.StakeholderCount = Convert.ToInt32(dr["StakeholderCount"]);
                                objM.Subject = Convert.ToString(dr["Subject"]);
                                objM.Message = Convert.ToString(dr["Message"]);
                                objOutput.MailDetails = objM;
                            }
                        }
                    }
                }

                tblIndex++;
                if (ds.Tables.Count > tblIndex)
                {
                    using (DataTable dt = ds.Tables[tblIndex])
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            List<BusinessObjects.ManageAccess.StakeHolder> StakeholderList = new List<BusinessObjects.ManageAccess.StakeHolder>();
                            foreach (DataRow dr in dt.Rows)
                            {
                                BusinessObjects.ManageAccess.StakeHolder objS = new BusinessObjects.ManageAccess.StakeHolder();
                                objS.StakeHolderId = Convert.ToInt32(dr["StakeholderId"]);
                                objS.FirstName = Convert.ToString(dr["FirstName"]);
                                objS.LastName = Convert.ToString(dr["LastName"]);
                                objS.Email = Convert.ToString(dr["EmailId"]);
                                StakeholderList.Add(objS);
                            }

                            objOutput.StakeHolders = StakeholderList;
                        }
                    }
                }

            }
            return objOutput;
        }

        public string MailbodyForStakeholders(StackholderMail obj)
        {
            StringBuilder strMailbody = new StringBuilder();
            strMailbody.Append("<table style='text-align : left;font-family : Arial; font-size : 10pt;'>");
            strMailbody.Append("<tr><td>Dear #Name#,<br/><br/></td></tr>");
            strMailbody.Append("<tr><td>" + obj.Message + ".</td></tr>");
            strMailbody.Append("<tr><td><br/>Regards,<br/>WTO Team</td></tr></table>");
            return strMailbody.ToString();
        }

        public SendMail_Output SaveAndSendMailToEnquiryDesk(long Id, SendMailToEnquiryDesk obj)
        {
            SendMail_Output objOutput = new SendMail_Output();
            NotificationDataManager objDM = new NotificationDataManager();
            DataTable dt = objDM.SendMailToEnquiryDesk(Id, obj);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    NotificationMail objMail = new NotificationMail();
                    objMail.MailId = Convert.ToInt32(dr["MailId"]);
                    objMail.Subject = Convert.ToString(dr["Subject"]);
                    objMail.Body = Convert.ToString(dr["Message"]);
                    objOutput.MailDetails = objMail;

                    objOutput.MailTo = Convert.ToString(dr["MailTo"]);
                    objOutput.ReplyTo = Convert.ToString(dr["ReplyTo"]);
                    objOutput.CC = Convert.ToString(dr["CC"]);
                    objOutput.BCC = Convert.ToString(dr["BCC"]);
                    objOutput.DisplayName = Convert.ToString(dr["DisplayName"]);
                }
            }
            return objOutput;
        }

        private NotificationDetails_Pdf GetNotificationDetails_forPDF(long Id)
        {
            NotificationDetails_Pdf obj = new NotificationDetails_Pdf();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.GetDetails_Notification(Id);
            if (ds != null)
            {
                int tblIndex = -1;

                #region "Notification Details"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex] != null && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        EditAttachment objF = new EditAttachment();
                        obj.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        obj.NotificationType = Convert.ToString(dr["NotificationType"]);
                        obj.NotificationStatus = Convert.ToString(dr["NotificationStatus"]);
                        obj.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        obj.DateOfNotification = Convert.ToString(dr["DateOfNotification"]);
                        obj.FinalDateOfComment = Convert.ToString(dr["FinalDateOfComment"]);
                        obj.SendResponseBy = Convert.ToString(dr["SendResponseBy"]);
                        obj.Country = Convert.ToString(dr["Country"]);
                        obj.Title = Convert.ToString(dr["Title"]);
                        obj.ResponsibleAgency = Convert.ToString(dr["AgencyResponsible"]);
                        obj.Articles = Convert.ToString(dr["UnderArticle"]);
                        obj.ProductCovered = Convert.ToString(dr["ProductCovered"]);
                        obj.Description = Convert.ToString(dr["Description"]);

                        obj.CreatedBy = Convert.ToString(dr["CreatedBy"]);
                        obj.CreatedOn = Convert.ToString(dr["CreatedOn"]);
                        obj.DoesHaveDetails = dr["DoesHaveDetails"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dr["DoesHaveDetails"]);

                        obj.EnquiryEmailId = Convert.ToString(dr["EnquiryEmailId"]);
                        obj.MailSentToEnquiryDeskOn = Convert.ToString(dr["MailSentToEnquiryDeskOn"]);

                        obj.ObtainDocumentBy = Convert.ToString(dr["ObtainDocumentBy"]);
                        obj.Language = Convert.ToString(dr["Language"]);
                        obj.Translator = Convert.ToString(dr["Translator"]);
                        obj.SendToTranslaterOn = Convert.ToString(dr["SendToTranslaterOn"]);
                        obj.TranslationReminder = Convert.ToString(dr["TranslationReminder"]);

                        obj.TranslationDueBy = Convert.ToString(dr["TranslationDueBy"]);
                        obj.TranslatedDocUploadedOn = Convert.ToString(dr["TranslatedDocUploadedOn"]);
                        obj.StakeholderResponseDueBy = Convert.ToString(dr["StakeholderResponseDueBy"]);
                        obj.NotificationDiscussedOn = Convert.ToString(dr["NotificationDiscussedOn"]);
                    }
                }
                #endregion

                #region "Notification Related HS Codes"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex] != null && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<HSCodes> HSCodeList = new List<HSCodes>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        HSCodes objHSCode = new HSCodes();
                        objHSCode.HSCode = Convert.ToString(dr["HSCode"]);
                        objHSCode.Text = Convert.ToString(dr["Description"]);
                        HSCodeList.Add(objHSCode);
                    }
                    obj.HSCodes = HSCodeList;
                }
                #endregion

                #region "Notification Related Stackholders"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex] != null && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<RelatedStakeHolders> StakeHolderList = new List<RelatedStakeHolders>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        RelatedStakeHolders objStakeHolder = new RelatedStakeHolders();
                        objStakeHolder.StakeHolderId = Convert.ToInt32(dr["StakeholderId"]);
                        objStakeHolder.FirstName = Convert.ToString(dr["FirstName"]);
                        objStakeHolder.LastName = Convert.ToString(dr["LastName"]);
                        objStakeHolder.HSCodes = Convert.ToString(dr["HSCodes"]);
                        objStakeHolder.MailCount = Convert.ToInt32(dr["MailCount"]);
                        objStakeHolder.ResponseCount = Convert.ToInt32(dr["ResponseCount"]);
                        StakeHolderList.Add(objStakeHolder);
                    }
                    obj.Stakholders = StakeHolderList;
                }
                #endregion

                #region "Notification Related Stackholder Mail Details"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex] != null && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<StackholderMail> MailList = new List<StackholderMail>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        StackholderMail objM = new StackholderMail();
                        objM.MailId = Convert.ToInt32(dr["MailId"]);
                        objM.Subject = Convert.ToString(dr["Subject"]);
                        objM.Message = Convert.ToString(dr["Message"]);
                        objM.MailSentDate = Convert.ToString(dr["MailSentOn"]);
                        objM.StakeholderCount = Convert.ToInt32(dr["StakeholderCount"]);
                        MailList.Add(objM);
                    }
                    obj.StackholderMails = MailList;
                }
                #endregion

                #region "Notification Related documents"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex] != null && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<EditAttachment> DocumentList = new List<EditAttachment>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        EditAttachment objDoc = new EditAttachment();
                        objDoc.FileName = Convert.ToString(dr["DocumentName"]);
                        objDoc.Path = Convert.ToString(dr["DocumentPath"]);
                        DocumentList.Add(objDoc);
                    }
                    obj.Documents = DocumentList;
                }
                #endregion
            }
            return obj;
        }
        private string GeneratePdf(NotificationDetails_Pdf obj)
        {
            string FileName = "";
            if (obj != null)
            {
                #region "PDF declaration"
                FileName = Convert.ToString(obj.NotificationId) + "_NotificationDetails.pdf";
                string FilePath = HttpContext.Current.Server.MapPath("/Attachments/Temp/" + FileName);

                if (File.Exists(FileName))
                    File.Delete(FileName);

                int leading = 9;
                Font fntHeader = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.WHITE);
                Font fntHeader1 = FontFactory.GetFont("Arial", 9, Font.BOLD, BaseColor.BLACK);
                Font fnttext = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
                Font fnttext1 = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
                Font XS = FontFactory.GetFont("Arial", 7, Font.NORMAL, BaseColor.BLACK);
                Font blank = FontFactory.GetFont("Background-color", 1, Font.NORMAL, BaseColor.WHITE);
                Font fntLabel = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);
                float[] width = new float[] { 515f };
                float pageSize = PageSize.A4.Width + 80;
                Document document = new Document(PageSize.A4, 30, 30, 60f, 50f);
                int[] widthsExport = new int[4];
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(FilePath, FileMode.Create));
                document.Open();

                PdfPTable HeaderTable = new PdfPTable(2);
                HeaderTable.WidthPercentage = 100;

                Paragraph para = new Paragraph();
                PdfPTable POTable;
                PdfPCell cell = new PdfPCell();
                #endregion

                #region "BackGroundImage"
                //Image jpgBack = Image.GetInstance(@"C:\Website\mycii\Image\certificate-bg.jpg");
                //cell.BackgroundColor = new BaseColor(255, 255, 255);
                //jpgBack.SetAbsolutePosition(0, 0);
                //jpgBack.ScaleAbsolute(PageSize.A4.Rotate().Width, PageSize.A4.Rotate().Height);
                //document.Add(jpgBack);
                #endregion

                #region "Logo"
                cell = new PdfPCell();
                Image Logo = Image.GetInstance(HttpContext.Current.Server.MapPath("/contents/img/wto-logo.png"));
                cell.AddElement(Logo);
                cell.Border = 0;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                HeaderTable.AddCell(cell);
                document.Add(HeaderTable);
                #endregion

                #region "PDF Body"
                #region "Notification Basic details"

                POTable = new PdfPTable(1);
                POTable.TotalWidth = 515.433070866f;
                POTable.LockedWidth = true;
                POTable.SpacingAfter = 10.185f;
                POTable.DefaultCell.Border = 0;
                POTable.AddCell(new PdfPCell(new Phrase(leading, "Notification", fntLabel)) { BackgroundColor = BaseColor.BLUE });
                document.Add(POTable);

                POTable = new PdfPTable(2);
                POTable.TotalWidth = 515.433070866f;
                POTable.LockedWidth = true;
                POTable.SpacingAfter = 10.185f;
                POTable.DefaultCell.Border = 0;

                #region "Row : Notification Type"
                cell = new PdfPCell();
                para = new Paragraph("Notification Type", fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.NotificationType), fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Notification Status"
                cell = new PdfPCell();
                para = new Paragraph("Notification Status", fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.NotificationStatus), fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Notification Number"
                cell = new PdfPCell();
                para = new Paragraph("Notification Number", fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.NotificationNumber), fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Date of Notification"
                cell = new PdfPCell();
                para = new Paragraph("Date of Notification", fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.DateOfNotification), fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Final Date of Comment"
                cell = new PdfPCell();
                para = new Paragraph("Final Date of Comment", fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.FinalDateOfComment), fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Send Response By"
                cell = new PdfPCell();
                para = new Paragraph("Send Response By", fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.SendResponseBy), fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Country"
                cell = new PdfPCell();
                para = new Paragraph("Country", fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.Country), fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Title"
                cell = new PdfPCell();
                para = new Paragraph("Title", fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.Title), fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Agency Responsible"
                cell = new PdfPCell();
                para = new Paragraph("Responsible Agency", fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.ResponsibleAgency), fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Under Article"
                cell = new PdfPCell();
                para = new Paragraph("Article", fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.Articles), fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Product Covered"
                cell = new PdfPCell();
                para = new Paragraph("Products Covered", fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.ProductCovered), fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);
                #endregion

                #region "Row : HS Codes"
                if (obj.HSCodes != null)
                {
                    cell = new PdfPCell();
                    para = new Paragraph("HS Codes", fnttext);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    para = new Paragraph(HttpUtility.HtmlDecode(":   "), fnttext);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    cell.Colspan = 6;
                    cell.FixedHeight = 15;
                    POTable.AddCell(cell);
                    document.Add(POTable);

                    PdfPTable tblHSCodes = new PdfPTable(2);
                    tblHSCodes.TotalWidth = 500.433070866f;
                    tblHSCodes.LockedWidth = true;

                    tblHSCodes.AddCell(new PdfPCell(new Phrase(leading, "HS Code", fnttext)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblHSCodes.AddCell(new PdfPCell(new Phrase(leading, "Description", fnttext)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    foreach (HSCodes hs in obj.HSCodes)
                    {
                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(hs.HSCode), fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblHSCodes.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(hs.Text), fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblHSCodes.AddCell(cell);
                    }
                    tblHSCodes.SpacingAfter = 20f;
                    document.Add(tblHSCodes);

                    POTable = new PdfPTable(2);
                    POTable.TotalWidth = 515.433070866f;
                    POTable.LockedWidth = true;
                    POTable.SpacingAfter = 10.185f;
                    POTable.DefaultCell.Border = 0;
                }
                #endregion

                #region "Row : Description"
                cell = new PdfPCell();
                para = new Paragraph("Description", fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.Description), fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Created By"
                cell = new PdfPCell();
                para = new Paragraph("Submitted by", fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.CreatedBy), fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Created On"
                cell = new PdfPCell();
                para = new Paragraph("Submitted On", fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.CreatedOn), fnttext);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.FixedHeight = 15;
                POTable.AddCell(cell);
                #endregion

                POTable.SpacingAfter = 20f;
                document.Add(POTable);
                #endregion

                #region "Document Section"
                if (obj.DoesHaveDetails != null)
                {
                    POTable = new PdfPTable(1);
                    POTable.TotalWidth = 515.433070866f;
                    POTable.LockedWidth = true;
                    POTable.SpacingAfter = 10.185f;
                    POTable.DefaultCell.Border = 0;
                    POTable.AddCell(new PdfPCell(new Phrase(leading, "Document", fntLabel)) { BackgroundColor = BaseColor.BLUE });
                    document.Add(POTable);

                    POTable = new PdfPTable(2);
                    POTable.TotalWidth = 515.433070866f;
                    POTable.LockedWidth = true;
                    POTable.SpacingAfter = 10.185f;
                    POTable.DefaultCell.Border = 0;
                    #region "Row : Have Detailed Notification"
                    cell = new PdfPCell();
                    para = new Paragraph("Have detailed notification provided?", fnttext);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    cell.FixedHeight = 15;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    para = new Paragraph(HttpUtility.HtmlDecode(":   " + (Convert.ToBoolean(obj.DoesHaveDetails) ? "Yes" : "No")), fnttext);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    cell.Border = 0;
                    cell.Colspan = 6;
                    cell.FixedHeight = 15;
                    POTable.AddCell(cell);
                    #endregion

                    if (Convert.ToBoolean(obj.DoesHaveDetails))
                    {
                        #region "Row : Language"
                        cell = new PdfPCell();
                        para = new Paragraph("Notification Document in Language", fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Border = 0;
                        cell.FixedHeight = 15;
                        POTable.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.Language), fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Border = 0;
                        cell.Border = 0;
                        cell.Colspan = 6;
                        cell.FixedHeight = 15;
                        POTable.AddCell(cell);
                        #endregion

                        if (obj.Language.ToLower() != "english" && obj.Translator != "")
                        {
                            #region "Row : Tranlsator"
                            cell = new PdfPCell();
                            para = new Paragraph("Translator", fnttext);
                            para.SetLeading(leading, 0);
                            cell.AddElement(para);
                            cell.Border = 0;
                            cell.FixedHeight = 15;
                            POTable.AddCell(cell);

                            cell = new PdfPCell();
                            para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.Translator), fnttext);
                            para.SetLeading(leading, 0);
                            cell.AddElement(para);
                            cell.Border = 0;
                            cell.Border = 0;
                            cell.Colspan = 6;
                            cell.FixedHeight = 15;
                            POTable.AddCell(cell);
                            #endregion

                            #region "Row : Sent to tranlsator on"
                            cell = new PdfPCell();
                            para = new Paragraph("Sent to translator for translation on ", fnttext);
                            para.SetLeading(leading, 0);
                            cell.AddElement(para);
                            cell.Border = 0;
                            cell.FixedHeight = 15;
                            POTable.AddCell(cell);

                            cell = new PdfPCell();
                            para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.SendToTranslaterOn), fnttext);
                            para.SetLeading(leading, 0);
                            cell.AddElement(para);
                            cell.Border = 0;
                            cell.Border = 0;
                            cell.Colspan = 6;
                            cell.FixedHeight = 15;
                            POTable.AddCell(cell);
                            #endregion

                            #region "Row : Remainder to tranlsator on"
                            cell = new PdfPCell();
                            para = new Paragraph("Remainder to translator for translation on ", fnttext);
                            para.SetLeading(leading, 0);
                            cell.AddElement(para);
                            cell.Border = 0;
                            cell.FixedHeight = 15;
                            POTable.AddCell(cell);

                            cell = new PdfPCell();
                            para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.TranslationReminder), fnttext);
                            para.SetLeading(leading, 0);
                            cell.AddElement(para);
                            cell.Border = 0;
                            cell.Border = 0;
                            cell.Colspan = 6;
                            cell.FixedHeight = 15;
                            POTable.AddCell(cell);
                            #endregion

                            #region "Row : Translation Due Date"
                            cell = new PdfPCell();
                            para = new Paragraph("Translation Due Date ", fnttext);
                            para.SetLeading(leading, 0);
                            cell.AddElement(para);
                            cell.Border = 0;
                            cell.FixedHeight = 15;
                            POTable.AddCell(cell);

                            cell = new PdfPCell();
                            para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.TranslationDueBy), fnttext);
                            para.SetLeading(leading, 0);
                            cell.AddElement(para);
                            cell.Border = 0;
                            cell.Border = 0;
                            cell.Colspan = 6;
                            cell.FixedHeight = 15;
                            POTable.AddCell(cell);
                            #endregion

                            #region "Row : Translation Due Date"
                            cell = new PdfPCell();
                            para = new Paragraph("Translation expected on ", fnttext);
                            para.SetLeading(leading, 0);
                            cell.AddElement(para);
                            cell.Border = 0;
                            cell.FixedHeight = 15;
                            POTable.AddCell(cell);

                            cell = new PdfPCell();
                            para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.TranslationDueBy), fnttext);
                            para.SetLeading(leading, 0);
                            cell.AddElement(para);
                            cell.Border = 0;
                            cell.Border = 0;
                            cell.Colspan = 6;
                            cell.FixedHeight = 15;
                            POTable.AddCell(cell);
                            #endregion

                            #region "Row : Translated doc received on"
                            if (obj.TranslatedDocUploadedOn != "")
                            {
                                cell = new PdfPCell();
                                para = new Paragraph("Translated document received on ", fnttext);
                                para.SetLeading(leading, 0);
                                cell.AddElement(para);
                                cell.Border = 0;
                                cell.FixedHeight = 15;
                                POTable.AddCell(cell);

                                cell = new PdfPCell();
                                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.TranslatedDocUploadedOn), fnttext);
                                para.SetLeading(leading, 0);
                                cell.AddElement(para);
                                cell.Border = 0;
                                cell.Border = 0;
                                cell.Colspan = 6;
                                cell.FixedHeight = 15;
                                POTable.AddCell(cell);
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region "Row : Enquiry desk email-id"
                        cell = new PdfPCell();
                        para = new Paragraph("Enquiry desk email-id ", fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Border = 0;
                        cell.FixedHeight = 15;
                        POTable.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.EnquiryEmailId), fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Border = 0;
                        cell.Border = 0;
                        cell.Colspan = 6;
                        cell.FixedHeight = 15;
                        POTable.AddCell(cell);
                        #endregion

                        #region "Row : Enquiry desk email sent on "
                        if (obj.MailSentToEnquiryDeskOn != "")
                        {
                            cell = new PdfPCell();
                            para = new Paragraph("Mail sent to enquiry desk on ", fnttext);
                            para.SetLeading(leading, 0);
                            cell.AddElement(para);
                            cell.Border = 0;
                            cell.FixedHeight = 15;
                            POTable.AddCell(cell);

                            cell = new PdfPCell();
                            para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.MailSentToEnquiryDeskOn), fnttext);
                            para.SetLeading(leading, 0);
                            cell.AddElement(para);
                            cell.Border = 0;
                            cell.Border = 0;
                            cell.Colspan = 6;
                            cell.FixedHeight = 15;
                            POTable.AddCell(cell);
                        }
                        #endregion

                        #region "Row : Enquiry desk email sent on "
                        if (obj.ObtainDocumentBy != "")
                        {
                            cell = new PdfPCell();
                            para = new Paragraph("Document obtained on ", fnttext);
                            para.SetLeading(leading, 0);
                            cell.AddElement(para);
                            cell.Border = 0;
                            cell.FixedHeight = 15;
                            POTable.AddCell(cell);

                            cell = new PdfPCell();
                            para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.ObtainDocumentBy), fnttext);
                            para.SetLeading(leading, 0);
                            cell.AddElement(para);
                            cell.Border = 0;
                            cell.Border = 0;
                            cell.Colspan = 6;
                            cell.FixedHeight = 15;
                            POTable.AddCell(cell);
                        }
                        #endregion
                    }
                    POTable.SpacingAfter = 20f;
                    document.Add(POTable);
                }
                #endregion

                #region "Notification related Stakeholders"
                if (obj.Stakholders != null)
                {
                    POTable = new PdfPTable(1);
                    POTable.TotalWidth = 515.433070866f;
                    POTable.LockedWidth = true;
                    POTable.SpacingAfter = 10.185f;
                    POTable.DefaultCell.Border = 0;
                    POTable.AddCell(new PdfPCell(new Phrase(leading, "Stakeholders", fntLabel)) { BackgroundColor = BaseColor.BLUE });
                    document.Add(POTable);

                    PdfPTable tblStakholders = new PdfPTable(5);
                    tblStakholders.TotalWidth = 500.433070866f;
                    tblStakholders.LockedWidth = true;
                    tblStakholders.AddCell(new PdfPCell(new Phrase(leading, "S No.", fnttext)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblStakholders.AddCell(new PdfPCell(new Phrase(leading, "Stakeholder", fnttext)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblStakholders.AddCell(new PdfPCell(new Phrase(leading, "HS Code", fnttext)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblStakholders.AddCell(new PdfPCell(new Phrase(leading, "Stakeholder’s Mail Count", fnttext)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblStakholders.AddCell(new PdfPCell(new Phrase(leading, "Response count", fnttext)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    int i = 1;
                    foreach (RelatedStakeHolders rs in obj.Stakholders)
                    {
                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(i)), fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholders.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(rs.FullName), fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholders.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(rs.HSCodes), fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholders.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(rs.MailCount)), fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholders.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(rs.ResponseCount)), fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholders.AddCell(cell);
                    }
                    tblStakholders.SpacingAfter = 20f;
                    document.Add(tblStakholders);
                }
                #endregion

                #region "Notification related Mails"
                if (obj.StackholderMails != null)
                {
                    POTable = new PdfPTable(1);
                    POTable.TotalWidth = 515.433070866f;
                    POTable.LockedWidth = true;
                    POTable.SpacingAfter = 10.185f;
                    POTable.DefaultCell.Border = 0;
                    POTable.AddCell(new PdfPCell(new Phrase(leading, "Mails Sents", fntLabel)) { BackgroundColor = BaseColor.BLUE });
                    document.Add(POTable);

                    PdfPTable tblStakholderMails = new PdfPTable(4);
                    tblStakholderMails.SpacingAfter = 20f;
                    tblStakholderMails.TotalWidth = 500.433070866f;
                    tblStakholderMails.LockedWidth = true;
                    tblStakholderMails.AddCell(new PdfPCell(new Phrase(leading, "S No.", fnttext)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblStakholderMails.AddCell(new PdfPCell(new Phrase(leading, "Date", fnttext)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblStakholderMails.AddCell(new PdfPCell(new Phrase(leading, "Subject", fnttext)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblStakholderMails.AddCell(new PdfPCell(new Phrase(leading, "Sent to", fnttext)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    int i = 1;
                    foreach (StackholderMail smd in obj.StackholderMails)
                    {
                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(i)), fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholderMails.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(smd.MailSentDate), fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholderMails.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(smd.Subject), fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholderMails.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(smd.StakeholderCount)), fnttext);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholderMails.AddCell(cell);
                    }
                    tblStakholderMails.SpacingAfter = 20f;
                    document.Add(tblStakholderMails);
                }
                #endregion
                #endregion

                #region "Footer section"
                int[] w = { 100, 150 };
                POTable = new PdfPTable(2);

                POTable.HorizontalAlignment = Element.ALIGN_CENTER;
                POTable.DefaultCell.BackgroundColor = new BaseColor(255, 253, 230);
                POTable.SpacingBefore = 0;
                POTable.TotalWidth = 450;
                POTable.SetWidths(w);
                POTable.LockedWidth = true;
                Chunk chk = new Chunk("");
                Font f = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL, BaseColor.BLACK);
                chk.Font = f;

                cell = new PdfPCell();
                cell.AddElement(chk);
                cell.Border = 0;
                cell.PaddingLeft = 0;
                POTable.AddCell(cell);

                document.Add(POTable);
                para = new Paragraph();
                chk = new Chunk("Department of Commerce, Ministry of Commerce and Industry \n", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                para.Add(chk);
                chk = new Chunk("Goverment of India", new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK));
                para.Add(chk);
                para.Alignment = Element.ALIGN_CENTER;
                para.SpacingBefore = 5;
                document.Add(para);
                document.Close();
                #endregion
            }
            return FileName;
        }

        public void GenerateNotificationZip(long Id, out MemoryStream mStream)
        {
            NotificationDetails_Pdf obj = GetNotificationDetails_forPDF(Id);
            mStream = new MemoryStream();

            if (obj != null)
            {
                string PdfFileName = GeneratePdf(obj);

                using (ZipFile zip = new ZipFile())
                {
                    if (PdfFileName != "" && File.Exists(HttpContext.Current.Server.MapPath("/Attachments/Temp/" + PdfFileName)))
                        zip.AddItem(HttpContext.Current.Server.MapPath("/Attachments/Temp/" + PdfFileName), "");

                    foreach (EditAttachment ea in obj.Documents)
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath(ea.Path)))
                        {
                            FileInfo fi = new FileInfo(HttpContext.Current.Server.MapPath(ea.Path));
                            DirectoryInfo di = fi.Directory;
                            zip.AddItem(HttpContext.Current.Server.MapPath(ea.Path), di.Name);
                        }
                    }

                    zip.Save(mStream);

                    if (File.Exists(HttpContext.Current.Server.MapPath("/Attachments/Temp/" + PdfFileName)))
                        File.Delete(HttpContext.Current.Server.MapPath("/Attachments/Temp/" + PdfFileName));
                }
            }
        }

        public StakeHolderConversationPopUp GetConversation(Int64 NotificationId, Int64 StakeholderId)
        {
            StakeHolderConversationPopUp objR = new StakeHolderConversationPopUp();
            NotificationDataManager objDM = new NotificationDataManager();
            DataTable dt = objDM.StakeHolderConversation(NotificationId, StakeholderId);
            if (dt != null && dt.Rows.Count > 0)
            {
                StakeHolderConversation objMail;
                List<StakeHolderConversation> objConversation = new List<StakeHolderConversation>();
                foreach (DataRow dr in dt.Rows)
                {
                    objMail = new StakeHolderConversation();
                    objMail.MailDate = Convert.ToString(dr["MailDate"]);
                    objMail.MailTime = Convert.ToString(dr["MailTime"]);
                    objMail.MailSubject = Convert.ToString(dr["Subject"]);
                    objMail.MailMessage = Convert.ToString(dr["Message"]);
                    objMail.MailType = Convert.ToBoolean(dr["MailType"]);
                    objMail.Attachments = Convert.ToString(dr["Attachments"]);
                    objConversation.Add(objMail);
                }
                objR.Conversation = objConversation;
            }
            return objR;
        }

        public NotificationActions GetNotificationActions(Int64 Id)
        {
            NotificationActions objNA = new NotificationActions();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.NotificationActions(Id);
            if (ds != null)
            {
                int tblIndex = -1;

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        objNA.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                        objNA.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                        objNA.NotificationTitle = Convert.ToString(dr["Title"]);
                        objNA.MeetingDate = Convert.ToString(dr["MeetingDate"]);
                    }
                }

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<NotificationAction> NotificationActionList = new List<NotificationAction>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationAction objNotificationAction = new NotificationAction();
                        objNotificationAction.NotificationActionId = Convert.ToInt64(dr["NotificationActionId"]);
                        objNotificationAction.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objNotificationAction.Action = Convert.ToString(dr["Action"]);
                        objNotificationAction.RequiredOn = Convert.ToString(dr["RequiredOn"]);
                        objNotificationAction.EnteredOn = Convert.ToString(dr["EnteredOn"]);

                        EditAttachment objF = new EditAttachment();
                        if (Convert.ToString(dr["Attachment"]) != "")
                        {
                            objF = new EditAttachment();
                            objF.FileName = Convert.ToString(dr["AttachmentName"]);
                            objF.Path = "/Attachments/ActionAttachment/" + Convert.ToString(dr["Attachment"]);
                            objNotificationAction.Attachment = objF;
                        }
                        objNotificationAction.Remarks = Convert.ToString(dr["Remarks"]);
                        objNotificationAction.CompletedOn = Convert.ToString(dr["CompletedOn"]);
                        objNotificationAction.UpdatedOn = Convert.ToString(dr["UpdatedOn"]);
                        NotificationActionList.Add(objNotificationAction);
                    }
                    objNA.Actions = NotificationActionList;
                }
            }

            return objNA;
        }

        public AddNotificationAction_Output InsertUpdateActions(Int64 UserId, AddNotificationAction obj)
        {
            AddNotificationAction_Output objO = new AddNotificationAction_Output();
            NotificationDataManager objDM = new NotificationDataManager();
            DataTable dt = objDM.InsertUpdateActions(UserId, obj);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    objO.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                    objO.ActionId = Convert.ToInt32(dr["ActionId"]);
                }
            }
            return objO;
        }

        public Notification_Template GetNotificationSMSMailTemplate(long Id, Notification_Template_Search obj)
        {
            Notification_Template objT = new Notification_Template();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.MailSMSTemplate(Id, obj);
            if (ds != null)
            {
                int tblIndex = -1;

                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        objT.Subject = Convert.ToString(dr["Subject"]);
                        objT.Message = Convert.ToString(dr["Message"]);
                    }
                }

                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        #region "Mail Subject"
                        objT.Subject = objT.Subject.Replace("#NotificationType#", Convert.ToString(dr["NotificationType"]));
                        objT.Subject = objT.Subject.Replace("#NotificationStatus#", Convert.ToString(dr["NotificationStatus"]));
                        objT.Subject = objT.Subject.Replace("#NotificationNumber#", Convert.ToString(dr["NotificationNumber"]));
                        objT.Subject = objT.Subject.Replace("#DateOfNotification#", Convert.ToString(dr["DateOfNotification"]));
                        objT.Subject = objT.Subject.Replace("#FinalDateOfComment#", Convert.ToString(dr["FinalDateOfComment"]));
                        objT.Subject = objT.Subject.Replace("#SendResponseBy#", Convert.ToString(dr["SendResponseBy"]));
                        objT.Subject = objT.Subject.Replace("#Country#", Convert.ToString(dr["Country"]));
                        objT.Subject = objT.Subject.Replace("#Title#", Convert.ToString(dr["Title"]));
                        objT.Subject = objT.Subject.Replace("#AgencyResponsible#", Convert.ToString(dr["AgencyResponsible"]));
                        objT.Subject = objT.Subject.Replace("#UnderArticle#", Convert.ToString(dr["UnderArticle"]));
                        objT.Subject = objT.Subject.Replace("#ProductCovered#", Convert.ToString(dr["ProductCovered"]));
                        objT.Subject = objT.Subject.Replace("#Description#", Convert.ToString(dr["Description"]));
                        objT.Subject = objT.Subject.Replace("#StakeholderResponseDueBy#", Convert.ToString(dr["StakeholderResponseDueBy"]));
                        objT.Subject = objT.Subject.Replace("#EnquiryEmail#", Convert.ToString(dr["EnquiryEmailId"]));
                        objT.Subject = objT.Subject.Replace("#CreatedBy#", Convert.ToString(dr["CreatedBy"]));
                        objT.Subject = objT.Subject.Replace("#CreatedOn#", Convert.ToString(dr["CreatedOn"]));
                        objT.Subject = objT.Subject.Replace("#ObtainDocumentBy#", Convert.ToString(dr["ObtainDocumentBy"]));
                        objT.Subject = objT.Subject.Replace("#Language#", Convert.ToString(dr["Language"]));
                        objT.Subject = objT.Subject.Replace("#Translator#", Convert.ToString(dr["Translator"]));
                        objT.Subject = objT.Subject.Replace("#SendToTranslaterOn#", Convert.ToString(dr["SendToTranslaterOn"]));
                        objT.Subject = objT.Subject.Replace("#TranslationDueBy#", Convert.ToString(dr["TranslationDueBy"]));
                        objT.Subject = objT.Subject.Replace("#TranslationReminder#", Convert.ToString(dr["TranslationReminder"]));
                        #endregion

                        #region "Mail template Body"
                        objT.Message = objT.Message.Replace("#NotificationType#", Convert.ToString(dr["NotificationType"]));
                        objT.Message = objT.Message.Replace("#NotificationStatus#", Convert.ToString(dr["NotificationStatus"]));
                        objT.Message = objT.Message.Replace("#NotificationNumber#", Convert.ToString(dr["NotificationNumber"]));
                        objT.Message = objT.Message.Replace("#DateOfNotification#", Convert.ToString(dr["DateOfNotification"]));
                        objT.Message = objT.Message.Replace("#FinalDateOfComment#", Convert.ToString(dr["FinalDateOfComment"]));
                        objT.Message = objT.Message.Replace("#SendResponseBy#", Convert.ToString(dr["SendResponseBy"]));
                        objT.Message = objT.Message.Replace("#Country#", Convert.ToString(dr["Country"]));
                        objT.Message = objT.Message.Replace("#Title#", Convert.ToString(dr["Title"]));
                        objT.Message = objT.Message.Replace("#AgencyResponsible#", Convert.ToString(dr["AgencyResponsible"]));
                        objT.Message = objT.Message.Replace("#UnderArticle#", Convert.ToString(dr["UnderArticle"]));
                        objT.Message = objT.Message.Replace("#ProductCovered#", Convert.ToString(dr["ProductCovered"]));
                        objT.Message = objT.Message.Replace("#Description#", Convert.ToString(dr["Description"]));
                        objT.Message = objT.Message.Replace("#StakeholderResponseDueBy#", Convert.ToString(dr["StakeholderResponseDueBy"]));
                        objT.Message = objT.Message.Replace("#EnquiryEmail#", Convert.ToString(dr["EnquiryEmailId"]));
                        objT.Message = objT.Message.Replace("#CreatedBy#", Convert.ToString(dr["CreatedBy"]));
                        objT.Message = objT.Message.Replace("#CreatedOn#", Convert.ToString(dr["CreatedOn"]));
                        objT.Message = objT.Message.Replace("#ObtainDocumentBy#", Convert.ToString(dr["ObtainDocumentBy"]));
                        objT.Message = objT.Message.Replace("#Language#", Convert.ToString(dr["Language"]));
                        objT.Message = objT.Message.Replace("#Translator#", Convert.ToString(dr["Translator"]));
                        objT.Message = objT.Message.Replace("#SendToTranslaterOn#", Convert.ToString(dr["SendToTranslaterOn"]));
                        objT.Message = objT.Message.Replace("#TranslationDueBy#", Convert.ToString(dr["TranslationDueBy"]));
                        objT.Message = objT.Message.Replace("#TranslationReminder#", Convert.ToString(dr["TranslationReminder"]));
                        #endregion
                    }
                }
            }
            return objT;
        }

        public List<StackholderMail> GetNotificationMails(long Id)
        {
            List<StackholderMail> MailList = new List<StackholderMail>();
            NotificationDataManager objDM = new NotificationDataManager();
            DataTable dt = objDM.NotificationMails(Id);
            #region "Stakeholder Mails"
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    StackholderMail objStackholderMail = new StackholderMail();
                    objStackholderMail.MailId = Convert.ToInt32(dr["MailId"]);
                    objStackholderMail.Subject = Convert.ToString(dr["Subject"]);
                    objStackholderMail.Message = Convert.ToString(dr["Message"]);
                    objStackholderMail.StakeholderCount = Convert.ToInt32(dr["StakeholderCount"]);
                    objStackholderMail.MailSentDate = Convert.ToString(dr["MailSentOn"]);
                    MailList.Add(objStackholderMail);
                }
            }
            #endregion
            return MailList;
        }
    }
}
