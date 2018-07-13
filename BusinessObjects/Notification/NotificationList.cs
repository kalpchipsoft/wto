﻿using System;
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
        public Nullable<DateTime> FinalDateOfComments_From { get; set; }
        public Nullable<DateTime> FinalDateOfComments_To { get; set; }
        public int NotificationType { get; set; }
        public int StatusId { get; set; }
        public string PendingFrom { get; set; }
        public int ActionId { get; set; }
        public string ActionStatus { get; set; }
        public Nullable<DateTime> MeetingDate { get; set; }
        public Nullable<DateTime> DateofNotification_From { get; set; }
        public Nullable<DateTime> DateofNotification_To { get; set; }
        public string Hscode { get; set; }
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
        public string Actions { get; set; }
        public string MeetingDate { get; set; }
        public Boolean IsInMeeting { get; set; }
    }

    public class NotificationList
    {
        public List<Country> CountryList { get; set; }
        public List<StatusMaster> StatusList { get; set; }
        public List<ActionMaster> ActionList { get; set; }
        public List<Notification> Notifications { get; set; }
        public List<Status> ActionStatusList { get; set; }
        public long TotalCount { get; set; }

        public List<NotificationProcessDot> NotificationProcessDots { get; set; }
        public List<NotificationActionDot> NotificationActionDots { get; set; }
    }

    public class NotificationProcessDot
    {
        public Int64 NotificationId { get; set; }
        public string ColorCode { get; set; }
        public string TooltipText { get; set; }
        public int Sequence { get; set; }
    }

    public class NotificationActionDot
    {
        public Int64 NotificationId { get; set; }
        public string ColorCode { get; set; }
        public string TooltipText { get; set; }
        public int Sequence { get; set; }
    }

    public class Status
    {
        public string StatusName { get; set; }
        public int StatusId { get; set; }
    }
    public class StatusMaster
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
    }

    public class Notifications : PagerTotalCount
    {
        public List<Notification> ItemsList { get; set; }
        public List<NotificationProcessDot> NotificationProcessDots { get; set; }
        public List<NotificationActionDot> NotificationActionDots { get; set; }
    }

    public class Search_NotificationCountries
    {
        public long PageIndex { get; set; }
        public long PageSize { get; set; }
        public string Hscode { get; set; }
        public string CountryId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }

    public class NotificationCountries : PagerTotalCount
    {
        public List<CountriesNotificationList> objNotificationCountries;
        public List<Country> CountryList { get; set; }
    }
    public class CountriesNotificationList
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string NotificationCount { get; set; }
        public string CountryId { get; set; }

    }
    public class StakeholderMailSentReceive : PagerTotalCount
    {
        public List<RelatedStakeHolders> objRelatedStakeHolders;
    }
    public class Search_StakeholderMailSentReceive
    {
        public long PageIndex { get; set; }
        public long PageSize { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
