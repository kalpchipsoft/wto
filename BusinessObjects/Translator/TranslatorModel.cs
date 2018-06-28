using System;

namespace BusinessObjects.Translator
{
    public class Translator_Search
    {
        public Int64 TranslatorId { get; set; }
    }

    public class SearchDocument : Translator_Search
    {
        public Nullable<DateTime> ReceivedOn { get; set; }
        public Nullable<DateTime> SubmissionDueOn { get; set; }
        public string NotificationNumber { get; set; }
        public string Status { get; set; }
    }

    public class ChangePassword : Translator_Search
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class NotificationDocument
    {
        public long NotificationId { get; set; }
        public long NotificationDocumentId { get; set; }
        public string NotificationNumber { get; set; }
        public EditAttachment UntranslatedDocument { get; set; }
        public string SendToTranslaterOn { get; set; }
        public string TranslationDueBy { get; set; }
        public EditAttachment TranslatedDocument { get; set; }
    }

    public class EditAttachment
    {
        public string DisplayName { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
    }

    public class Attachment
    {
        public string FileName { get; set; }
        public string Content { get; set; }
    }

    public class UploadDocument
    {
        public long NotificationId { get; set; }
        public long NotificationDocumentId { get; set; }
        public string DisplayName { get; set; }
        public Attachment Document { get; set; }
    }
}
