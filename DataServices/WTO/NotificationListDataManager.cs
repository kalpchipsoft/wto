﻿using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using BusinessObjects.Notification;
using UtilitiesManagers;

namespace DataServices.WTO
{
    public class NotificationListDataManager
    {
        public DataSet GetPageLoad_NotificationsList(Search_Notification obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetNotificationList_PageLoadData;
                sqlCommand.Parameters.AddWithValue("@PageIndex", obj.PageIndex);
                sqlCommand.Parameters.AddWithValue("@PageSize", obj.PageSize);
                sqlCommand.Parameters.AddWithValue("@NotificationNumber", obj.NotificationNumber);
                sqlCommand.Parameters.AddWithValue("@CountryId", obj.CountryId);
                sqlCommand.Parameters.AddWithValue("@FinalDateOfComment_From", obj.FinalDateOfComments_From);
                sqlCommand.Parameters.AddWithValue("@FinalDateOfComment_To", obj.FinalDateOfComments_To);
                sqlCommand.Parameters.AddWithValue("@NotificationType", obj.NotificationType);
                sqlCommand.Parameters.AddWithValue("@Status", obj.StatusId);
                sqlCommand.Parameters.AddWithValue("@Action", obj.ActionId);
                sqlCommand.Parameters.AddWithValue("@ActionStatus", obj.ActionStatus);
                sqlCommand.Parameters.AddWithValue("@MeetingDate", obj.MeetingDate);
                sqlCommand.Parameters.AddWithValue("@PendingStatusFrom", obj.PendingFrom);
                sqlCommand.Parameters.AddWithValue("@DateofNotification_From", obj.DateofNotification_From);
                sqlCommand.Parameters.AddWithValue("@DateofNotification_To", obj.DateofNotification_To);
                sqlCommand.Parameters.AddWithValue("@Hscode", obj.Hscode);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet Notifications(Search_Notification obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetNotificationsList;
                sqlCommand.Parameters.AddWithValue("@PageIndex", obj.PageIndex);
                sqlCommand.Parameters.AddWithValue("@PageSize", obj.PageSize);
                sqlCommand.Parameters.AddWithValue("@NotificationNumber", obj.NotificationNumber);
                sqlCommand.Parameters.AddWithValue("@CountryId", obj.CountryId);
                sqlCommand.Parameters.AddWithValue("@FinalDateOfComment_From", obj.FinalDateOfComments_From);
                sqlCommand.Parameters.AddWithValue("@FinalDateOfComment_To", obj.FinalDateOfComments_To);
                sqlCommand.Parameters.AddWithValue("@NotificationType", obj.NotificationType);
                sqlCommand.Parameters.AddWithValue("@Status", obj.StatusId);
                sqlCommand.Parameters.AddWithValue("@Action", obj.ActionId);
                sqlCommand.Parameters.AddWithValue("@ActionStatus", obj.ActionStatus);
                sqlCommand.Parameters.AddWithValue("@MeetingDate", obj.MeetingDate);
                sqlCommand.Parameters.AddWithValue("@PendingStatusFrom", obj.PendingFrom);
                sqlCommand.Parameters.AddWithValue("@DateofNotification_From", obj.DateofNotification_From);
                sqlCommand.Parameters.AddWithValue("@DateofNotification_To", obj.DateofNotification_To);
                sqlCommand.Parameters.AddWithValue("@Hscode", obj.Hscode);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet ExportNotificationList(Search_Notification obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.ExportNotificationList;
                // sqlCommand.Parameters.AddWithValue("@PageIndex", obj.PageIndex);
                // sqlCommand.Parameters.AddWithValue("@PageSize", obj.PageSize);
                sqlCommand.Parameters.AddWithValue("@NotificationNumber", obj.NotificationNumber);
                sqlCommand.Parameters.AddWithValue("@CountryId", obj.CountryId);
                sqlCommand.Parameters.AddWithValue("@FinalDateOfComment_From", obj.FinalDateOfComments_From);
                sqlCommand.Parameters.AddWithValue("@FinalDateOfComment_To", obj.FinalDateOfComments_To);
                sqlCommand.Parameters.AddWithValue("@NotificationType", obj.NotificationType);
                sqlCommand.Parameters.AddWithValue("@Status", obj.StatusId);
                sqlCommand.Parameters.AddWithValue("@Action", obj.ActionId);
                sqlCommand.Parameters.AddWithValue("@ActionStatus", obj.ActionStatus);
                sqlCommand.Parameters.AddWithValue("@MeetingDate", obj.MeetingDate);
                sqlCommand.Parameters.AddWithValue("@PendingStatusFrom", obj.PendingFrom);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        #region "Notification Country List"
        public DataSet PageLoad_CountriesNotifications(Search_NotificationCountries obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetPageLoadCountriesNotificationList;
                sqlCommand.Parameters.AddWithValue("@PageIndex", obj.PageIndex);
                sqlCommand.Parameters.AddWithValue("@PageSize", obj.PageSize);
                sqlCommand.Parameters.AddWithValue("@CountryId", obj.CountryId);
                sqlCommand.Parameters.AddWithValue("@FromDate", obj.FromDate);
                sqlCommand.Parameters.AddWithValue("@ToDate", obj.ToDate);
                sqlCommand.Parameters.AddWithValue("@Hscode", obj.Hscode);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet CountriesNotifications(Search_NotificationCountries obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetCountriesNotificationList;
                sqlCommand.Parameters.AddWithValue("@PageIndex", obj.PageIndex);
                sqlCommand.Parameters.AddWithValue("@PageSize", obj.PageSize);
                sqlCommand.Parameters.AddWithValue("@CountryId", obj.CountryId);
                sqlCommand.Parameters.AddWithValue("@FromDate", obj.FromDate);
                sqlCommand.Parameters.AddWithValue("@ToDate", obj.ToDate);
                sqlCommand.Parameters.AddWithValue("@Hscode", obj.Hscode);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        #endregion

        #region "Stakeholder Mail sent & Response List"
        public DataSet GetStakeholderMailSentResponse(Search_StakeholderMailSentReceive obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetStakholdersMailSentResponseList;
                sqlCommand.Parameters.AddWithValue("@PageIndex", obj.PageIndex);
                sqlCommand.Parameters.AddWithValue("@PageSize", obj.PageSize);
                sqlCommand.Parameters.AddWithValue("@FromDate", obj.FromDate);
                sqlCommand.Parameters.AddWithValue("@ToDate", obj.ToDate);
                sqlCommand.Parameters.AddWithValue("@Status", obj.Status);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        #endregion

    }
}
