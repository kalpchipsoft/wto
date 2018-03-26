using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Notification
{
    public class GetNotificationList
    {
        public long PageIndex { get; set; }
        public long PageSize { get; set; }
        public string Noti_Number { get; set; }
        public long Noti_Country { get; set; }

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
        public int Noti_Type { get; set; }
        public int Noti_Status { get; set; }
    }
    public class NotificationList : CommonResponseModel
    {
        public long NotificationId { get; set; }
        public long ItemNumber { get; set; }
        public string Title { get; set; }
        public string Noti_Number { get; set; }
        public string Noti_Date { get; set; }
        public string FinalDateOfComments { get; set; }
        public string Noti_Country { get; set; }        
    }

    public class NotificationList_Masters
    {        
        public List<Country> CountryList { get; set; }
        public List<NotificationStatusList> StatusList { get; set; }
        public string CallFrom { get; set; }
    }

    public class NotificationStatusList
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }

    public class Notifications : PagerTotalCount
    {
        public List<NotificationList> ItemsList { get; set; }
    }
}
