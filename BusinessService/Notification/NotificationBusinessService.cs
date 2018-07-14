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
using iTextSharp.text.pdf;
using Ionic.Zip;
using iTextSharp.text.html.simpleparser;

namespace BusinessService.Notification
{
    public class NotificationBusinessService
    {
        #region "Add/Edit Notification"
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
                    List<EditAttachment> AttachList = new List<EditAttachment>();
                    objR.NotificationId = Convert.ToInt64(ds.Tables[0].Rows[0]["NotificationId"]);
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        EditAttachment objE = new EditAttachment();
                        objE.FileName = Convert.ToString(dr["Document"]);
                        objE.Path = Convert.ToString(dr["Attachment"]);
                        AttachList.Add(objE);
                    }
                    objR.Attachments = AttachList;
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
                #region "DocumentType Details"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<DocumentType> list = new List<DocumentType>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        DocumentType obj = new DocumentType();
                        obj.DocumentTypeId = Convert.ToInt32(dr["DocumentTypeId"]);
                        obj.Type = Convert.ToString(dr["Type"]);
                        list.Add(obj);
                    }
                    objR.DocumentTypeList = list;
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
                        objR.Country = Convert.ToString(dr["Country"]);
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

                        objR.ObtainDocBy = Convert.ToString(dr["ObtainDocumentBy"]);
                        objR.RemainderToTranslaterOn = Convert.ToString(dr["TranslationReminder"]);
                        objR.TranslationDueOn = Convert.ToString(dr["TranslationDueBy"]);

                        objR.SkippedToDiscussion = dr["SkippedToDiscussion"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(dr["SkippedToDiscussion"]);
                        objR.Stakeholders = Convert.ToString(dr["SelectedStakeholders"]);
                        objR.TotalResponses = Convert.ToString(dr["ResponseCount"]);
                        objR.StakeholderResponseDueBy = Convert.ToString(dr["StakeholderResponseDueBy"]);
                        objR.TotalMails = Convert.ToString(dr["MailCount"]);
                        objR.RegulationFlag = Convert.ToInt32(dr["RegulationFlag"]);
                        objR.TranslationFlag = Convert.ToInt32(dr["TranslationFlag"]);
                        objR.StakholderMailFlag = Convert.ToInt32(dr["StakholderMailFlag"]);
                        objR.MeetingFlag = Convert.ToInt32(dr["MeetingFlag"]);
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

                #region "Regulations"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<Regulations> RegulationsList = new List<Regulations>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        Regulations objRegulations = new Regulations();
                        objRegulations.DocumentId = Convert.ToInt64(dr["DocumentId"]);

                        EditAttachment objE = new EditAttachment();
                        if (Convert.ToString(dr["Document"]) != "")
                        {
                            objE.FileName = Convert.ToString(dr["Document"]);
                            objE.Path = "/Attachments/NotificationDocument/" + Convert.ToInt64(dr["DocumentId"]) + "_" + Convert.ToString(dr["Document"]);
                            objRegulations.Document = objE;
                        }

                        objRegulations.DocumentName = Convert.ToString(dr["DocumentName"]);

                        if (Convert.ToString(dr["TranslatedDocument"]) != "")
                        {
                            objE = new EditAttachment();
                            objE.FileName = Convert.ToString(dr["TranslatedDocument"]);
                            objE.Path = "/Attachments/NotificationDocument_Translated/" + Convert.ToInt64(dr["DocumentId"]) + "_" + Convert.ToString(dr["TranslatedDocument"]);
                            objRegulations.TranslatedDoc = objE;
                        }

                        objRegulations.TranslatedDocName = Convert.ToString(dr["TranslatedDocumentName"]);
                        objRegulations.SentForTranslation = Convert.ToString(dr["SendToTranslaterOn"]);
                        objRegulations.LanguageId = Convert.ToInt64(dr["LanguageId"]);
                        objRegulations.Language = Convert.ToString(dr["Language"]);
                        objRegulations.TranslatorId = Convert.ToInt64(dr["TranslatorId"]);
                        objRegulations.Translator = Convert.ToString(dr["Translator"]);
                        RegulationsList.Add(objRegulations);
                    }
                    objR.Regulations = RegulationsList;
                }
                #endregion
            }

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
            return objR;
        }

        public ValidateNotification_Output ValidateNotification(CheckNotification obj)
        {
            ValidateNotification_Output objV = new ValidateNotification_Output();
            string EnquiryEmailId = String.Empty;
            NotificationDataManager objDM = new NotificationDataManager();
            DataTable dt = objDM.ValidateNotification(obj);
            if (dt != null && dt.Rows.Count > 0)
            {
                objV.IsExists = Convert.ToBoolean(dt.Rows[0]["IsExists"]);
                objV.CountryId = Convert.ToInt32(dt.Rows[0]["CountryId"]);
                objV.Country = Convert.ToString(dt.Rows[0]["Country"]);
                objV.CountryCode = Convert.ToString(dt.Rows[0]["CountryCode"]);
                objV.EnquiryDeskEmail = Convert.ToString(dt.Rows[0]["EnquiryDeskEmail"]);

                objV.SendResponseBy = Convert.ToString(dt.Rows[0]["SendResponseBy"]);
                objV.FinalDateofComment = Convert.ToString(dt.Rows[0]["FinalDateofComment"]);
                objV.StakeholderResponseBy = Convert.ToString(dt.Rows[0]["StakeholderResponsedueBy"]);
            }

            return objV;
        }

        public CalculatedDate getCalculatedDate(Int64 Id, string DateOfNotification, string FinalDateOfComments)
        {
            CalculatedDate objE = new CalculatedDate();
            NotificationDataManager objDM = new NotificationDataManager();
            DataTable dt = objDM.getCalculatedDate(Id, DateOfNotification, FinalDateOfComments);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    objE.FinalDateOfComment = Convert.ToString(dr["FinalDateOfComment"]);
                    objE.ObtainDocumentBy = Convert.ToString(dr["ObtainDocumentBy"]);
                    objE.TranslationDueBy = Convert.ToString(dr["TranslationDueBy"]);
                    objE.SendResponseBy = Convert.ToString(dr["SendResponseBy"]);
                    objE.StakeholderResponsedueBy = Convert.ToString(dr["StakeholderResponsedueBy"]);
                    objE.StakeholderReminder = Convert.ToString(dr["StakeholderReminderDate"]);
                    objE.TranslationReminder = Convert.ToString(dr["TranslationReminderDate"]);
                }
            }
            return objE;
        }
        public SendMailDetailStakeholders_Output GetMailDetailsSendtoStakeholder(string MailId, string Callfor)
        {
            SendMailDetailStakeholders_Output obj = new SendMailDetailStakeholders_Output();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.GetMailDetailsSendtoStakeholder(MailId, Callfor);
            if (ds != null && ds.Tables.Count > 0)
            {
                using (DataTable dtMailDetails = ds.Tables[0])
                {
                    StackholderMail objSHM = new StackholderMail();
                    if (dtMailDetails.Rows.Count > 0)
                    {
                        objSHM.MailId = Convert.ToInt64(dtMailDetails.Rows[0]["MailId"]);
                        objSHM.Subject = Convert.ToString(dtMailDetails.Rows[0]["Subject"]);
                        objSHM.Message = Convert.ToString(dtMailDetails.Rows[0]["Message"]);
                        objSHM.StakeholderCount = Convert.ToInt64(dtMailDetails.Rows[0]["StakeholderCount"]);
                    }
                    obj.MailDetails = objSHM;
                }
                using (DataTable dtStakeHolder = ds.Tables[1])
                {
                    List<RelatedStakeHolders> objStakeHolderList = new List<RelatedStakeHolders>();
                    if (dtStakeHolder.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtStakeHolder.Rows)
                        {
                            RelatedStakeHolders objSH = new RelatedStakeHolders();
                            objSH.FullName = Convert.ToString(dr["StakeholderName"]);
                            objSH.OrgName = Convert.ToString(dr["OrgName"]);
                            objStakeHolderList.Add(objSH);
                        }
                    }
                    obj.StakeHolders = objStakeHolderList;
                }
                using (DataTable dtMailAttachment = ds.Tables[2])
                {
                    List<EditAttachment> objMailAttachmentList = new List<EditAttachment>();
                    if (dtMailAttachment.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtMailAttachment.Rows)
                        {
                            EditAttachment objMailAttachment = new EditAttachment();
                            objMailAttachment.FileName = Convert.ToString(dr["FileName"]);
                            objMailAttachment.Path = Convert.ToString(dr["Path"]);
                            objMailAttachmentList.Add(objMailAttachment);
                        }
                    }
                    obj.MailAttachmentDetails = objMailAttachmentList;

                }
            }
            return obj;
        }
        #endregion

        #region "View notification"
        public ViewNotification GetViewNotificationDetails(Int64 Id, Nullable<Int64> MomId, string CallFor, Nullable<Int64> RowNum, string DataFor, Nullable<Int64> TotalRow)
        {
            ViewNotification objViewDetails = new ViewNotification();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.ViewNotifications(Id);
            if (ds != null && ds.Tables.Count > 0)
            {
                Int16 tblIndx = -1;

                #region "Notification basic details"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    objViewDetails.NotificationId = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["NotificationId"]);
                    objViewDetails.NotificationStage = Convert.ToString(ds.Tables[tblIndx].Rows[0]["NotificationStage"]);
                    objViewDetails.NotificationType = Convert.ToString(ds.Tables[tblIndx].Rows[0]["NotificationType"]);
                    objViewDetails.NotificationNumber = Convert.ToString(ds.Tables[tblIndx].Rows[0]["NotificationNumber"]);
                    objViewDetails.Notifica_Document = Convert.ToString(ds.Tables[tblIndx].Rows[0]["NotificationAttachment"]);
                    objViewDetails.NotificationStatus = Convert.ToString(ds.Tables[tblIndx].Rows[0]["NotificationStatus"]);
                    objViewDetails.DateofNotification = Convert.ToString(ds.Tables[tblIndx].Rows[0]["DateOfNotification"]);
                    objViewDetails.FinalDateOfComments = Convert.ToString(ds.Tables[tblIndx].Rows[0]["FinalDateOfComment"]);
                    objViewDetails.SendResponseBy = Convert.ToString(ds.Tables[tblIndx].Rows[0]["SendResponseBy"]);
                    objViewDetails.Country = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Country"]);
                    objViewDetails.EnquiryPoint = Convert.ToString(ds.Tables[tblIndx].Rows[0]["EnquiryEmailId"]);
                    objViewDetails.Title = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Title"]);
                    objViewDetails.ResponsibleAgency = Convert.ToString(ds.Tables[tblIndx].Rows[0]["AgencyResponsible"]);
                    objViewDetails.UnderArticle = Convert.ToString(ds.Tables[tblIndx].Rows[0]["UnderArticle"]);
                    objViewDetails.ProductsCovered = Convert.ToString(ds.Tables[tblIndx].Rows[0]["ProductCovered"]);
                    objViewDetails.Description = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Description"]);
                    objViewDetails.MailSentToEnquiryDeskOn = Convert.ToString(ds.Tables[tblIndx].Rows[0]["MailSentToEnquiryDeskOn"]);
                    objViewDetails.RemainderToTranslaterOn = Convert.ToString(ds.Tables[tblIndx].Rows[0]["TranslationReminder"]);
                    objViewDetails.TranslationDueOn = Convert.ToString(ds.Tables[tblIndx].Rows[0]["TranslationDueBy"]);
                    objViewDetails.StakeholderResponseDueBy = Convert.ToString(ds.Tables[tblIndx].Rows[0]["StakeholderResponseDueBy"]);
                    objViewDetails.MeetingNotes = Convert.ToString(ds.Tables[tblIndx].Rows[0]["MeetingNote"]);
                    if (MomId != null && MomId > 0)
                        objViewDetails.MomId = MomId;

                    if (RowNum != null && RowNum > 0)
                        objViewDetails.RowNum = RowNum;

                    if (DataFor != "")
                        objViewDetails.CallFor = CallFor;

                    if (TotalRow != null)
                        objViewDetails.TotalRow = TotalRow;

                    objViewDetails.RegulationFlag = Convert.ToInt32(ds.Tables[tblIndx].Rows[0]["RegulationFlag"]);
                    objViewDetails.TranslationFlag = Convert.ToInt32(ds.Tables[tblIndx].Rows[0]["TranslationFlag"]);
                    objViewDetails.StakholderMailFlag = Convert.ToInt32(ds.Tables[tblIndx].Rows[0]["StakholderMailFlag"]);
                    objViewDetails.MeetingFlag = Convert.ToInt32(ds.Tables[tblIndx].Rows[0]["MeetingFlag"]);
                }
                #endregion

                #region "Notification HS Codes"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<HSCodes> objSH = new List<HSCodes>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        HSCodes objCodes = new HSCodes();
                        objCodes.HSCode = Convert.ToString(dr["HSCode"]);
                        objCodes.Text = Convert.ToString(dr["Description"]);
                        objSH.Add(objCodes);
                    }
                    objViewDetails.HSCodes = objSH;
                }
                #endregion

                #region "Notification full text"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<Regulations> objReg = new List<Regulations>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        Regulations objR = new Regulations();
                        objR.DocumentId = Convert.ToInt64(dr["NotificationDocumentId"]);
                        objR.DocumentName = Convert.ToString(dr["Document"]);

                        EditAttachment objE = new EditAttachment();
                        if (dr["Document"] != null && Convert.ToString(dr["Document"]) != "")
                        {
                            objE.FileName = Convert.ToString(dr["FullTextOfRegulation"]);
                            objE.Path = Convert.ToString(dr["NotificationDocumentId"]) + "_" + Convert.ToString(dr["FullTextOfRegulation"]);
                            objR.Document = objE;
                        }

                        objR.LanguageId = Convert.ToInt64(dr["LanguageId"]);
                        objR.Language = Convert.ToString(dr["Language"]);
                        objR.TranslatorId = Convert.ToInt64(dr["TranslatorId"]);
                        objR.Translator = Convert.ToString(dr["Translator"]);
                        objR.SentForTranslation = Convert.ToString(dr["SendToTranslaterOn"]);
                        objR.TranslatedDocName = Convert.ToString(dr["TranslatedDocumentName"]);

                        objE = new EditAttachment();
                        if (dr["Document"] != null && Convert.ToString(dr["Document"]) != "")
                        {
                            objE.FileName = Convert.ToString(dr["TranslatedDocument"]);
                            objE.Path = Convert.ToString(dr["NotificationDocumentId"]) + "_" + Convert.ToString(dr["TranslatedDocument"]);
                            objR.TranslatedDoc = objE;
                        }
                        objR.ReceivedOn = Convert.ToString(dr["ReceivedOn"]);
                        objReg.Add(objR);
                    }
                    objViewDetails.Regulations = objReg;
                }
                #endregion

                #region "Notification related stakeholder"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<Stakeholder> SHList = new List<Stakeholder>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        Stakeholder objStakeHolder = new Stakeholder();
                        objStakeHolder.Name = Convert.ToString(dr["StakeHolderName"]);
                        objStakeHolder.HSCodes = Convert.ToString(dr["HSCodes"]);
                        objStakeHolder.MailsCount = Convert.ToInt32(dr["MailCount"]);
                        objStakeHolder.ResponseCount = Convert.ToInt32(dr["ResponseCount"]);
                        objStakeHolder.OrgName = Convert.ToString(dr["OrgName"]);
                        SHList.Add(objStakeHolder);
                    }
                    objViewDetails.StakeHolders = SHList;
                }
                #endregion

                #region "Notification stakeholder mail"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<StakeholderMail> objSHMail = new List<StakeholderMail>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        StakeholderMail objSHMList = new StakeholderMail();
                        objSHMList.SentOn = Convert.ToString(dr["MailSentOn"]);
                        objSHMList.Subject = Convert.ToString(dr["Message"]);
                        objSHMList.StakholdersCount = Convert.ToInt16(dr["StakeholderCount"]);
                        objSHMList.MailId = Convert.ToInt32(dr["MailId"]);
                        objSHMail.Add(objSHMList);
                    }
                    objViewDetails.MailSent = objSHMail;
                }
                #endregion

                #region "Notification Stakeholder response"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<StakeholderMail> objSHMail = new List<StakeholderMail>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        StakeholderMail objSHMList = new StakeholderMail();
                        objSHMList.SentOn = Convert.ToString(dr["ReceivedOn"]);
                        objSHMList.Subject = Convert.ToString(dr["Message"]);
                        objSHMList.StakholdersCount = Convert.ToInt16(dr["Responses"]);
                        objSHMList.StakeholderResponseId = Convert.ToInt32(dr["StakeholderResponseId"]);
                        objSHMList.OrgName = Convert.ToString(dr["OrgName"]);
                        objSHMList.Name = Convert.ToString(dr["StakeholderName"]);
                        objSHMail.Add(objSHMList);
                    }
                    objViewDetails.MailReceived = objSHMail;
                }
                #endregion

                #region "Notification Actions"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<Notifi_Actions> objNotifiActions = new List<Notifi_Actions>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        Notifi_Actions objActionDetails = new Notifi_Actions();
                        objActionDetails.ActionId = Convert.ToInt64(dr["ActionId"]);
                        objActionDetails.Action = Convert.ToString(dr["Action"]);
                        objActionDetails.RequiredBy = Convert.ToString(dr["RequiredBy"]);
                        objActionDetails.TakenOn = Convert.ToString(dr["TakenOn"]);
                        objActionDetails.MailId = Convert.ToInt32(dr["MailId"]);
                        objActionDetails.Subject = Convert.ToString(dr["Subject"]);
                        objActionDetails.Message = Convert.ToString(dr["Message"]);
                        objActionDetails.Attachments = Convert.ToString(dr["Attachments"]);
                        objActionDetails.ColorCode = Convert.ToString(dr["ColorCode"]);
                        objActionDetails.TooltipText = Convert.ToString(dr["TooltipText"]);
                        objNotifiActions.Add(objActionDetails);
                    }
                    objViewDetails.Actions = objNotifiActions;
                }
                #endregion

                #region "Notification mails attachments"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<MailSentAttachment> objMailSentAttachment = new List<MailSentAttachment>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        MailSentAttachment objAttachment = new MailSentAttachment();
                        objAttachment.MailId = Convert.ToInt32(dr["MailId"]);
                        objAttachment.FileName = Convert.ToString(dr["FileName"]);
                        objAttachment.Path = Convert.ToString(dr["Path"]);
                        objMailSentAttachment.Add(objAttachment);
                    }
                    objViewDetails.MailSentAttachment = objMailSentAttachment;
                }
                #endregion

                #region "Notification responses"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<MailReceivedAttachment> objReceivedAttachment = new List<MailReceivedAttachment>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        MailReceivedAttachment objRAttachment = new MailReceivedAttachment();
                        objRAttachment.StakeholderResponseId = Convert.ToInt32(dr["StakeholderResponseId"]);
                        objRAttachment.FileName = Convert.ToString(dr["FileName"]);
                        objRAttachment.Path = Convert.ToString(dr["Path"]);
                        objReceivedAttachment.Add(objRAttachment);
                    }
                    objViewDetails.MailReceivedAttachment = objReceivedAttachment;
                }
                #endregion

                #region "Notification action attachments"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<ActionAttachment> objActionAttachmentList = new List<ActionAttachment>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        ActionAttachment objActionAttachment = new ActionAttachment();
                        objActionAttachment.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objActionAttachment.FileName = Convert.ToString(dr["FileName"]);
                        objActionAttachment.Path = Convert.ToString(dr["Path"]);
                        objActionAttachmentList.Add(objActionAttachment);
                    }
                    objViewDetails.ActionAttachment = objActionAttachmentList;
                }
                #endregion
            }
            return objViewDetails;
        }
        public ViewNotification NotificationSummary(Nullable<Int64> MomId, string CallFor, Nullable<Int64> RowNum, string DataFor)
        {
            ViewNotification objViewDetails = new ViewNotification();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.NotificationSummary(MomId, CallFor, RowNum, DataFor);
            if (ds != null && ds.Tables.Count > 0)
            {
                Int16 tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    objViewDetails.NotificationId = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["NotificationId"]);
                    objViewDetails.RowNum = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["RowNo"]);
                    objViewDetails.MomId = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["MoMId"]);
                }
            }
            return objViewDetails;
        }
        private Paragraph CreateSimpleHtmlParagraph(String text1, Font FntParaNormalBlack)
        {
            Paragraph p = new Paragraph();
            p.Font = FntParaNormalBlack;


            using (StringReader sr = new StringReader(text1))
            {
                StyleSheet styles = new StyleSheet();
                System.Collections.Generic.List<IElement> elements = iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(sr, styles);
                foreach (IElement e in elements)
                {
                    e.Chunks[0].Font = FntParaNormalBlack;
                    p.Add(e);
                }
            }

            return p;
        }
        #endregion

        #region "Notification Stakeholders"
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
                    objStakeHolder.FullName = Convert.ToString(dr["StakeHolderName"]);
                    objStakeHolder.HSCodes = Convert.ToString(dr["HSCodes"]);
                    objStakeHolder.MailCount = Convert.ToInt32(dr["MailCount"]);
                    objStakeHolder.ResponseCount = Convert.ToInt32(dr["ResponseCount"]);
                    objStakeHolder.OrgName = Convert.ToString(dr["OrgName"]);
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
                            List<BusinessObjects.ManageAccess.StakeHolder> StakeholderList = new List<BusinessObjects.ManageAccess.StakeHolder>();
                            foreach (DataRow dr in dt.Rows)
                            {
                                BusinessObjects.ManageAccess.StakeHolder objS = new BusinessObjects.ManageAccess.StakeHolder();
                                objS.StakeHolderId = Convert.ToInt32(dr["StakeholderId"]);
                                objS.StakeHolderName = Convert.ToString(dr["StakeHolderName"]);
                                objS.Email = Convert.ToString(dr["EmailId"]);
                                StakeholderList.Add(objS);
                            }

                            objOutput.StakeHolders = StakeholderList;
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
                        List<EditAttachment> AttList = new List<EditAttachment>();
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                EditAttachment objAtt = new EditAttachment();
                                objAtt.Path = Convert.ToString(dr["Attachment"]);
                                objAtt.FileName = Convert.ToString(dr["DocumentName"]);
                                AttList.Add(objAtt);
                            }
                            objOutput.Attachments = AttList;
                        }
                    }
                }
            }
            return objOutput;
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
                    objMail.RowNumber = Convert.ToInt32(dr["RowNumber"]);
                    objMail.MailId = Convert.ToInt64(dr["MailId"]);
                    objMail.MailDate = Convert.ToString(dr["MailDate"]);
                    objMail.MailTime = Convert.ToString(dr["MailTime"]);
                    objMail.MailSubject = Convert.ToString(dr["Subject"]);
                    objMail.MailMessage = Convert.ToString(dr["Message"]);
                    objMail.MailType = Convert.ToBoolean(dr["MailType"]);
                    objMail.Attachments = Convert.ToString(dr["Attachments"]);
                    objMail.FileName = Convert.ToString(dr["FileName"]);
                    objConversation.Add(objMail);
                }
                objR.Conversation = objConversation;
            }
            return objR;
        }

        public MailsSentResponses GetNotificationMails(long Id)
        {
            MailsSentResponses obj = new MailsSentResponses();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.NotificationMails(Id);
            if (ds != null && ds.Tables.Count > 0)
            {
                StackholderMail objStatistics = new StackholderMail();
                int tblIndx = -1;
                tblIndx++;
                #region "Mails sent"
                if (ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<StackholderMail> objRows = new List<StackholderMail>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        objStatistics = new StackholderMail();
                        objStatistics.Subject = Convert.ToString(dr["Subject"]);
                        objStatistics.StakeholderCount = Convert.ToInt32(dr["StakeholderCount"]);
                        objStatistics.Date = Convert.ToString(dr["MailSentOn"]);
                        objStatistics.MailId = Convert.ToInt64(dr["MailId"]);
                        objRows.Add(objStatistics);
                    }
                    obj.MailsSent = objRows;
                }
                #endregion

                tblIndx++;
                #region "Mails received"
                if (ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<StackholderMail> objRows = new List<StackholderMail>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        objStatistics = new StackholderMail();
                        objStatistics.Subject = Convert.ToString(dr["Subject"]);
                        objStatistics.StakeholderCount = Convert.ToInt32(dr["Responses"]);
                        objStatistics.Date = Convert.ToString(dr["ReceivedOn"]);
                        objStatistics.StakeholderMailId = Convert.ToInt64(dr["StakeholderResponseId"]);
                        objStatistics.StakeholderName = Convert.ToString(dr["StakeholderName"]);
                        objStatistics.OrgName = Convert.ToString(dr["OrgName"]);
                        objStatistics.Message = Convert.ToString(dr["Message"]);
                        objRows.Add(objStatistics);
                    }
                    obj.Responses = objRows;
                }
                #endregion
            }
            return obj;
        }

        public SendMailStakeholders_Output SaveStakeholderResponse(StakeholderResponse obj)
        {
            SendMailStakeholders_Output objOutput = new SendMailStakeholders_Output();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.SaveStakeholderResponse(obj);
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
                            StakeholderResponse objS = new StakeholderResponse();
                            objS.StakeholderResponseId = Convert.ToInt64(dt.Rows[0]["StakeholderResponseId"]);
                            objOutput.SRId = objS;
                        }
                    }
                }
            }
            return objOutput;
        }

        public StakeHoldersList GetStakeHoldersMaster(string SearchText)
        {
            StakeHoldersList objSHL = new StakeHoldersList();
            NotificationDataManager objDM = new NotificationDataManager();
            DataTable dt = objDM.GetStakeHoldersMaster(SearchText);
            if (dt != null && dt.Rows.Count > 0)
            {
                int i = 1;
                List<StakeHolderMaster> StakeHolderList = new List<StakeHolderMaster>();
                foreach (DataRow dr in dt.Rows)
                {
                    StakeHolderMaster objStakeHolder = new StakeHolderMaster();
                    objStakeHolder.ItemNumber = i;
                    objStakeHolder.StakeHolderId = Convert.ToInt64(dr["StakeholderId"]);
                    objStakeHolder.FullName = Convert.ToString(dr["StakeHolderName"]);
                    objStakeHolder.HSCodes = Convert.ToString(dr["HSCodes"]);
                    objStakeHolder.OrganizationName = Convert.ToString(dr["OrgName"]);
                    objStakeHolder.Designation = Convert.ToString(dr["Designation"]);
                    i++;
                    StakeHolderList.Add(objStakeHolder);
                }
                objSHL.StakeHolders = StakeHolderList;
            }
            return objSHL;
        }
        public ResponseMailDetailsStakeholders_Output ViewResponseFromStackHolder(long Id)
        {
            ResponseMailDetailsStakeholders_Output objSO = new ResponseMailDetailsStakeholders_Output();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.ViewResponseFromStackHolder(Id);
            if (ds != null)
            {
                int tblIndex = -1;

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {

                        StackholderMail objT = new StackholderMail();
                        objT.Date = Convert.ToString(dr["ReceivedOn"]);
                        objT.Subject = Convert.ToString(dr["Subject"]);
                        objT.Message = Convert.ToString(dr["Message"]);
                        objSO.objStackholderMail = objT;
                    }
                }

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<EditAttachment> AttachmentList = new List<EditAttachment>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        EditAttachment objA = new EditAttachment();
                        objA.FileName = Convert.ToString(dr["FileName"]);
                        objA.Path = Convert.ToString(dr["FilePath"]);
                        AttachmentList.Add(objA);
                    }
                    objSO.MailAttachmentDetails = AttachmentList;
                }
            }

            return objSO;
        }
        #endregion

        #region "Notification full text of documents"
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
        public SendToTranslater_Output SendDocumentToTranslater(SendToTranslater obj)
        {
            SendToTranslater_Output objOutput = new SendToTranslater_Output();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.SendDocumentToTranslater(obj);
            if (ds != null)
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
                    }
                }

                tblIndex++;
                if (ds.Tables.Count > tblIndex)
                {
                    using (DataTable dt = ds.Tables[tblIndex])
                    {
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            List<EditAttachment_Doc> AttachList = new List<EditAttachment_Doc>();
                            foreach (DataRow dr in dt.Rows)
                            {
                                EditAttachment_Doc objA = new EditAttachment_Doc();
                                objA.SentToTranslatorOn = Convert.ToString(dr["SendToTranslaterOn"]);
                                objA.DocumentId = Convert.ToInt64(dr["DocumentId"]);
                                objA.FileName = Convert.ToString(dr["FileName"]);
                                objA.Path = Convert.ToString(dr["Path"]);
                                AttachList.Add(objA);
                            }
                            objOutput.Attachments = AttachList;
                        }
                    }
                }
            }

            return objOutput;
        }

        #endregion

        #region "Notification Details"
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
                        obj.Notifica_Document = Convert.ToString(dr["NotificationAttachment"]);
                        obj.DateOfNotification = Convert.ToString(dr["DateOfNotification"]);
                        obj.FinalDateOfComment = Convert.ToString(dr["FinalDateOfComment"]);
                        obj.SendResponseBy = Convert.ToString(dr["SendResponseBy"]);
                        obj.Country = Convert.ToString(dr["Country"]);
                        obj.EnquiryPoint = Convert.ToString(dr["EnquiryEmailId"]);
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
                        obj.MeetingNotes = Convert.ToString(dr["MeetingNote"]);
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
                #region "Regulation"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex] != null && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<Regulations> objReg = new List<Regulations>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        Regulations objR = new Regulations();
                        objR.DocumentName = Convert.ToString(dr["FullTextOfRegulation"]);
                        objR.Language = Convert.ToString(dr["Language"]);
                        objR.Translator = Convert.ToString(dr["Translator"]);
                        objR.SentForTranslation = Convert.ToString(dr["SendToTranslaterOn"]);
                        objR.TranslatedDocName = Convert.ToString(dr["TranslatedDocumentName"]);
                        objR.ReceivedOn = Convert.ToString(dr["ReceivedOn"]);
                        objReg.Add(objR);
                    }
                    obj.Regulations = objReg;
                }

                #endregion "Regulation"
                #region "Notification Related Stackholders"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex] != null && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<RelatedStakeHolders> StakeHolderList = new List<RelatedStakeHolders>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        RelatedStakeHolders objStakeHolder = new RelatedStakeHolders();
                        //objStakeHolder.StakeHolderId = Convert.ToInt32(dr["StakeholderId"]);
                        objStakeHolder.FullName = Convert.ToString(dr["StakeHolderName"]);
                        //objStakeHolder.HSCodes = Convert.ToString(dr["HSCodes"]);
                        objStakeHolder.MailCount = Convert.ToInt32(dr["MailCount"]);
                        objStakeHolder.ResponseCount = Convert.ToInt32(dr["ResponseCount"]);
                        objStakeHolder.OrgName = Convert.ToString(dr["OrgName"]);
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
                        objM.Date = Convert.ToString(dr["MailSentOn"]);
                        objM.StakeholderCount = Convert.ToInt32(dr["StakeholderCount"]);
                        MailList.Add(objM);
                    }
                    obj.StackholderMails = MailList;
                }
                #endregion

                #region "Received On"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex] != null && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<StakeholderMail> objSHMail = new List<StakeholderMail>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        StakeholderMail objSHMList = new StakeholderMail();
                        objSHMList.SentOn = Convert.ToString(dr["ReceivedOn"]);
                        objSHMList.Subject = Convert.ToString(dr["Message"]);
                        objSHMList.StakholdersCount = Convert.ToInt16(dr["Responses"]);
                        objSHMList.Name = Convert.ToString(dr["StakeholderName"]);
                        objSHMList.OrgName = Convert.ToString(dr["OrgName"]);
                        objSHMail.Add(objSHMList);
                    }
                    obj.MailReceived = objSHMail;
                }
                #endregion "Received On"

                #region "Actions"
                tblIndex++;
                if (ds.Tables.Count > tblIndex && ds.Tables[tblIndex] != null && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<Notifi_Actions> objNotifiActions = new List<Notifi_Actions>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        Notifi_Actions objActionDetails = new Notifi_Actions();
                        objActionDetails.ActionId = Convert.ToInt64(dr["ActionId"]);
                        objActionDetails.Action = Convert.ToString(dr["Action"]);
                        objActionDetails.RequiredBy = Convert.ToString(dr["RequiredBy"]);
                        objActionDetails.TakenOn = Convert.ToString(dr["TakenOn"]);
                        objActionDetails.MailId = Convert.ToInt32(dr["MailId"]);
                        objActionDetails.Subject = Convert.ToString(dr["Subject"]);
                        objActionDetails.Message = Convert.ToString(dr["Message"]);
                        objActionDetails.Attachments = Convert.ToString(dr["Attachments"]);
                        objNotifiActions.Add(objActionDetails);
                    }
                    obj.Actions = objNotifiActions;
                }
                #endregion "Actions"

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
                        para = new Paragraph(HttpUtility.HtmlDecode(smd.Date), fnttext);
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
        public class RoundRectangle : IPdfPCellEvent
        {
            long Act;
            public RoundRectangle(long Action)
            {
                Act = Action;

            }
            public void CellLayout(
              PdfPCell cell, Rectangle rect, PdfContentByte[] canvas
            )
            {
                PdfContentByte cb = canvas[PdfPTable.LINECANVAS];
                cb.RoundRectangle(
                    rect.Left,
      rect.Bottom,
      rect.Width,
      rect.Height / 2,

                  4 // change to adjust how "round" corner is displayed
                );
                //BaseColor Action1 = new BaseColor(241, 92, 34);
                //BaseColor Action2 = new BaseColor(211, 1, 1);
                //BaseColor Action3 = new BaseColor(254, 192, 43);
                //BaseColor Action4 = new BaseColor(204, 204, 204);
                if (Act == 1)
                {
                    cb.SetRGBColorStroke(241, 92, 34);
                    cb.SetRGBColorFill(241, 92, 34);
                    cb.Fill();
                    cb.Stroke();
                }
                else if (Act == 2)
                {
                    cb.SetRGBColorStroke(211, 1, 1);
                    cb.SetRGBColorFill(211, 1, 1);
                    cb.Fill();
                    cb.Stroke();
                }
                else if (Act == 3)
                {
                    cb.SetRGBColorStroke(254, 192, 43);
                    cb.SetRGBColorFill(254, 192, 43);
                    cb.Fill();
                    cb.Stroke();
                }
                else if (Act == 4)
                {
                    cb.SetRGBColorStroke(204, 204, 204);
                    cb.SetRGBColorFill(204, 204, 204);
                    cb.Fill();
                    cb.Stroke();
                }
            }
        }
        private string GeneratePdf1(NotificationDetails_Pdf obj)
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
                Font fntheader = FontFactory.GetFont("Arial", 14, Font.BOLD, BaseColor.BLACK);
                Font fntparaNormal = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
                Font fntparaBOLD = FontFactory.GetFont("Arial", 10, Font.BOLD, BaseColor.BLACK);
                BaseColor Action1 = new BaseColor(241, 92, 34);
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

                #region "Header"
                POTable = new PdfPTable(1);
                POTable.TotalWidth = 515.433070866f;
                POTable.LockedWidth = true;
                POTable.SpacingAfter = 10.185f;
                POTable.AddCell(new PdfPCell(new Phrase(leading, "Notification", fntheader)) { BackgroundColor = BaseColor.WHITE, Border = Rectangle.NO_BORDER });
                document.Add(POTable);

                POTable = new PdfPTable(36);
                POTable.TotalWidth = 515.433070866f;
                POTable.LockedWidth = true;
                POTable.SpacingAfter = 10.185f;
                POTable.DefaultCell.Border = 0;

                #endregion "Header"
                #region "Row : Notification Type"
                cell = new PdfPCell();
                para = new Paragraph("Notification Type", fntparaBOLD);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Colspan = 9;
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(obj.NotificationType), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Colspan = 26;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Notification Status"
                cell = new PdfPCell();
                para = new Paragraph("Notification Status", fntparaBOLD);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Colspan = 9;
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(obj.NotificationStatus), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Colspan = 26;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Notification Number"
                cell = new PdfPCell();
                para = new Paragraph("Notification Number", fntparaBOLD);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Colspan = 9;
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(obj.NotificationNumber), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Colspan = 26;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Notification Doc"
                cell = new PdfPCell();
                para = new Paragraph("Notification", fntparaBOLD);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Colspan = 9;
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(obj.Notifica_Document), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Colspan = 26;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Date of Notification"
                cell = new PdfPCell();
                para = new Paragraph("Date of Notification", fntparaBOLD);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Colspan = 9;
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(obj.DateOfNotification), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Colspan = 26;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Final Date of Comment"
                cell = new PdfPCell();
                para = new Paragraph("Final Date of Comments", fntparaBOLD);
                para.SetLeading(leading, 0);
                cell.Colspan = 9;
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(obj.FinalDateOfComment), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Colspan = 26;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Send Response By"
                cell = new PdfPCell();
                para = new Paragraph("Send Response By", fntparaBOLD);
                para.SetLeading(leading, 0);
                cell.Colspan = 9;
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(obj.SendResponseBy), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Colspan = 26;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Country"
                cell = new PdfPCell();
                para = new Paragraph("Notifying Country", fntparaBOLD);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Colspan = 9;
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(obj.Country), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Colspan = 26;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Enquiry Point"
                cell = new PdfPCell();
                para = new Paragraph("Enquiry Point", fntparaBOLD);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Colspan = 9;
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(obj.EnquiryEmailId), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Colspan = 26;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Title"
                cell = new PdfPCell();
                para = new Paragraph("Title", fntparaBOLD);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Colspan = 9;
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(obj.Title), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Colspan = 26;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Agency Responsible"
                cell = new PdfPCell();
                para = new Paragraph("Agency Responsible", fntparaBOLD);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Colspan = 9;
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(obj.ResponsibleAgency), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Colspan = 26;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Under Article"
                cell = new PdfPCell();
                para = new Paragraph("Notification under Article", fntparaBOLD);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Colspan = 9;
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(obj.Articles), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Colspan = 26;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Product Covered"
                cell = new PdfPCell();
                para = new Paragraph("Products Covered", fntparaBOLD);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Colspan = 9;
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(obj.ProductCovered), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Colspan = 26;
                POTable.AddCell(cell);
                #endregion

                #region "Row : HS Codes"
                if (obj.HSCodes != null)
                {
                    PdfPTable tblHSCodes = new PdfPTable(6);
                    tblHSCodes.TotalWidth = 360.433070866f;
                    tblHSCodes.LockedWidth = true;

                    cell = new PdfPCell(new Phrase(leading, "HS Code", fntparaBOLD));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;

                    tblHSCodes.AddCell(cell);
                    cell = new PdfPCell(new Phrase(leading, "Name", fntparaBOLD));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.Colspan = 5;
                    tblHSCodes.AddCell(cell);

                    foreach (HSCodes hs in obj.HSCodes)
                    {
                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(hs.HSCode), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblHSCodes.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(hs.Text), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Colspan = 5;
                        tblHSCodes.AddCell(cell);
                    }

                    cell = new PdfPCell();
                    para = new Paragraph("HS Codes", fntparaBOLD);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Colspan = 9;
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    cell.SetLeading(leading, 0);
                    cell.AddElement(tblHSCodes);
                    cell.Colspan = 26;
                    cell.Border = 0;
                    POTable.AddCell(cell);
                }
                #endregion
                #region "Row : Description"
                cell = new PdfPCell();
                para = new Paragraph("Description of Content", fntparaBOLD);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Colspan = 9;
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                POTable.AddCell(cell);

                cell = new PdfPCell();
                para = new Paragraph(HttpUtility.HtmlDecode(obj.Description), fntparaNormal);
                para.SetLeading(leading, 0);
                cell.AddElement(para);
                cell.Border = 0;
                cell.Colspan = 26;
                POTable.AddCell(cell);
                #endregion

                #region "Row : Regulation"
                if (obj.Regulations != null)
                {
                    PdfPTable tblRegulation = new PdfPTable(6);
                    tblRegulation.TotalWidth = 360.433070866f;
                    tblRegulation.LockedWidth = true;

                    tblRegulation.AddCell(new PdfPCell(new Phrase(leading, "Full text of Regulation", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblRegulation.AddCell(new PdfPCell(new Phrase(leading, "Language", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblRegulation.AddCell(new PdfPCell(new Phrase(leading, "Translator", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblRegulation.AddCell(new PdfPCell(new Phrase(leading, "Sent to translator on", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblRegulation.AddCell(new PdfPCell(new Phrase(leading, "Translated full text of regulation", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblRegulation.AddCell(new PdfPCell(new Phrase(leading, "Received on", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    foreach (Regulations Reg in obj.Regulations)
                    {
                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Reg.DocumentName), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblRegulation.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Reg.Language), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblRegulation.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Reg.Translator), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblRegulation.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Reg.SentForTranslation), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblRegulation.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Reg.TranslatedDocName), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblRegulation.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Reg.ReceivedOn), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblRegulation.AddCell(cell);
                    }

                    cell = new PdfPCell();
                    para = new Paragraph("Full text of Regulation", fntparaBOLD);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Colspan = 9;
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    cell.SetLeading(leading, 0);
                    cell.AddElement(tblRegulation);
                    cell.Colspan = 26;
                    cell.Border = 0;
                    POTable.AddCell(cell);
                }
                #endregion

                #region "Row : Reminder for translation"
                if (obj.TranslationReminder != "")
                {
                    cell = new PdfPCell();
                    para = new Paragraph("Reminder for translation", fntparaBOLD);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Colspan = 9;
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    para = new Paragraph(HttpUtility.HtmlDecode(obj.TranslationReminder), fntparaNormal);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    cell.Colspan = 26;
                    POTable.AddCell(cell);
                }
                #endregion

                #region "Row : Due Date for Translation"
                if (obj.TranslationDueBy != "")
                {
                    cell = new PdfPCell();
                    para = new Paragraph("Due Date for Translation", fntparaBOLD);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Colspan = 9;
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    para = new Paragraph(HttpUtility.HtmlDecode(obj.TranslationDueBy), fntparaNormal);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    cell.Colspan = 26;
                    POTable.AddCell(cell);
                }
                #endregion

                #region "Notification related Stakeholders"
                if (obj.Stakholders != null)
                {
                    PdfPTable tblStakholders = new PdfPTable(5);
                    tblStakholders.TotalWidth = 360.433070866f;
                    tblStakholders.LockedWidth = true;
                    tblStakholders.AddCell(new PdfPCell(new Phrase(leading, "S No.", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblStakholders.AddCell(new PdfPCell(new Phrase(leading, "Stakeholder", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblStakholders.AddCell(new PdfPCell(new Phrase(leading, "Org. Name", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblStakholders.AddCell(new PdfPCell(new Phrase(leading, "Mail Sent", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblStakholders.AddCell(new PdfPCell(new Phrase(leading, "Response received", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    int i = 1;
                    foreach (RelatedStakeHolders rs in obj.Stakholders)
                    {
                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(i)), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholders.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(rs.FullName), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholders.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(rs.OrgName), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholders.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(rs.MailCount)), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholders.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(rs.ResponseCount)), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholders.AddCell(cell);
                        i++;
                    }

                    cell = new PdfPCell();
                    para = new Paragraph("Stakeholder", fntparaBOLD);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Colspan = 9;
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    cell.SetLeading(leading, 0);
                    cell.AddElement(tblStakholders);
                    cell.Colspan = 26;
                    cell.Border = 0;
                    POTable.AddCell(cell);
                }
                #endregion

                #region "Row : Stakeholder response by"
                if (obj.StakeholderResponseDueBy != "")
                {
                    cell = new PdfPCell();
                    para = new Paragraph("Stakeholder response by", fntparaBOLD);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Colspan = 9;
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    para = new Paragraph(HttpUtility.HtmlDecode(obj.StakeholderResponseDueBy), fntparaNormal);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    cell.Colspan = 26;
                    POTable.AddCell(cell);
                }
                #endregion

                #region "Notification related Mails"
                if (obj.StackholderMails != null)
                {
                    PdfPTable tblStakholderMails = new PdfPTable(10);
                    tblStakholderMails.SpacingAfter = 20f;
                    tblStakholderMails.TotalWidth = 360.433070866f;
                    tblStakholderMails.LockedWidth = true;
                    cell = new PdfPCell(new Phrase(leading, "S No.", fntparaBOLD));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    tblStakholderMails.AddCell(cell);
                    cell = new PdfPCell(new Phrase(leading, "Sent on", fntparaBOLD));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.Colspan = 2;
                    tblStakholderMails.AddCell(cell);
                    cell = new PdfPCell(new Phrase(leading, "Subject", fntparaBOLD));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.Colspan = 5;
                    tblStakholderMails.AddCell(cell);
                    cell = new PdfPCell(new Phrase(leading, "Stakeholder Count", fntparaBOLD));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.Colspan = 2;
                    tblStakholderMails.AddCell(cell);

                    //tblStakholderMails.AddCell(new PdfPCell(new Phrase(leading, "", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    //tblStakholderMails.AddCell(new PdfPCell(new Phrase(leading, "", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    //tblStakholderMails.AddCell(new PdfPCell(new Phrase(leading, "", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    int i = 1;
                    foreach (StackholderMail smd in obj.StackholderMails)
                    {
                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(i)), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblStakholderMails.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(smd.Date), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Colspan = 2;
                        tblStakholderMails.AddCell(cell);

                        cell = new PdfPCell();

                        para = CreateSimpleHtmlParagraph(smd.Message, fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Colspan = 5;
                        tblStakholderMails.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(smd.StakeholderCount)), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Colspan = 2;
                        tblStakholderMails.AddCell(cell);


                        i++;
                    }
                    cell = new PdfPCell();
                    para = new Paragraph("Mail Sent", fntparaBOLD);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Colspan = 9;
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    cell.SetLeading(leading, 0);
                    cell.AddElement(tblStakholderMails);
                    cell.Colspan = 26;
                    cell.Border = 0;
                    POTable.AddCell(cell);
                }
                #endregion

                #region "Mail Reveived"
                if (obj.MailReceived != null)
                {
                    PdfPTable tblReceivedMail = new PdfPTable(5);
                    tblReceivedMail.SpacingAfter = 20f;
                    tblReceivedMail.TotalWidth = 360.433070866f;
                    tblReceivedMail.LockedWidth = true;
                    tblReceivedMail.AddCell(new PdfPCell(new Phrase(leading, "S No.", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblReceivedMail.AddCell(new PdfPCell(new Phrase(leading, "Received on", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblReceivedMail.AddCell(new PdfPCell(new Phrase(leading, "Organization", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblReceivedMail.AddCell(new PdfPCell(new Phrase(leading, "Contact", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    tblReceivedMail.AddCell(new PdfPCell(new Phrase(leading, "Message", fntparaBOLD)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    int i = 1;
                    foreach (StakeholderMail smdMail in obj.MailReceived)
                    {
                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(i)), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        tblReceivedMail.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(smdMail.SentOn), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        //   cell.Colspan = 5;
                        tblReceivedMail.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(smdMail.OrgName), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        //   cell.Colspan = 5;
                        tblReceivedMail.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(smdMail.Name), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        //  cell.Colspan = 2;
                        tblReceivedMail.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(smdMail.Subject)), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        //  cell.Colspan = 2;
                        tblReceivedMail.AddCell(cell);
                        i++;
                    }
                    cell = new PdfPCell();
                    para = new Paragraph("Response received", fntparaBOLD);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Colspan = 9;
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    cell.SetLeading(leading, 0);
                    cell.AddElement(tblReceivedMail);
                    cell.Colspan = 26;
                    cell.Border = 0;
                    POTable.AddCell(cell);
                }
                #endregion

                #region "Actions"
                if (obj.Actions != null)
                {
                    PdfPTable tblMailAction = new PdfPTable(50);
                    tblMailAction.SpacingAfter = 20f;
                    tblMailAction.TotalWidth = 360.433070866f;
                    tblMailAction.LockedWidth = true;
                    int i = 1;
                    foreach (Notifi_Actions Act in obj.Actions)
                    {
                        RoundRectangle rr = new RoundRectangle(Act.ActionId);
                        if (Act.ActionId == 1)
                        {
                            cell = new PdfPCell(new PdfPCell(new Phrase("", fntparaNormal)) { CellEvent = rr });
                            cell.Colspan = 1;
                            cell.Border = 0;
                            tblMailAction.AddCell(cell);
                        }
                        else if (Act.ActionId == 2)
                        {
                            cell = new PdfPCell(new PdfPCell(new Phrase("", fntparaNormal)) { CellEvent = rr });
                            cell.Colspan = 1;
                            cell.Border = 0;
                            tblMailAction.AddCell(cell);
                        }
                        else if (Act.ActionId == 3)
                        {
                            cell = new PdfPCell(new PdfPCell(new Phrase("", fntparaNormal)) { CellEvent = rr });
                            cell.Colspan = 1;
                            cell.Border = 0;
                            tblMailAction.AddCell(cell);
                        }
                        else if (Act.ActionId == 4)
                        {
                            cell = new PdfPCell(new PdfPCell(new Phrase("", fntparaNormal)) { CellEvent = rr });
                            cell.Colspan = 1;
                            cell.Border = 0;
                            tblMailAction.AddCell(cell);
                        }

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(Act.Action)), fntparaBOLD);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Colspan = 49;
                        cell.Border = 0;
                        tblMailAction.AddCell(cell);

                        cell = new PdfPCell(new PdfPCell(new Phrase("", fntparaNormal)) { BackgroundColor = BaseColor.WHITE });
                        cell.Colspan = 1;
                        cell.Border = 0;
                        tblMailAction.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString("Required by")), fntparaBOLD);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Colspan = 14;
                        cell.Border = 0;
                        tblMailAction.AddCell(cell);


                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(Act.RequiredBy)), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Colspan = 35;
                        cell.Border = 0;
                        tblMailAction.AddCell(cell);

                        cell = new PdfPCell(new PdfPCell(new Phrase("", fntparaNormal)) { BackgroundColor = BaseColor.WHITE });
                        cell.Colspan = 1;
                        cell.Border = 0;
                        tblMailAction.AddCell(cell);


                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString("Completed on")), fntparaBOLD);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Colspan = 14;
                        cell.Border = 0;
                        tblMailAction.AddCell(cell);


                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(Convert.ToString(Act.TakenOn)), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Colspan = 35;
                        cell.Border = 0;
                        tblMailAction.AddCell(cell);

                        cell = new PdfPCell(new PdfPCell(new Phrase("", fntparaNormal)) { BackgroundColor = BaseColor.WHITE });
                        cell.Colspan = 1;
                        cell.Border = 0;
                        tblMailAction.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph();
                        para = CreateSimpleHtmlParagraph(Convert.ToString(Act.Subject), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Colspan = 49;
                        cell.Border = 0;
                        tblMailAction.AddCell(cell);

                        cell = new PdfPCell(new PdfPCell(new Phrase("", fntparaNormal)) { BackgroundColor = BaseColor.WHITE });
                        cell.Colspan = 1;
                        cell.Border = 0;
                        tblMailAction.AddCell(cell);

                        cell = new PdfPCell();
                        para = CreateSimpleHtmlParagraph(Convert.ToString(Act.Message), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Colspan = 49;
                        cell.Border = 0;
                        tblMailAction.AddCell(cell);
                        i++;
                    }
                    cell = new PdfPCell();
                    para = new Paragraph("Action", fntparaBOLD);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Colspan = 9;
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                    para.SetLeading(leading, 0);
                    cell.AddElement(para);
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    cell = new PdfPCell();
                    cell.SetLeading(leading, 0);
                    cell.AddElement(tblMailAction);
                    cell.Colspan = 26;
                    cell.Border = 0;
                    POTable.AddCell(cell);

                    if (obj.MeetingNotes != "")
                    {
                        #region "Row : Meeting Note"

                        cell = new PdfPCell();
                        para = new Paragraph("", fntparaBOLD);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Colspan = 9;
                        cell.Border = 0;
                        POTable.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph("Note", fntparaBOLD);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Colspan = 2;
                        cell.Border = 0;
                        POTable.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(":"), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Border = 0;
                        POTable.AddCell(cell);

                        cell = new PdfPCell();
                        para = new Paragraph(HttpUtility.HtmlDecode(obj.MeetingNotes), fntparaNormal);
                        para.SetLeading(leading, 0);
                        cell.AddElement(para);
                        cell.Border = 0;
                        cell.Colspan = 24;
                        POTable.AddCell(cell);
                        #endregion
                    }
                }
                #endregion

                POTable.SpacingAfter = 20f;
                document.Add(POTable);

                #endregion

                #region "Commented Document section"

                //#region "Document Section"
                //if (obj.DoesHaveDetails != null)
                //{
                //    POTable = new PdfPTable(1);
                //    POTable.TotalWidth = 515.433070866f;
                //    POTable.LockedWidth = true;
                //    POTable.SpacingAfter = 10.185f;
                //    POTable.AddCell(new PdfPCell(new Phrase(leading, "Document", fntheader)) { BackgroundColor = BaseColor.WHITE, Border = Rectangle.NO_BORDER });
                //    document.Add(POTable);

                //    POTable = new PdfPTable(2);
                //    POTable.TotalWidth = 515.433070866f;
                //    POTable.LockedWidth = true;
                //    POTable.SpacingAfter = 10.185f;
                //    POTable.DefaultCell.Border = 0;
                //    #region "Row : Have Detailed Notification"
                //    cell = new PdfPCell();
                //    para = new Paragraph("Have detailed notification provided?", fntparaBOLD);
                //    para.SetLeading(leading, 0);
                //    cell.AddElement(para);
                //    cell.Border = 0;
                //    cell.FixedHeight = 15;
                //    POTable.AddCell(cell);

                //    cell = new PdfPCell();
                //    para = new Paragraph(HttpUtility.HtmlDecode(":   " + (Convert.ToBoolean(obj.DoesHaveDetails) ? "Yes" : "No")), fntparaNormal);
                //    para.SetLeading(leading, 0);
                //    cell.AddElement(para);
                //    cell.Border = 0;
                //    cell.Border = 0;
                //    cell.Colspan = 6;
                //    cell.FixedHeight = 15;
                //    POTable.AddCell(cell);
                //    #endregion

                //    if (Convert.ToBoolean(obj.DoesHaveDetails))
                //    {
                //        #region "Row : Language"
                //        cell = new PdfPCell();
                //        para = new Paragraph("Notification Document in Language", fntparaBOLD);
                //        para.SetLeading(leading, 0);
                //        cell.AddElement(para);
                //        cell.Border = 0;
                //        cell.Colspan = 9;
                //        // cell.FixedHeight = 15;
                //        POTable.AddCell(cell);

                //        cell = new PdfPCell();
                //        para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.Language), fntparaNormal);
                //        para.SetLeading(leading, 0);
                //        cell.AddElement(para);
                //        cell.Border = 0;
                //        cell.Colspan = 11;
                //        // cell.FixedHeight = 15;
                //        POTable.AddCell(cell);
                //        #endregion

                //        if (obj.Language.ToLower() != "english" && obj.Translator != "")
                //        {
                //            #region "Row : Tranlsator"
                //            cell = new PdfPCell();
                //            para = new Paragraph("Translator", fntparaBOLD);
                //            para.SetLeading(leading, 0);
                //            cell.AddElement(para);
                //            cell.Colspan = 4;
                //            cell.Border = 0;
                //            POTable.AddCell(cell);

                //            cell = new PdfPCell();
                //            para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.Translator), fntparaNormal);
                //            para.SetLeading(leading, 0);
                //            cell.AddElement(para);
                //            cell.Border = 0;
                //            cell.Colspan = 6;
                //            POTable.AddCell(cell);
                //            #endregion

                //            #region "Row : Sent to tranlsator on"
                //            cell = new PdfPCell();
                //            para = new Paragraph("Sent to translator for translation on ", fntparaBOLD);
                //            para.SetLeading(leading, 0);
                //            cell.AddElement(para);
                //            cell.Border = 0;
                //            cell.Colspan = 4;
                //            POTable.AddCell(cell);

                //            cell = new PdfPCell();
                //            para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.SendToTranslaterOn), fntparaNormal);
                //            para.SetLeading(leading, 0);
                //            cell.AddElement(para);
                //            cell.Border = 0;
                //            cell.Colspan = 6;
                //            POTable.AddCell(cell);
                //            #endregion

                //            #region "Row : Remainder to tranlsator on"
                //            cell = new PdfPCell();
                //            para = new Paragraph("Remainder to translator for translation on ", fntparaBOLD);
                //            para.SetLeading(leading, 0);
                //            cell.AddElement(para);
                //            cell.Colspan = 4;
                //            cell.Border = 0;
                //            POTable.AddCell(cell);

                //            cell = new PdfPCell();
                //            para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.TranslationReminder), fntparaNormal);
                //            para.SetLeading(leading, 0);
                //            cell.AddElement(para);
                //            cell.Border = 0;
                //            cell.Colspan = 6;
                //            POTable.AddCell(cell);
                //            #endregion

                //            #region "Row : Translation Due Date"
                //            cell = new PdfPCell();
                //            para = new Paragraph("Translation Due Date ", fntparaBOLD);
                //            para.SetLeading(leading, 0);
                //            cell.AddElement(para);
                //            cell.Border = 0;
                //            cell.Colspan = 4;
                //            POTable.AddCell(cell);

                //            cell = new PdfPCell();
                //            para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.TranslationDueBy), fntparaNormal);
                //            para.SetLeading(leading, 0);
                //            cell.AddElement(para);
                //            cell.Border = 0;
                //            cell.Colspan = 6;
                //            POTable.AddCell(cell);
                //            #endregion

                //            #region "Row : Translation Due Date"
                //            cell = new PdfPCell();
                //            para = new Paragraph("Translation expected on ", fntparaBOLD);
                //            para.SetLeading(leading, 0);
                //            cell.AddElement(para);
                //            cell.Border = 0;
                //            cell.Colspan = 4;
                //            POTable.AddCell(cell);

                //            cell = new PdfPCell();
                //            para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.TranslationDueBy), fntparaNormal);
                //            para.SetLeading(leading, 0);
                //            cell.AddElement(para);
                //            cell.Border = 0;
                //            cell.Colspan = 6;
                //            POTable.AddCell(cell);
                //            #endregion

                //            #region "Row : Translated doc received on"
                //            if (obj.TranslatedDocUploadedOn != "")
                //            {
                //                cell = new PdfPCell();
                //                para = new Paragraph("Translated document received on ", fntparaBOLD);
                //                para.SetLeading(leading, 0);
                //                cell.AddElement(para);
                //                cell.Border = 0;
                //                cell.Colspan = 4;
                //                POTable.AddCell(cell);

                //                cell = new PdfPCell();
                //                para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.TranslatedDocUploadedOn), fntparaNormal);
                //                para.SetLeading(leading, 0);
                //                cell.AddElement(para);
                //                cell.Border = 0;
                //                cell.Colspan = 6;
                //                POTable.AddCell(cell);
                //            }
                //            #endregion
                //        }
                //    }
                //    else
                //    {
                //        #region "Row : Enquiry desk email-id"
                //        cell = new PdfPCell();
                //        para = new Paragraph("Enquiry desk email-id ", fntparaBOLD);
                //        para.SetLeading(leading, 0);
                //        cell.AddElement(para);
                //        cell.Border = 0;
                //        cell.Colspan = 4;
                //        POTable.AddCell(cell);

                //        cell = new PdfPCell();
                //        para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.EnquiryEmailId), fntparaNormal);
                //        para.SetLeading(leading, 0);
                //        cell.AddElement(para);
                //        cell.Border = 0;
                //        cell.Colspan = 6;
                //        POTable.AddCell(cell);
                //        #endregion

                //        #region "Row : Enquiry desk email sent on "
                //        if (obj.MailSentToEnquiryDeskOn != "")
                //        {
                //            cell = new PdfPCell();
                //            para = new Paragraph("Mail sent to enquiry desk on ", fntparaBOLD);
                //            para.SetLeading(leading, 0);
                //            cell.AddElement(para);
                //            cell.Border = 0;
                //            cell.Colspan = 4;
                //            POTable.AddCell(cell);

                //            cell = new PdfPCell();
                //            para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.MailSentToEnquiryDeskOn), fntparaNormal);
                //            para.SetLeading(leading, 0);
                //            cell.AddElement(para);
                //            cell.Border = 0;
                //            cell.Colspan = 6;
                //            POTable.AddCell(cell);
                //        }
                //        #endregion

                //        #region "Row : Enquiry desk email sent on "
                //        if (obj.ObtainDocumentBy != "")
                //        {
                //            cell = new PdfPCell();
                //            para = new Paragraph("Document obtained on ", fntparaBOLD);
                //            para.SetLeading(leading, 0);
                //            cell.AddElement(para);
                //            cell.Border = 0;
                //            cell.Colspan = 4;
                //            POTable.AddCell(cell);

                //            cell = new PdfPCell();
                //            para = new Paragraph(HttpUtility.HtmlDecode(":   " + obj.ObtainDocumentBy), fntparaNormal);
                //            para.SetLeading(leading, 0);
                //            cell.AddElement(para);
                //            cell.Border = 0;
                //            cell.Colspan = 6;
                //            POTable.AddCell(cell);
                //        }
                //        #endregion
                //    }
                //    POTable.SpacingAfter = 20f;
                //    document.Add(POTable);
                //}
                //#endregion
                #endregion "Commented Document section"

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
                string PdfFileName = GeneratePdf1(obj);

                using (ZipFile zip = new ZipFile())
                {
                    if (PdfFileName != "" && File.Exists(HttpContext.Current.Server.MapPath("/Attachments/Temp/" + PdfFileName)))
                        zip.AddItem(HttpContext.Current.Server.MapPath("/Attachments/Temp/" + PdfFileName), "");

                    if (obj.Documents != null)
                    {
                        foreach (EditAttachment ea in obj.Documents)
                        {
                            if (File.Exists(HttpContext.Current.Server.MapPath(ea.Path)))
                            {
                                FileInfo fi = new FileInfo(HttpContext.Current.Server.MapPath(ea.Path));
                                DirectoryInfo di = fi.Directory;
                                zip.AddItem(HttpContext.Current.Server.MapPath(ea.Path), di.Name);
                            }
                        }
                    }

                    zip.Save(mStream);

                    if (File.Exists(HttpContext.Current.Server.MapPath("/Attachments/Temp/" + PdfFileName)))
                        File.Delete(HttpContext.Current.Server.MapPath("/Attachments/Temp/" + PdfFileName));
                }
            }
        }

        public RegulationSummary NotificationSummary(Int64 NotificationId)
        {
            RegulationSummary objS = new RegulationSummary();
            return objS;
        }

        #endregion

        #region "Notification Actions/Meeting"
        public string GetMeetingNotes(int NotificationId)
        {
            NotificationDataManager objDM = new NotificationDataManager();
            string MeetingNote = "";
            DataTable dt = objDM.GetMeetingNotes(NotificationId);
            if (dt != null && dt.Rows.Count > 0)
                MeetingNote = Convert.ToString(dt.Rows[0]["MeetingNote"]);

            return MeetingNote;
        }
        public string UpdateMeetingNote(SaveNote obj)
        {
            NotificationDataManager objDM = new NotificationDataManager();
            string MeetingNote = "";
            DataTable dt = objDM.UpdateMeetingNote(obj);
            if (dt != null && dt.Rows.Count > 0)
                MeetingNote = Convert.ToString(dt.Rows[0]["MeetingNote"]);

            return MeetingNote;
        }
        public NotificationActions GetNotificationActions(Int64 Id, int ActionId)
        {
            NotificationActions objNA = new NotificationActions();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.EditActions(Id, ActionId);
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
                        objNA.IsUpdate = Convert.ToBoolean(dr["IsUpdate"]);
                        objNA.MeetingNotes = Convert.ToString(dr["MeetingNote"]);
                        objNA.RetainedForNextDiscussion = Convert.ToBoolean(dr["RetainedForNextDiscussion"]);
                    }
                }

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<NotificationActionDetail> NotificationActionList = new List<NotificationActionDetail>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        NotificationActionDetail objNotificationAction = new NotificationActionDetail();
                        objNotificationAction.NotificationActionId = Convert.ToInt64(dr["NotificationActionId"]);
                        objNotificationAction.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objNotificationAction.Action = Convert.ToString(dr["Action"]);
                        objNotificationAction.RequiredOn = Convert.ToString(dr["RequiredOn"]);
                        objNotificationAction.EnteredOn = Convert.ToString(dr["EnteredOn"]);
                        objNotificationAction.UpdatedOn = Convert.ToString(dr["UpdatedOn"]);
                        objNotificationAction.MailId = Convert.ToInt64(dr["MailId"]);
                        objNotificationAction.MailTo = Convert.ToString(dr["MailTo"]);
                        NotificationActionList.Add(objNotificationAction);
                    }
                    objNA.Actions = NotificationActionList;
                }
            }

            return objNA;
        }
        public AddNotificationAction_Output EditNotificationAction(Int64 NotificationActionId)
        {
            AddNotificationAction_Output objO = new AddNotificationAction_Output();
            NotificationDataManager objDM = new NotificationDataManager();
            DataTable dt = objDM.EditNotificationAction(NotificationActionId);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    objO.NotificationId = Convert.ToInt64(dr["NotificationId"]);
                    objO.NotificationActionId = Convert.ToInt64(dr["NotificationActionId"]);
                    objO.ActionId = Convert.ToInt32(dr["ActionId"]);
                    objO.NotificationNumber = Convert.ToString(dr["NotificationNumber"]);
                    objO.DateofNotification = Convert.ToString(dr["DatefNotification"]);
                    objO.ResponseCount = Convert.ToInt32(dr["ResponseCount"]);
                    objO.MailCount = Convert.ToInt32(dr["MailCount"]);
                    objO.MailTo = Convert.ToString(dr["EnquiryEmailId"]);
                }

                if (objO.ActionId == 1 || objO.ActionId == 2)
                {
                    Notification_Template_Search objS = new Notification_Template_Search();
                    objS.TemplateType = "Mail";
                    objS.TemplateFor = objO.ActionId == 1 ? "BrieftoRegulators" : "PolicyBrief";
                    objS.NotificationActionId = objO.NotificationActionId;
                    objO.MailDetails = GetNotificationSMSMailTemplate(objO.NotificationId, objS);
                }
            }
            return objO;
        }
        public SendActionMail_Output SaveAndSendMailforAction(long Id, ActionMailDetails obj)
        {
            SendActionMail_Output objOutput = new SendActionMail_Output();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.SendActionMail(Id, obj);
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
                            objOutput.Subject = Convert.ToString(dr["Subject"]);
                            objOutput.Body = Convert.ToString(dr["Message"]);

                            objOutput.MailTo = Convert.ToString(dr["MailTo"]);
                            objOutput.ReplyTo = Convert.ToString(dr["ReplyTo"]);
                            objOutput.CC = Convert.ToString(dr["CC"]);
                            objOutput.BCC = Convert.ToString(dr["BCC"]);
                            objOutput.DisplayName = Convert.ToString(dr["DisplayName"]);
                        }
                        if (obj.Attachments.Count > 0 && obj.Attachments != null)
                        {
                            foreach (Attachment objA in obj.Attachments)
                            {
                                if (objA.Content != "")
                                {
                                    try
                                    {
                                        byte[] bytes = null;
                                        if (objA.Content.IndexOf(',') >= 0)
                                        {
                                            var myString = objA.Content.Split(new char[] { ',' });
                                            bytes = Convert.FromBase64String(myString[1]);
                                        }
                                        else
                                            bytes = Convert.FromBase64String(objA.Content);

                                        if (objA.FileName.Length > 0 && bytes.Length > 0)
                                        {
                                            string filePath = HttpContext.Current.Server.MapPath("/Attachments/MailAttachment/" + Convert.ToString(dt.Rows[0]["MailId"]) + "_" + objA.FileName);
                                            File.WriteAllBytes(filePath, bytes);
                                        }
                                    }
                                    catch (Exception ex) { }
                                }
                            }
                        }
                    }
                }

                tblIndex++;
                if (ds.Tables.Count > tblIndex)
                {
                    using (DataTable dt = ds.Tables[tblIndex])
                    {
                        List<EditAttachment> AttachmentList = new List<EditAttachment>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            EditAttachment objA = new EditAttachment();
                            objA.FileName = Convert.ToString(dr["Attachment"]);
                            objA.Path = Convert.ToString(dr["DocumentPath"]);
                            AttachmentList.Add(objA);
                        }
                        objOutput.Attachments = AttachmentList;
                    }
                }

            }
            return objOutput;
        }
        public NotificationActionDetail ViewAction(long Id)
        {
            NotificationActionDetail objNA = new NotificationActionDetail();
            NotificationDataManager objDM = new NotificationDataManager();
            DataSet ds = objDM.ViewActions(Id);
            if (ds != null)
            {
                int tblIndex = -1;

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        objNA.NotificationActionId = Convert.ToInt64(dr["NotificationActionId"]);
                        objNA.ActionId = Convert.ToInt32(dr["ActionId"]);
                        objNA.Action = Convert.ToString(dr["Action"]);
                        objNA.RequiredOn = Convert.ToString(dr["RequiredOn"]);
                        objNA.EnteredOn = Convert.ToString(dr["EnteredOn"]);
                        objNA.UpdatedOn = Convert.ToString(dr["UpdatedOn"]);
                        objNA.MailId = Convert.ToInt64(dr["MailId"]);
                        objNA.MailTo = Convert.ToString(dr["MailTo"]);

                        Notification_Template objT = new Notification_Template();
                        objT.Subject = Convert.ToString(dr["Subject"]);
                        objT.Message = Convert.ToString(dr["Message"]);
                        objNA.MailDetails = objT;

                        objNA.ResponseId = Convert.ToInt64(dr["ResponseId"]);
                    }
                }

                tblIndex++;
                if (ds.Tables.Count > 0 && ds.Tables[tblIndex].Rows.Count > 0)
                {
                    List<EditAttachment> AttachmentList = new List<EditAttachment>();
                    foreach (DataRow dr in ds.Tables[tblIndex].Rows)
                    {
                        EditAttachment objA = new EditAttachment();
                        objA.FileName = Convert.ToString(dr["FileName"]);
                        objA.Path = Convert.ToString(dr["FilePath"]);
                        AttachmentList.Add(objA);
                    }
                    objNA.Attachments = AttachmentList;
                }
            }

            return objNA;
        }

        #endregion

        #region "Notification Mail"
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
                            if (obj.StakeHolderResponseDate != string.Empty)
                                objT.Subject = objT.Subject.Replace("#SendResponseBy#", Convert.ToString(obj.StakeHolderResponseDate));
                            else
                                objT.Subject = objT.Subject.Replace("#SendResponseBy#", Convert.ToString(dr["SendResponseBy"]));

                            objT.Subject = objT.Subject.Replace("#Country#", Convert.ToString(dr["Country"]));
                            objT.Subject = objT.Subject.Replace("#Title#", Convert.ToString(dr["Title"]));
                            objT.Subject = objT.Subject.Replace("#AgencyResponsible#", Convert.ToString(dr["AgencyResponsible"]));
                            objT.Subject = objT.Subject.Replace("#UnderArticle#", Convert.ToString(dr["UnderArticle"]));
                            objT.Subject = objT.Subject.Replace("#ProductCovered#", Convert.ToString(dr["ProductCovered"]));
                            objT.Subject = objT.Subject.Replace("#Description#", Convert.ToString(dr["Description"]));
                            //objT.Subject = objT.Subject.Replace("#StakeholderResponseDueBy#", Convert.ToString(dr["StakeholderResponseDueBy"]));
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
                            objT.Message = objT.Message.Replace("#NotificationAttachment#", Convert.ToString(dr["NotificationAttachment"]));

                            objT.Message = objT.Message.Replace("#NotificationStatus#", Convert.ToString(dr["NotificationStatus"]));
                            objT.Message = objT.Message.Replace("#NotificationNumber#", Convert.ToString(dr["NotificationNumber"]));
                            objT.Message = objT.Message.Replace("#DateOfNotification#", Convert.ToString(dr["DateOfNotification"]));
                            objT.Message = objT.Message.Replace("#FinalDateOfComment#", Convert.ToString(dr["FinalDateOfComment"]));
                            if (obj.StakeHolderResponseDate != string.Empty)
                                objT.Subject = objT.Subject.Replace("#SendResponseBy#", Convert.ToString(obj.StakeHolderResponseDate));
                            else
                                objT.Subject = objT.Subject.Replace("#SendResponseBy#", Convert.ToString(dr["SendResponseBy"]));
                            // objT.Message = objT.Message.Replace("#SendResponseBy#", Convert.ToString(dr["SendResponseBy"]));
                            objT.Message = objT.Message.Replace("#Country#", Convert.ToString(dr["Country"]));
                            objT.Message = objT.Message.Replace("#Title#", Convert.ToString(dr["Title"]));
                            objT.Message = objT.Message.Replace("#AgencyResponsible#", Convert.ToString(dr["AgencyResponsible"]));
                            objT.Message = objT.Message.Replace("#UnderArticle#", Convert.ToString(dr["UnderArticle"]));
                            objT.Message = objT.Message.Replace("#ProductCovered#", Convert.ToString(dr["ProductCovered"]));
                            objT.Message = objT.Message.Replace("#Description#", Convert.ToString(dr["Description"]));
                            //if (obj.StakeHolderResponseDate != string.Empty)
                            //    objT.Message = objT.Message.Replace("#StakeholderResponseDueBy#", Convert.ToString(obj.StakeHolderResponseDate));
                            //else
                            //    objT.Message = objT.Message.Replace("#StakeholderResponseDueBy#", Convert.ToString(dr["StakeholderResponseDueBy"]));
                            objT.Message = objT.Message.Replace("#EnquiryEmail#", Convert.ToString(dr["EnquiryEmailId"]));
                            objT.Message = objT.Message.Replace("#CreatedBy#", Convert.ToString(dr["CreatedBy"]));
                            objT.Message = objT.Message.Replace("#CreatedOn#", Convert.ToString(dr["CreatedOn"]));
                            objT.Message = objT.Message.Replace("#ObtainDocumentBy#", Convert.ToString(dr["ObtainDocumentBy"]));

                            if (obj.TemplateFor != "Translator")
                            {
                                objT.Message = objT.Message.Replace("#Language#", Convert.ToString(dr["Language"]));
                                objT.Message = objT.Message.Replace("#Translator#", Convert.ToString(dr["Translator"]));
                                objT.Message = objT.Message.Replace("#TranslationDueBy#", Convert.ToString(dr["TranslationDueBy"]));
                            }

                            objT.Message = objT.Message.Replace("#SendToTranslaterOn#", Convert.ToString(dr["SendToTranslaterOn"]));
                            objT.Message = objT.Message.Replace("#TranslationReminder#", Convert.ToString(dr["TranslationReminder"]));

                            objT.Message = objT.Message.Replace("#HSCodes#", Convert.ToString(dr["HSCodes"]));

                            #endregion
                        }
                    }
                }
            }
            return objT;
        }
        public List<EditAttachment> GetNotificationDocuments(Int64 Id)
        {
            List<EditAttachment> objE = new List<EditAttachment>();
            NotificationDataManager objDM = new NotificationDataManager();
            DataTable dt = objDM.GetNotificationDocuments(Id);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    EditAttachment obj = new EditAttachment();
                    obj.FileName = Convert.ToString(dr["FileName"]);
                    obj.Path = Convert.ToString(dr["Path"]);
                    objE.Add(obj);
                }
            }
            return objE;
        }
        #endregion

        #region "Response Action Mail"
        public SendMailStakeholders_Output SaveResponseActionMail(StakeholderResponse obj)
        {
            SendMailStakeholders_Output objOutput = new SendMailStakeholders_Output();
            NotificationDataManager objNDM = new NotificationDataManager();
            using (DataTable dt = objNDM.SaveResponseActionMail(obj))
            {
                if (dt != null && dt.Rows.Count > 0)
                {
                    StakeholderResponse objS = new StakeholderResponse();
                    objS.StakeholderResponseId = Convert.ToInt64(dt.Rows[0]["ResponseId"]);
                    objOutput.SRId = objS;
                }
            }
            return objOutput;
        }
        #endregion
    }
}
