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
                if (obj.NotificationDoc != null)
                {
                    sqlCommand.Parameters.AddWithValue("@NotificationDocument", obj.NotificationDoc.FileName);
                    sqlCommand.Parameters.AddWithValue("@NotificationDocumentName", obj.NotificationDoc.DisplayName);
                }
                sqlCommand.Parameters.AddWithValue("@LanguageId ", obj.LanguageId);
                sqlCommand.Parameters.AddWithValue("@TranslatorId ", obj.TranslatorId);
                sqlCommand.Parameters.AddWithValue("@ObtainDocBy", obj.ObtainDocBy);
                sqlCommand.Parameters.AddWithValue("@StakeholderResponseDueBy", obj.StakeholderResponseDueBy);
                if (obj.TranslatedDoc != null)
                {
                    sqlCommand.Parameters.AddWithValue("@TranslatedDocument", obj.TranslatedDoc.FileName);
                    sqlCommand.Parameters.AddWithValue("@TranslatedDocumentName", obj.TranslatedDoc.DisplayName);
                }
                sqlCommand.Parameters.AddWithValue("@Role", obj.Role);
                sqlCommand.Parameters.AddWithValue("@SkippedToDiscussion", obj.SkippedToDiscussion);
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

        public DataTable SendDocumentToTranslater(SendToTranslater obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_SendToTranslate;
                sqlCommand.Parameters.AddWithValue("@NotificationId", obj.NotificationId);
                sqlCommand.Parameters.AddWithValue("@DoesHaveDetails", obj.DoesHaveDetails);
                sqlCommand.Parameters.AddWithValue("@NotificationDocument", obj.NotificationDoc.FileName);
                sqlCommand.Parameters.AddWithValue("@NotificationDocumentName", obj.NotificationDoc.DisplayName);
                sqlCommand.Parameters.AddWithValue("@LanguageId", obj.LanguageId);
                sqlCommand.Parameters.AddWithValue("@TranslatorId", obj.TranslaterId);
                sqlCommand.Parameters.AddWithValue("@TranslationDueOn", obj.TranslationDueOn);
                sqlCommand.Parameters.AddWithValue("@TranslationReminderOn", obj.TranslationReminderOn);
                sqlCommand.Parameters.AddWithValue("@UserId", obj.UserId);
                sqlCommand.Parameters.AddWithValue("@Role", obj.Role);
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
                sqlCommand.Parameters.AddWithValue("@Attachments", obj._Attachments);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

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
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

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

        public DataTable InsertUpdateActions(Int64 UserId, AddNotificationAction obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_InsertUpdateAction;
                sqlCommand.Parameters.AddWithValue("@UserId", UserId);
                sqlCommand.Parameters.AddWithValue("@NotificationActionId", obj.NotificationActionId);
                sqlCommand.Parameters.AddWithValue("@MeetingDate", obj.Meetingdate);

                sqlCommand.Parameters.AddWithValue("@NotificationId", obj.NotificationId);
                sqlCommand.Parameters.AddWithValue("@ActionId", obj.ActionId);
                sqlCommand.Parameters.AddWithValue("@DueOn", obj.RequiredOn);
                if (obj.Attachment != null)
                    sqlCommand.Parameters.AddWithValue("@Attachment", obj.Attachment.FileName);
                sqlCommand.Parameters.AddWithValue("@Remarks", obj.Remarks);
                sqlCommand.Parameters.AddWithValue("@Status", obj.Status);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet NotificationActions(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_Actions;
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet MailSMSTemplate(long Id, Notification_Template_Search obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_GetTemplate;
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                sqlCommand.Parameters.AddWithValue("@TemplateFor", obj.TemplateFor);
                sqlCommand.Parameters.AddWithValue("@TemplateType", obj.TemplateType);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataTable NotificationMails(long Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_Mails;
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
    }
}
