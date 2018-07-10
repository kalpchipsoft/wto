using BusinessObjects.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BusinessObjects.MOM
{
    public class Search_MoM
    {
        public string callFor { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string NotificationNumber { get; set; }
        public string SelectedNotifications { get; set; }
        public string ExistingNotifications { get; set; }
        public string SearchText { get; set; }
    }
    public class Action
    {
        public int ActionId { get; set; }
        public string ActionName { get; set; }
        public string MailId { get; set; }
    }

    public class NotificationAction : Action
    {
        public Int64 NotificationId { get; set; }
        public string RequiredOn { get; set; }
    }

    public class Meeting
    {
        public Int64 NotificationId { get; set; }
        public string MeetingNote { get; set; }
        public string NotificationGroup { get; set; }
    }

    public class MeetingDetail : Meeting
    {
        public Int64 MoMId { get; set; }
        public int ActionId { get; set; }
    }
    public class AddMeeting
    {
        [Required]
        public string MeetingDate { get; set; }

        public List<Meeting> MeetingDetails { get; set; }

        public string MeetingDetailXML
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
                XmlSerializer serializer = new XmlSerializer(typeof(List<Meeting>), new XmlRootAttribute("Meetings"));
                serializer.Serialize(xw, MeetingDetails, Namespace);

                //Load XML to document
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sw.ToString());
                return doc.InnerXml;
            }
        }

    }
    public class Notification_Mom
    {
        public Int32 ItemNumber { get; set; }
        public Int64 NotificationId { get; set; }
        public string Title { get; set; }
        public string NotificationNumber { get; set; }
        public string Country { get; set; }
        public string MeetingNote { get; set; }
        public Boolean IsUpdate { get; set; }
        public string SendResponseBy { get; set; }
        public string FinalDateofComments { get; set; }
        public string Description { get; set; }
        public Nullable<Int64> RowNum { get; set; }
        public Nullable<Int64> TotalRow { get; set; }
        public string NotificationGroup { get; set; }
    }

    public class NotificationMOM
    {
        public List<Notification_Mom> Notification_MomList { get; set; }
        public List<GetCountry> CountryList { get; set; }
        public List<NotificationProcessDot> NotificationProcessDots { get; set; }
    }

    public class EditMeeting
    {
        public Int32? IsExistFlag { get; set; }//IsExistFlag
        public Int64 MoMId { get; set; }
        public bool IsActive { get; set; }
        public string MeetingDate { get; set; }
        public List<Action> Actions { get; set; }
        public List<Notification_Mom> Notifications { get; set; }
        public List<NotificationAction> NotificationActions { get; set; }
    }

    public class MoMs
    {
        public List<Action> Actions { get; set; }
        public List<MoM> MoMList { get; set; }
        public List<MoMAction> MoMActionList { get; set; }
    }

    public class MoM
    {
        public Int32 ItemNumber { get; set; }
        public Int64 MoMId { get; set; }
        public Int32 NotificationCount { get; set; }
        public Int32 PendingCount { get; set; }
        public string MeetingDate { get; set; }
    }


    public class MoMAction
    {
        public Int64 MoMId { get; set; }
        public int ActionId { get; set; }
        public Int64 TotalCount { get; set; }
        public Int64 PendingCount { get; set; }
    }

    public class EditAction : Action
    {
        public Int64 NotificationActionId { get; set; }
        public string RequiredOn { get; set; }
        public string UpdatedOn { get; set; }
    }

    public class EditNotificationMeeting
    {
        public string NotificationNumber { get; set; }
        public string Title { get; set; }
        //public string Status { get; set; }
        public string MeetingNote { get; set; }
        public List<EditAction> Actions { get; set; }
        public string MeetingDate { get; set; }
        public bool RetainedForNextDiscussion { get; set; }
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
    public class GetCountry
    {
        public int? CountryId { get; set; }
        public string Country { get; set; }
    }
}
