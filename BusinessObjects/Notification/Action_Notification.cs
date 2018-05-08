using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public string Remarks { get; set; }
        public string RequiredOn { get; set; }
        public string EnteredOn { get; set; }
        public EditAttachment Attachment { get; set; }
        public string CompletedOn { get; set; }
        public string UpdatedOn { get; set; }
    }

    public class NotificationActions
    {
        public Int64 NotificationId { get; set; }
        public string NotificationNumber { get; set; }
        public string NotificationTitle { get; set; }
        public string MeetingDate { get; set; }
        public List<NotificationAction> Actions { get; set; }
    }

    public class AddNotificationAction
    {
        public Int64 NotificationActionId { get; set; }
        public Nullable<DateTime> Meetingdate { get; set; }
        [Required]
        public Int64 NotificationId { get; set; }
        [Required]
        public int ActionId { get; set; }
        [Required]
        public string RequiredOn { get; set; }
        [Required]
        public Attachment Attachment { get; set; }
        public string Remarks { get; set; }

        public string Status { get; set; }
    }

    public class AddNotificationAction_Output
    {
        public Int64 NotificationId { get; set; }
        public int ActionId { get; set; }
    }
}
