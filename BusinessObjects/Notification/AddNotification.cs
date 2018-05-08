using System;
using System.Collections.Generic;
using BusinessObjects.Masters;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Notification
{
    public class AddNotification
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public long NotificationId { get; set; }
        [Required]
        public string NotificationType { get; set; }
        [Required]
        public string NotificationNumber { get; set; }
        [Required]
        public Int32 NotificationStatus { get; set; }
        [Required]
        public DateTime DateofNotification { get; set; }
        [Required]
        public DateTime FinalDateOfComments { get; set; }
        [Required]
        public DateTime SendResponseBy { get; set; }
        [Required]
        public Int32 CountryId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string ResponsibleAgency { get; set; }
        [Required]
        public string UnderArticle { get; set; }
        [Required]
        public string ProductsCovered { get; set; }
        [Required]
        public string HSCodes { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Role { get; set; }
        public Attachment NotificationAttachment { get; set; }
        public Nullable<bool> DoesHaveDetails { get; set; }
        public Attachment NotificationDoc { get; set; }
        public Int32 LanguageId { get; set; }
        public Int32 TranslatorId { get; set; }
        public Attachment TranslatedDoc { get; set; }
        public Nullable<DateTime> ObtainDocBy { get; set; }
        public Nullable<DateTime> StakeholderResponseDueBy { get; set; }
        public string EnquiryEmail { get; set; }
        public Nullable<bool> SkippedToDiscussion { get; set; }
    }

    public class NotificationDetails
    {
        public long NotificationId { get; set; }
        public string NotificationType { get; set; }
        public string NotificationNumber { get; set; }
        public Int32 NotificationStatus { get; set; }
        public string DateofNotification { get; set; }
        public string FinalDateOfComments { get; set; }
        public string SendResponseBy { get; set; }
        public string Country { get; set; }
        public Int32 CountryId { get; set; }
        public string Title { get; set; }
        public string ResponsibleAgency { get; set; }
        public string Articles { get; set; }
        public string ProductsCovered { get; set; }
        public string HSCodes { get; set; }
        public string Description { get; set; }
        public string EnquiryEmailId { get; set; }
        public string EnquiryEmailSentOn { get; set; }
        public EditAttachment NotificationAttachment { get; set; }
        public Nullable<bool> DoesHaveDetails { get; set; }
        public EditAttachment NotificationDoc { get; set; }
        public string NotificationDocName { get; set; }
        public Int32 LanguageId { get; set; }
        public Int32 TranslaterId { get; set; }
        public string RemainderToTranslaterOn { get; set; }
        public string TranslationDueOn { get; set; }
        public string SentToTranslaterOn { get; set; }
        public EditAttachment TranslatedDoc { get; set; }
        public string TranslatedDocName { get; set; }
        public string TranslatedDocUploadedOn { get; set; }
        public string ObtainDocBy { get; set; }
        public string Stakeholders { get; set; }
        public string StakeholderResponseDueBy { get; set; }
        public string NotificationDiscussedOn { get; set; }
        public Nullable<bool> SkippedToDiscussion { get; set; }
        public int Status { get; set; }
    }

    public class AddNoti_Result : CommonResponseModel
    {
        public long NotificationId { get; set; }
    }

    public class EditNotification : NotificationDetails
    {
        public List<NotificationStatus> NotificationStatusList { get; set; }
        public List<Masters.Country> CountryList { get; set; }
        public List<StakeHolderMaster> StakeHoldersList { get; set; }
        public List<HSCodes> SelectedHSCodes { get; set; }
        public List<StackholderMail> Mails { get; set; }
    }
    
    public class Attachment
    {
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
    }

    public class EditAttachment
    {
        public string FileName { get; set; }
        public string Path { get; set; }
    }

    public class NotificationMail
    {
        public long MailId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string MailFor { get; set; }
    }
    
    public sealed class NotificationDetails_Pdf
    {
        public long NotificationId { get; set; }
        public string NotificationType { get; set; }
        public string NotificationStatus { get; set; }
        public string NotificationNumber { get; set; }
        public string DateOfNotification { get; set; }
        public string FinalDateOfComment { get; set; }
        public string SendResponseBy { get; set; }
        public string Country { get; set; }
        public string Title { get; set; }
        public string ResponsibleAgency { get; set; }
        public string Articles { get; set; }
        public string ProductCovered { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public Nullable<bool> DoesHaveDetails { get; set; }
        public string EnquiryEmailId { get; set; }
        public string MailSentToEnquiryDeskOn { get; set; }
        public string ObtainDocumentBy { get; set; }
        public string Language { get; set; }
        public string Translator { get; set; }
        public string SendToTranslaterOn { get; set; }
        public string TranslationReminder { get; set; }
        public string TranslationDueBy { get; set; }
        public string TranslatedDocUploadedOn { get; set; }
        public string StakeholderResponseDueBy { get; set; }
        public string NotificationDiscussedOn { get; set; }
        public List<HSCodes> HSCodes { get; set; }
        public List<RelatedStakeHolders> Stakholders { get; set; }
        public List<StackholderMail> StackholderMails { get; set; }
        public List<EditAttachment> Documents { get; set; }
    }
}
