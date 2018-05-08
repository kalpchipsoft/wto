using System;
using System.Collections.Generic;
using BusinessObjects.Masters;

namespace BusinessObjects.Notification
{
    public class Search_Notification
    {
        public long PageIndex { get; set; }
        public long PageSize { get; set; }
        public string NotificationNumber { get; set; }
        public int CountryId { get; set; }

        private DateTime _dateFrom = Convert.ToDateTime("1/1/1753 12:00:00 AM");
        private DateTime _dateTo = Convert.ToDateTime("1/1/9998 12:00:00 AM");
        public DateTime FinalDateOfComments_From
        {
            get { return _dateFrom; }
            set { _dateFrom = value; }
        }
        public DateTime FinalDateOfComments_To
        {
            get { return _dateTo; }
            set { _dateTo = value; }
        }
        public int NotificationType { get; set; }
        public int StatusId { get; set; }
        public string StatusFor { get; set; }
    }
    public class Notification : CommonResponseModel
    {
        public long ItemNumber { get; set; }
        public long NotificationId { get; set; }
        public string Title { get; set; }
        public string NotificationNumber { get; set; }
        public string NotificationDate { get; set; }
        public string FinalDateOfComments { get; set; }
        public string Country { get; set; }
        public int MailCount { get; set; }
        public int ResponseCount { get; set; }
        public int DiscussionStatus { get; set; }
        public string Actions { get; set; }
    }

    public class NotificationList 
    {
        public List<Country> CountryList { get; set; }
        public List<StatusMaster> StatusList { get; set; }
        public List<ActionMaster> ActionList { get; set; }
        public List<Notification> Notifications { get; set; }

        public long TotalCount { get; set; }
    }

    public class StatusMaster
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
    }

    public class Notifications : PagerTotalCount
    {

        public List<Notification> ItemsList { get; set; }
    }
}
