using BusinessObjects.ManageAccess;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BusinessObjects.Notification
{
    public class RelatedStakeHolders
    {
        public long ItemNumber { get; set; }
        public long StakeHolderId { get; set; }
        public string FullName { get; set; }
        public string HSCodes { get; set; }
        public int MailCount { get; set; }
        public int ResponseCount { get; set; }
        public string OrgName { get; set; }
        public string Designation { get; set; }
    }

    public class MailsSentResponses
    {
        public List<StackholderMail> MailsSent { get; set; }
        public List<StackholderMail> Responses { get; set; }
    }

    public class StackholderMail
    {
        public long StakeholderMailId { get; set; }
        public long MailId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public long StakeholderCount { get; set; }
        public string Date { get; set; }
        public string StakeholderName { get; set; }
        public string OrgName { get; set; }
    }

    public class StackholderMailDetails : StackholderMail
    {
        public string MailSentTime { get; set; }
        public List<EditAttachment> Attachments { get; set; }
        public string MailType { get; set; }
        public string MailSentBy { get; set; }
    }

    public class SendMailStakeholders : StackholderMail
    {
        public long NotificationId { get; set; }
        public string UserId { get; set; }
        public string Stakeholders { get; set; }
        public List<MailAttachment> Attachments { get; set; }
        public string StakeholderResponseDueBy { get; set; }
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
                XmlSerializer serializer = new XmlSerializer(typeof(List<MailAttachment>), new XmlRootAttribute("MailAttachments"));
                serializer.Serialize(xw, Attachments, Namespace);

                //Load XML to document
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sw.ToString());
                return doc.InnerXml;
            }
        }
    }

    public class MailAttachment
    {
        public string FileName { get; set; }

        [XmlIgnore]
        public string Content { get; set; }
        public string Path { get; set; }
    }

    public class SendMailStakeholders_Output
    {
        public StackholderMail MailDetails { get; set; }

        public List<StakeHolder> StakeHolders { get; set; }

        public List<EditAttachment> Attachments { get; set; }
        public List<ResponseAttachment> ResponseAttachments { get; set; }
        public StakeholderResponse SRId { get; set; }
    }
    public class SendMailDetailStakeholders_Output
    {
        public StackholderMail MailDetails { get; set; }

        public List<RelatedStakeHolders> StakeHolders { get; set; }
        public List<EditAttachment> MailAttachmentDetails { get; set; }
    }

    public class ResponseMailDetailsStakeholders_Output
    {
        public StackholderMail objStackholderMail;
        public List<EditAttachment> MailAttachmentDetails { get; set; }
    }

}
