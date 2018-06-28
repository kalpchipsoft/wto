using BusinessObjects.ManageAccess;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Notification
{
    public class SendToTranslater
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public int NotificationId { get; set; }

        [Required]
        public bool DoesHaveDetails { get; set; }

        public Attachment NotificationDoc { get; set; }
        public int LanguageId { get; set; }
        public int TranslaterId { get; set; }
        public DateTime TranslationDueOn { get; set; }

        public DateTime TranslationReminderOn { get; set; }
    }

    public class SendToTranslater_Output : TranslatorInfo
    {
        public string CreatedBy { get; set; }
        public string CreatorEmailId { get; set; }

        public string TranslaterEmailId { get; set; }
        public string Language { get; set; }

        public string TranslationDueOn { get; set; }
    }

    public class SendMailToEnquiryDesk
    {
        [Required]
        public string Role { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public string EnquiryEmailId { get; set; }
        public NotificationMail MailDetails { get; set; }
    }
    public class SendMail_Output
    {
        public NotificationMail MailDetails { get; set; }
        public string ReplyTo { get; set; }
        public string MailTo { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string DisplayName { get; set; }
    }
}
