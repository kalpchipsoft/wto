using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BusinessObjects.Notification
{
    public class ActionMaster
    {
        public string Action { get; set; }
        public int ActionId { get; set; }
    }
    public class NotificationAction : ActionMaster
    {
        public Int64 NotificationActionId { get; set; }
        public string RequiredOn { get; set; }
    }
    public class NotificationActionDetail : NotificationAction
    {
        public string EnteredOn { get; set; }
        public string UpdatedOn { get; set; }
        public Int64 MailId { get; set; }
        public string MailTo { get; set; }
        public Notification_Template MailDetails { get; set; }
        public List<EditAttachment> Attachments { get; set; }

        public Int64 ResponseId { get; set; }
    }

    public class NotificationActions
    {
        public Int64 NotificationId { get; set; }
        public Int64 MeetingId { get; set; }
        public string NotificationNumber { get; set; }
        public string NotificationTitle { get; set; }
        public string MeetingDate { get; set; }
        public bool IsUpdate { get; set; }
        public string MeetingNotes { get; set; }
        public bool RetainedForNextDiscussion { get; set; }
        public List<NotificationActionDetail> Actions { get; set; }
    }

    public class AddNotificationAction
    {
        [Required]
        public DateTime Meetingdate { get; set; }

        [Required]
        public Int64 NotificationId { get; set; }

        [Required]
        public List<NotificationAction> Actions { get; set; }

        [Required]
        public string MeetingNote { get; set; }

        public string NotificationGroup { get; set; }
        public string ActionXML
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
                XmlSerializer serializer = new XmlSerializer(typeof(List<NotificationAction>), new XmlRootAttribute("NotificationActions"));
                serializer.Serialize(xw, Actions, Namespace);

                //Load XML to document
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sw.ToString());
                return doc.InnerXml;
            }
        }
    }

    //public class EditNotificationAction
    //{
    //    [Required]
    //    public Int64 NotificationId { get; set; }

    //    [Required]
    //    public Int64 NotificationActionId { get; set; }
    //}

    public class AddNotificationAction_Output
    {
        public string MailTo { get; set; }
        public Int64 NotificationId { get; set; }
        public Int64 NotificationActionId { get; set; }
        public int ActionId { get; set; }
        public string NotificationNumber { get; set; }
        public string DateofNotification { get; set; }
        public int ResponseCount { get; set; }
        public int MailCount { get; set; }
        public Notification_Template MailDetails { get; set; }
    }

    public class ActionMailDetails
    {
        public int UserId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string MailTo { get; set; }
        public List<Attachement_Action> Attachments { get; set; }
        //public string AttachmentsXML { get; set; }

        public string AttachmentsXML
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
                XmlSerializer serializer = new XmlSerializer(typeof(List<Attachement_Action>), new XmlRootAttribute("Attachments"));
                serializer.Serialize(xw, Attachments, Namespace);

                //Load XML to document
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sw.ToString());
                return doc.InnerXml;
            }
        }
        public string _Attachments
        {
            get
            {
                if (Attachments == null)
                    return "";
                else
                {
                    return string.Join("|", Attachments.Select(x => x.FileName).ToArray());
                }
            }
        }
    }

    public class Attachement_Action : Attachment
    {
        public string Path { get; set; }
    }

    public class SendActionMail_Output
    {
        public string ReplyTo { get; set; }
        public string MailTo { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string DisplayName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<EditAttachment> Attachments { get; set; }
    }
}
