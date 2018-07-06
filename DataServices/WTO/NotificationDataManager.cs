using System;
using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using BusinessObjects.Notification;
using UtilitiesManagers;

namespace DataServices.WTO
{
    public class NotificationDataManager
    {
        #region "Add/Edit Notification"
        public DataSet InsertUpdate_Notification(AddNotification obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_AddUpdate;
                sqlCommand.Parameters.AddWithValue("@UserId", obj.UserId);
                sqlCommand.Parameters.AddWithValue("@NotificationId", obj.NotificationId);
                sqlCommand.Parameters.AddWithValue("@NotificationType", obj.NotificationType);
                sqlCommand.Parameters.AddWithValue("@NotificationNumber", obj.NotificationNumber);
                sqlCommand.Parameters.AddWithValue("@NotificationStatus", obj.NotificationStatus);
                sqlCommand.Parameters.AddWithValue("@DateOfNotification", obj.DateofNotification);
                sqlCommand.Parameters.AddWithValue("@DateOfComments", obj.FinalDateOfComments);
                sqlCommand.Parameters.AddWithValue("@SendResponseBy", obj.SendResponseBy);
                sqlCommand.Parameters.AddWithValue("@CountryId", obj.CountryId);
                sqlCommand.Parameters.AddWithValue("@Title", obj.Title);
                sqlCommand.Parameters.AddWithValue("@ResponsibleAgency", obj.ResponsibleAgency);
                sqlCommand.Parameters.AddWithValue("@UnderArticle", obj.UnderArticle);
                sqlCommand.Parameters.AddWithValue("@ProductsCovered", obj.ProductsCovered);
                sqlCommand.Parameters.AddWithValue("@HSCodes", obj.HSCodes);
                sqlCommand.Parameters.AddWithValue("@Description", obj.Description);
                sqlCommand.Parameters.AddWithValue("@EnquiryEmail", obj.EnquiryEmail);
                sqlCommand.Parameters.AddWithValue("@DoesHaveDetails", obj.DoesHaveDetails);

                if (obj.NotificationAttachment != null)
                    sqlCommand.Parameters.AddWithValue("@NotificationAttachment", obj.NotificationAttachment.FileName);

                if (obj.NotificationDoc != null && obj.NotificationDoc.Count > 0)
                    sqlCommand.Parameters.AddWithValue("@NotificationDocument", obj.NotificationDocXML);

                sqlCommand.Parameters.AddWithValue("@ObtainDocBy", obj.ObtainDocBy);
                sqlCommand.Parameters.AddWithValue("@TranslationReminder", obj.TranslationReminder);
                sqlCommand.Parameters.AddWithValue("@TranslationDueBy", obj.TranslationDueDate);
                sqlCommand.Parameters.AddWithValue("@StakeholderResponseDueBy", obj.StakeholderResponseDueBy);

                if (obj.TranslatedDoc != null && obj.TranslatedDoc.Count > 0)
                    sqlCommand.Parameters.AddWithValue("@TranslatedDocument", obj.TranslatedDocXML);

                sqlCommand.Parameters.AddWithValue("@Role", obj.Role);
                sqlCommand.Parameters.AddWithValue("@SkippedToDiscussion", obj.SkippedToDiscussion);
                sqlCommand.Parameters.AddWithValue("@RetainedForNextDiscussion", obj.RetainedforNextDiscussion);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet Edit_Notification(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_Edit;
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataTable ValidateNotification(CheckNotification obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_Validate;
                sqlCommand.Parameters.AddWithValue("@NotificationId", obj.NotificationId);
                sqlCommand.Parameters.AddWithValue("@NotificationNumber", obj.NotificationNumber);
                sqlCommand.Parameters.AddWithValue("@DateOfNotification", obj.DateOfNotification);
                sqlCommand.Parameters.AddWithValue("@DocFinalDateOfComments", obj.FinalDateOfComment);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataTable getCalculatedDate(Int64 Id, string DateOfNotification, string FinalDateOfComments)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_getCalculatedDate;
                sqlCommand.Parameters.AddWithValue("@DateOfNotification", DateOfNotification);
                sqlCommand.Parameters.AddWithValue("@DocFinalDateOfComments", FinalDateOfComments);
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet GetMailDetailsSendtoStakeholder(string MailId, string Callfor)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_MailSendToStakeHolder;
                sqlCommand.Parameters.AddWithValue("@MailId", MailId);
                sqlCommand.Parameters.AddWithValue("@Callfor", Callfor);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        #endregion

        #region "View notification"
        public DataSet ViewNotifications(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.ViewNotification;
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet NotificationSummary(Nullable<Int64> MomId, string CallFor, Nullable<Int64> RowNum, string DataFor)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.NotificationSummary;
                sqlCommand.Parameters.AddWithValue("@MoMId", MomId);
                sqlCommand.Parameters.AddWithValue("@CallFor", CallFor);
                sqlCommand.Parameters.AddWithValue("@RowNo", RowNum);
                sqlCommand.Parameters.AddWithValue("@DataFor", DataFor);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        #endregion

        #region "Notification Stakeholders"
        public DataTable RelatedStakeholders(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_RelatedStakeholders;
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataTable StakeHolderConversation(Int64 NotificationId, Int64 StakeholderId)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_GetStakeHolderConversation;
                sqlCommand.Parameters.AddWithValue("@NotificationId", NotificationId);
                sqlCommand.Parameters.AddWithValue("@StakeholderId", StakeholderId);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataTable InsertDelete_RelatedStakeholders(Int64 Id, string SelectedStakeHolder, bool IsDelete)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_AddRemoveRelatedStakeholders;
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                sqlCommand.Parameters.AddWithValue("@SelectedStakeHolder", SelectedStakeHolder);
                sqlCommand.Parameters.AddWithValue("@IsDelete", IsDelete);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet SendMailToStakeHolders(SendMailStakeholders obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_SendMailToStakeholders;
                sqlCommand.Parameters.AddWithValue("@UserId", obj.UserId);
                sqlCommand.Parameters.AddWithValue("@NotificationId", obj.NotificationId);
                sqlCommand.Parameters.AddWithValue("@Stakeholders", obj.Stakeholders);
                sqlCommand.Parameters.AddWithValue("@Subject", obj.Subject);
                sqlCommand.Parameters.AddWithValue("@Message", obj.Message);
                sqlCommand.Parameters.AddWithValue("@Attachments", obj.AttachmentXML);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet NotificationMails(long Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_Mails;
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet SaveStakeholderResponse(StakeholderResponse obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.SaveStakeholderResponse;
                sqlCommand.Parameters.AddWithValue("@MailId", obj.MailId);
                sqlCommand.Parameters.AddWithValue("@NotificationId", obj.NotificationId);
                sqlCommand.Parameters.AddWithValue("@StakeholderId", obj.StakeholderId);
                sqlCommand.Parameters.AddWithValue("@Subject", obj.Subject);
                sqlCommand.Parameters.AddWithValue("@Message", obj.Message);
                sqlCommand.Parameters.AddWithValue("@Attachments", obj.ResponseAttachmentXML);
                sqlCommand.Parameters.AddWithValue("@ReceivedOn", obj.ResponseReceivedOn);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataTable GetStakeHoldersMaster(string SearchText)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetStakeholderMaster;
                sqlCommand.Parameters.AddWithValue("@SearchText", SearchText);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet ViewResponseFromStackHolder(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_ViewResponseDetails;
                sqlCommand.Parameters.AddWithValue("@ResponseMailId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        #endregion

        #region "Notification full text of documents
        public DataTable SendMailToEnquiryDesk(long Id, SendMailToEnquiryDesk obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_SendMailToEnquiryDesk;
                sqlCommand.Parameters.AddWithValue("@UserId", obj.UserId);
                sqlCommand.Parameters.AddWithValue("@Role", obj.Role);
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                sqlCommand.Parameters.AddWithValue("@Subject", obj.MailDetails.Subject);
                sqlCommand.Parameters.AddWithValue("@Message", obj.MailDetails.Body);
                sqlCommand.Parameters.AddWithValue("@EnquiryMailId", obj.EnquiryEmailId);
                sqlCommand.Parameters.AddWithValue("@ObtainDocBy", obj.ObtainDocBy);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet SendDocumentToTranslater(SendToTranslater obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_SendToTranslate;
                sqlCommand.Parameters.AddWithValue("@NotificationId", obj.NotificationId);
                sqlCommand.Parameters.AddWithValue("@TranslatorId", obj.TranslaterId);
                sqlCommand.Parameters.AddWithValue("@TranslationDueOn", obj.TranslationDueOn);
                sqlCommand.Parameters.AddWithValue("@TranslationReminderOn", obj.TranslationReminderOn);
                sqlCommand.Parameters.AddWithValue("@UserId", obj.UserId);
                sqlCommand.Parameters.AddWithValue("@Attachments", obj.AttachmentXML);
                sqlCommand.Parameters.AddWithValue("@Subject", obj.MailDetails.Subject);
                sqlCommand.Parameters.AddWithValue("@Message", obj.MailDetails.Message);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        #endregion

        #region "Notification Details"
        public DataSet GetDetails_Notification(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_GetNotificationDetails;
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        #endregion

        #region "Notification Actions/Meeting"
        public DataTable GetMeetingNotes(int NotificationId)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetMeetingNote;
                sqlCommand.Parameters.AddWithValue("@NoteId", NotificationId);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataTable UpdateMeetingNote(SaveNote obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.InsertUpdateNote;
                sqlCommand.Parameters.AddWithValue("@NotificationId", obj.NotificationId);
                sqlCommand.Parameters.AddWithValue("@Note", obj.MeetingNote);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataTable EditNotificationAction(Int64 NotificationActionId)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_EditAction;
                sqlCommand.Parameters.AddWithValue("@NotificationActionId", NotificationActionId);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet EditActions(Int64 Id, int ActionId)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_Actions;
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                sqlCommand.Parameters.AddWithValue("@ActionId", ActionId);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet ViewActions(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_ViewActionDetails;
                sqlCommand.Parameters.AddWithValue("@NotificationActionId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet SendActionMail(long Id, ActionMailDetails obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_SendActionMail;
                sqlCommand.Parameters.AddWithValue("@UserId", obj.UserId);
                sqlCommand.Parameters.AddWithValue("@NotificationActionId", Id);
                sqlCommand.Parameters.AddWithValue("@Subject", obj.Subject);
                sqlCommand.Parameters.AddWithValue("@Message", obj.Body);
                sqlCommand.Parameters.AddWithValue("@MailTo", obj.MailTo);
                sqlCommand.Parameters.AddWithValue("@Attachments", obj.AttachmentsXML);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        #endregion

        #region "Notification Mail"
        public DataSet MailSMSTemplate(long Id, Notification_Template_Search obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_GetTemplate;
                sqlCommand.Parameters.AddWithValue("@TemplateFor", obj.TemplateFor);
                sqlCommand.Parameters.AddWithValue("@TemplateType", obj.TemplateType);
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                sqlCommand.Parameters.AddWithValue("@NotificationDocumentId", obj.NotificationDocumentId);
                sqlCommand.Parameters.AddWithValue("@NotificationActionId", obj.NotificationActionId);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataTable GetNotificationDocuments(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_GetNotificationRelatedDocuments;
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        #endregion
    }
}
