using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Notification
{
    public class RegulationSummary
    {
        public long NotificationId { get; set; }
        public string NotificationStage { get; set; }
        public string SendResponseBy { get; set; }
        public string NotificationNumber { get; set; }
        public EditAttachment NotificationAttachment { get; set; }
        public string NotificationType { get; set; }
        public Int32 NotificationStatus { get; set; }
        public string DateofNotification { get; set; }
        public string FinalDateOfComments { get; set; }
        public string Country { get; set; }
        public string EnquiryEmailId { get; set; }
        public string Title { get; set; }
        public string ResponsibleAgency { get; set; }
        public string Articles { get; set; }
        public string ProductsCovered { get; set; }
        public List<NotificationHSCodes> HSCodes { get; set; }
        public string Description { get; set; }
        public Nullable<bool> DoesHaveDetails { get; set; }
        public string EnquiryEmailSentOn { get; set; }
        public string ObtainFullTextBy { get; set; }
        public List<RegulationFullText> RegulationFullTexts { get; set; }
        public string RemainderToTranslaterOn { get; set; }
        public string TranslationDueOn { get; set; }
        public List<Stakeholder> Stakeholders { get; set; }
        public string StakeholderResponseDueBy { get; set; }
        public string MeetingDate { get; set; }
        public List<RegulationAction> RegulationActions { get; set; }

        public List<StakeholderMail> StakeholderMails { get; set; }

        public List<RegulationResponse> RegulationResponses { get; set; }
    }

    public class NotificationHSCodes
    {
        public string HSCode { get; set; }
        public string Text { get; set; }
    }

    public class RegulationFullText
    {
        public EditAttachment FullTextDoc { get; set; }
        public string Language { get; set; }
        public string Translator { get; set; }
        public string SendToTranslatorOn { get; set; }
        public EditAttachment TranslatedFullTextDoc { get; set; }
        public string TranslatedDocReceivedOn { get; set; }

    }

    public class Stakeholder
    {
        public string Name { get; set; }
        public int MailsCount { get; set; }
        public int ResponseCount { get; set; }
        public string OrgName { get; set; }
        public string HSCodes { get; set; }
    }

    public class StakeholderMail: Stakeholder
    {
        public string SentOn { get; set; }
        public string Subject { get; set; }
        public List<EditAttachment> Attachments { get; set; }
        public int StakholdersCount { get; set; }
        //public string Name { get; set; }
        public long MailId { get; set; }
        public long StakeholderResponseId { get; set; }
        //public string OrgName { get; set; }
        public string Message { get; set; }
    }

    public class RegulationResponse
    {
        public string StakeholderName { get; set; }
        public string ReceivedOn { get; set; }
        public int Message { get; set; }
        public List<EditAttachment> Attachments { get; set; }
    }

    public class RegulationAction
    {
        public string ActionName { get; set; }
        public string RequiredOn { get; set; }
        public string CompletedOn { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public List<EditAttachment> Attachments { get; set; }
    }
}
