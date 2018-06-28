using BusinessObjects.ManageAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BusinessObjects.Notification
{
    public class SendToTranslater
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public int NotificationId { get; set; }

        [Required]
        public Notification_Template MailDetails { get; set; }
        public int TranslaterId { get; set; }
        public DateTime TranslationDueOn { get; set; }
        public DateTime TranslationReminderOn { get; set; }
        public List<TranslatorMailAttachment> Attachments { get; set; }
        public string AttachmentXML
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
                XmlSerializer serializer = new XmlSerializer(typeof(List<TranslatorMailAttachment>), new XmlRootAttribute("TranslatorMailAttachments"));
                serializer.Serialize(xw, Attachments, Namespace);

                //Load XML to document
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sw.ToString());
                return doc.InnerXml;
            }
        }
    }
    public class TranslatorMailAttachment
    {
        public Int64 DocumentId { get; set; }
        public string DisplayName { get; set; }
        public string FileName { get; set; }

        [XmlIgnore]
        public string Content { get; set; }

        [XmlIgnore]
        public string Path { get; set; }
        public int LanguageId { get; set; }
        public int TranslaterId { get; set; }
    }
    public class SendToTranslater_Output : TranslatorInfo
    {
        public List<EditAttachment_Doc> Attachments { get; set; }
        public string CreatedBy { get; set; }
        public string CreatorEmailId { get; set; }
        public string TranslaterEmailId { get; set; }
        public string Language { get; set; }
        public string TranslationDueOn { get; set; }
    }

    public class EditAttachment_Doc : EditAttachment
    {
        public Int64 DocumentId { get; set; }
        public string SentToTranslatorOn { get; set; }
    }

    public class SendMailToEnquiryDesk
    {
        [Required]
        public string Role { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public string EnquiryEmailId { get; set; }
        [Required]
        public DateTime ObtainDocBy { get; set; }
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
