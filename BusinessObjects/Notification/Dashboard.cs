using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Notification
{
    public class Dashboard
    {
        [Required(ErrorMessage = "UserId missing in session.")]
        [Range(1, Int64.MaxValue)]
        public Int64 UserId { get; set; }
    }

    public class Count_Discussion: StatusMaster
    {
        public Int64 PendingFromUser { get; set; }
        public Int64 TotalPending { get; set; }
        public string CssColor { get; set; }
        public string Icon { get; set; }
    }

    public class Count_Action: ActionMaster
    {
        public int Total { get; set; }
        public int OverDue { get; set; }
        public string CssColor { get; set; }
    }

    public class Dashboard_PendingCounts
    {
        public List<Count_Discussion> PendingDiscussions { get; set; }

        public List<Count_Action> PendingActions { get; set; }
    }
    public class DashboardSearch
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string HSCode { get; set; }
        public string CallFor { get; set; }

    }

    public class HSCodeGraphData
    {
        public string Text { get; set; }
        public Int32 NotificationCount { get; set; }
        public string HSCode { get; set; }
    }
    public class HSCodeCountryData
    {
        public Int32 NotificationCount { get; set; }
        public string Country { get; set; }
        public string ColorCode { get; set; }
        public string CountryCode { get; set; } 
    }
    public class HSCodeCountry
    {
        public List<HSCodeCountryData> objHSCodeCountryData;
        public string HSCode;
        public string CountryCount;
    }
    public class NotificationGraphData
    {
        public Int32 NotificationCount { get; set; }
        public string MonthId { get; set; }
        public string MonthName { get; set; }
        public Int32 InProcessCount { get; set; }
        public Int32 UnderActionCount { get; set; }
        public Int32 ClosedCount { get; set; }
        public Int32 LapsedCount { get; set; }
    }
    public class NotificationRequestResponse
    {
        public List<NotificationTextCount> objRequestForFullText {get; set; }
        public List<NotificationTextCount> objRequestForTranslation { get; set; }
        public List<NotificationTextCount> objRequestForStakeholderResponse { get; set; }
        public string ClosedCount { get; set; }
        public string LapsedCount { get; set; }
    }
    public class NotificationTextCount
    {
        public string Text { get; set; }
        public string Count { get; set; }
    }
    public class NotificationPendingCount_Action
    {
        public Int32 ActionId { get; set; }
        public string Action { get; set; }
        public Int64 Total { get; set; }
        public Int64 Overdue { get; set; }
        public string ColorCode { get; set; }
        public int Completed { get; set; }
        public int Pending { get; set; }
    }
    public class NotificationProcessingStatus
    {
        public List<NotificationTextCount> objPendingFullText { get; set; }
        public List<NotificationTextCount> objPendingTranslation { get; set; }
        public List<NotificationTextCount> objToSendtoStakeholder { get; set; }
        public List<NotificationTextCount> objToDiscuss { get; set; }
    }
}
