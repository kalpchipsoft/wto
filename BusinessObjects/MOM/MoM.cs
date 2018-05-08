using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace BusinessObjects.MOM
{
    public class Action
    {
        public int ActionId { get; set; }
        public string ActionName { get; set; }
    }
    public class MoMDetail : Action
    {
        public Int64 NotificationId { get; set; }
        public bool IsUpdated { get; set; }
    }
    public class AddMoM
    {
        [Required]
        public Int64 MoMId { get; set; }

        [Required]
        public string MeetingDate { get; set; }

        [Required]
        public List<MoMDetail> MoMDetails { get; set; }

        public string MoMDetailXML
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
                XmlSerializer serializer = new XmlSerializer(typeof(List<MoMDetail>), new XmlRootAttribute("MoMDetails"));
                serializer.Serialize(xw, MoMDetails, Namespace);

                //Load XML to document
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(sw.ToString());
                return doc.InnerXml;
            }
        }

    }
    public class NotificationList_Mom
    {
        public Int64 MoMId { get; set; }
        public string MeetingDate { get; set; }
        public List<Action> Actions { get; set; }
        public List<Notification_Mom> NotificationList { get; set; }
        public List<MoMDetail> NotificationActions { get; set; }
    }
    public class Notification_Mom
    {
        public Int32 ItemNumber { get; set; }
        public Int64 NotificationId { get; set; }
        public string Title { get; set; }
        public string NotificationNumber { get; set; }
        public string Country { get; set; }
    }

    public class MoMs
    {
        public List<Action> Actions { get; set; }
        public List<MoM> MoMList { get; set; }
        public List<MoMAction> MoMActionList{ get; set; }
    }

    public class MoM
    {
        public Int32 ItemNumber { get; set; }
        public Int64 MoMId { get; set; }
        public Int32 NotificationCount { get; set; }
        public string MeetingDate { get; set; }
    }


    public class MoMAction
    {
        public Int64 MoMId { get; set; }
        public int ActionId { get; set; }
        public Int64 Count { get; set; }
    }

}
