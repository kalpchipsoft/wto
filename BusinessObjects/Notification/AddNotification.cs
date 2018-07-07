using System;
using System.Collections.Generic;
using BusinessObjects.Masters;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using System.Xml;
using System.Text;
using System.IO;

namespace BusinessObjects.Notification
{
    public class DocumentType
    {
        public int DocumentTypeId { get; set; }
        public string Type { get; set; }
    }
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
        public List<Attachment_TranslatedDoc> NotificationDoc { get; set; }
        public List<Attachment_TranslatedDoc> TranslatedDoc { get; set; }
        public Nullable<DateTime> ObtainDocBy { get; set; }
        public Nullable<DateTime> TranslationReminder { get; set; }
        public Nullable<DateTime> TranslationDueDate { get; set; }
        public Nullable<DateTime> StakeholderResponseDueBy { get; set; }
        public string EnquiryEmail { get; set; }
        public Nullable<bool> SkippedToDiscussion { get; set; }
        public Nullable<bool> RetainedforNextDiscussion { get; set; }
        [Required]
        public int DocumentTypeId { get; set; }
        public string NotificationDocXML
        {
            get
            {
                //Blank Namespace
                XmlSerializerNamespaces Namespace = new XmlSerializerNamespaces();
                Namespace.Add(string.Empty, string.Empty);

                //Remove xml declaration
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                xws.Encoding = Encoding.UTF8;

                //Stream to hold the serialize xml
                StringWriter sw = new StringWriter();

                XmlWriter xw = XmlWriter.Create(sw, xws);

                //Create Serializer object for required Class
                XmlSerializer serializer = new XmlSerializer(typeof(List<Attachment_TranslatedDoc>), new XmlRootAttribute("NotificationDocs"));
                serializer.Serialize(xw, NotificationDoc, Namespace);

                //Load XML to document
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sw.ToString());
                return doc.InnerXml;
            }
        }

        public string TranslatedDocXML
        {
            get
            {
                //Blank Namespace
                XmlSerializerNamespaces Namespace = new XmlSerializerNamespaces();
                Namespace.Add(string.Empty, string.Empty);

                //Remove xml declaration
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                xws.Encoding = Encoding.UTF8;

                //Stream to hold the serialize xml
                StringWriter sw = new StringWriter();

                XmlWriter xw = XmlWriter.Create(sw, xws);

                //Create Serializer object for required Class
                XmlSerializer serializer = new XmlSerializer(typeof(List<Attachment_TranslatedDoc>), new XmlRootAttribute("TranslatedDocs"));
                serializer.Serialize(xw, TranslatedDoc, Namespace);

                //Load XML to document
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sw.ToString());
                return doc.InnerXml;
            }
        }
    }

    public class Attachment_TranslatedDoc : Attachment
    {
        public Int64 DocumentId { get; set; }
        public int LanguageId { get; set; }
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
        public Int32 TranslaterId { get; set; }
        public string RemainderToTranslaterOn { get; set; }
        public string TranslationDueOn { get; set; }
        public string ObtainDocBy { get; set; }
        public string Stakeholders { get; set; }
        public string TotalResponses { get; set; }
        public string StakeholderResponseDueBy { get; set; }
        public string NotificationDiscussedOn { get; set; }
        public Nullable<bool> SkippedToDiscussion { get; set; }
        public int Status { get; set; }
        public string TotalMails { get; set; }
        public string MeetingNotes { get; set; }
        public int RegulationFlag { get; set; }
        public int TranslationFlag { get; set; }
        public int StakholderMailFlag { get; set; }
        public int MeetingFlag { get; set; }
    }

    public class AddNoti_Result : CommonResponseModel
    {
        public long NotificationId { get; set; }
        public List<EditAttachment> Attachments { get; set; }
    }

    public class EditNotification : NotificationDetails
    {
        public List<DocumentType> DocumentTypeList { get; set; }
        public List<NotificationStatus> NotificationStatusList { get; set; }
        public List<HSCodes> SelectedHSCodes { get; set; }
        public List<Regulations> Regulations { get; set; }
    }

    public class Regulations
    {
        public long DocumentId { get; set; }
        public string DocumentName { get; set; }
        public EditAttachment Document { get; set; }
        public long LanguageId { get; set; }
        public string Language { get; set; }
        public long TranslatorId { get; set; }
        public string Translator { get; set; }
        public string SentForTranslation { get; set; }
        public string TranslatedDocName { get; set; }
        public EditAttachment TranslatedDoc { get; set; }
        public string ReceivedOn { get; set; }
    }

    public class Attachment
    {
        public string DisplayName { get; set; }
        public string FileName { get; set; }

        [XmlIgnore]
        public string Content { get; set; }
    }

    public class EditAttachment
    {
        public string DisplayName { get; set; }
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
        public string Notifica_Document { get; set; }
        public string EnquiryPoint { get; set; }

        public List<Regulations> Regulations { get; set; }
        public List<StakeholderMail> MailReceived { get; set; }
        public List<Notifi_Actions> Actions { get; set; }
        public string MeetingNotes { get; set; }
    }

    public class StakeHolderConversationPopUp
    {
        public long NotificationNumber { get; set; }
        public string Title { get; set; }
        public string StakeHolderName { get; set; }
        public List<StakeHolderConversation> Conversation { get; set; }
    }

    public class StakeHolderConversation
    {
        public Int32 RowNumber { get; set; }
        public long MailId { get; set; }
        public string MailDate { get; set; }
        public string MailTime { get; set; }
        public string MailSubject { get; set; }
        public string MailMessage { get; set; }
        public bool MailType { get; set; }
        public string Attachments { get; set; }
        public string FileName { get; set; }
    }

    public class StakeholderResponse
    {
        public long MailId { get; set; }
        public long NotificationId { get; set; }
        public long StakeholderId { get; set; }
        public string ResponseReceivedOn { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public long StakeholderResponseId { get; set; }
        public List<ResponseAttachment> ResponseDocuments { get; set; }
        public string ResponseAttachmentXML
        {
            get
            {
                //Blank Namespace
                XmlSerializerNamespaces Namespace = new XmlSerializerNamespaces();
                Namespace.Add(string.Empty, string.Empty);

                //Remove xml declaration
                XmlWriterSettings xws = new XmlWriterSettings();
                xws.OmitXmlDeclaration = true;
                xws.Encoding = Encoding.UTF8;

                //Stream to hold the serialize xml
                StringWriter sw = new StringWriter();

                XmlWriter xw = XmlWriter.Create(sw, xws);

                //Create Serializer object for required Class
                XmlSerializer serializer = new XmlSerializer(typeof(List<ResponseAttachment>), new XmlRootAttribute("ResponseDocuments"));
                serializer.Serialize(xw, ResponseDocuments, Namespace);

                //Load XML to document
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sw.ToString());
                return doc.InnerXml;
            }
        }
    }

    public class StakeHoldersList
    {
        public List<StakeHolderMaster> StakeHolders { get; set; }
    }

    public class CheckNotification
    {
        public long NotificationId { get; set; }
        public string NotificationNumber { get; set; }
        public string DateOfNotification { get; set; }
        public string FinalDateOfComment { get; set; }
    }

    public class ValidateNotification_Output
    {
        public bool IsExists { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string EnquiryDeskEmail { get; set; }
        public string FinalDateofComment { get; set; }
        public string SendResponseBy { get; set; }
        public string StakeholderResponseBy { get; set; }

    }

    public class ViewNotification
    {
        public long NotificationId { get; set; }
        public string NotificationStage { get; set; }
        public string NotificationType { get; set; }
        public string NotificationNumber { get; set; }
        public string Notifica_Document { get; set; }
        public string Notifica_Document_path
        {
            get
            {
                return NotificationId + "_" + Notifica_Document;
            }
        }
        public string NotificationStatus { get; set; }
        public string DateofNotification { get; set; }
        public string FinalDateOfComments { get; set; }
        public string SendResponseBy { get; set; }
        public string Country { get; set; }
        public string EnquiryPoint { get; set; }
        public string Title { get; set; }
        public string ResponsibleAgency { get; set; }
        public string UnderArticle { get; set; }
        public string ProductsCovered { get; set; }
        public string Description { get; set; }
        public List<HSCodes> HSCodes { get; set; }
        public string MailSentToEnquiryDeskOn { get; set; }
        public List<Regulations> Regulations { get; set; }
        public List<Stakeholder> StakeHolders { get; set; }
        public List<StakeholderMail> MailSent { get; set; }
        public List<StakeholderMail> MailReceived { get; set; }
        public List<Notifi_Actions> Actions { get; set; }
        public string RemainderToTranslaterOn { get; set; }
        public string TranslationDueOn { get; set; }
        public string StakeholderResponseDueBy { get; set; }
        public List<MailSentAttachment> MailSentAttachment { get; set; }
        public List<MailReceivedAttachment> MailReceivedAttachment { get; set; }
        public List<ActionAttachment> ActionAttachment { get; set; }
        public string MeetingNotes { get; set; }
        public Nullable<Int64> MomId { get; set; }
        public string CallFor { get; set; }
        public Nullable<Int64> RowNum { get; set; }
        public Nullable<Int64> TotalRow { get; set; }
        public int RegulationFlag { get; set; }
        public int TranslationFlag { get; set; }
        public int StakholderMailFlag { get; set; }
        public int MeetingFlag { get; set; }
    }

    public class ActionAttachment
    {
        public long ActionId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
    }

    public class Notifi_Actions
    {
        public long ActionId { get; set; }
        public string Action { get; set; }
        public string RequiredBy { get; set; }
        public string TakenOn { get; set; }
        public Int32 MailId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Attachments { get; set; }
        public string ColorCode { get; set; }
        public string TooltipText { get; set; }
    }

    public class AddStakeholder
    {
        public long NotificationId { get; set; }
        public string StakeholderIds { get; set; }
    }

    public class CalculatedDate
    {
        public string FinalDateOfComment { get; set; }
        public string ObtainDocumentBy { get; set; }
        public string TranslationDueBy { get; set; }
        public string TranslationReminder { get; set; }
        public string SendResponseBy { get; set; }
        public string StakeholderResponsedueBy { get; set; }
        public string StakeholderReminder { get; set; }
    }
    public class ResponseAttachment
    {
        public string FileName { get; set; }

        [XmlIgnore]
        public string Content { get; set; }
        public string Path { get; set; }
    }

    public class EditResponseAttachment
    {
        public string FileName { get; set; }
        public string Path { get; set; }
    }
    public class MailSentAttachment
    {
        public long MailId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
    }
    public class MailReceivedAttachment
    {
        public long StakeholderResponseId { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
    }
    public class SaveNote
    {
        public int NotificationId { get; set; }
        public string MeetingNote { get; set; }
    }
}