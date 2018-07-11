using System;
using System.Data;
using BusinessObjects.MOM;
using System.Data.SqlClient;
using BusinessObjects;
using UtilitiesManagers;

namespace DataServices.WTO
{
    public class MOMDataManager
    {
        public DataSet GetNotificationListForMom(Search_MoM obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetNotificationListForMom;
                sqlCommand.Parameters.AddWithValue("@CallFor", obj.callFor);
                sqlCommand.Parameters.AddWithValue("@CountryId", obj.CountryId);
                sqlCommand.Parameters.AddWithValue("@NotificationNumber", obj.NotificationNumber);
                sqlCommand.Parameters.AddWithValue("@SelectedNotifications", obj.SelectedNotifications);
                sqlCommand.Parameters.AddWithValue("@ExistingNotifications", obj.ExistingNotifications);
                sqlCommand.Parameters.AddWithValue("@SearchText", obj.SearchText);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public Int32 InsertMeeting(Int64 UserId, AddMeeting obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.InsertMeeting;
                sqlCommand.Parameters.AddWithValue("@MeetingDate", obj.MeetingDate);
                sqlCommand.Parameters.AddWithValue("@MoMdetails", obj.MeetingDetailXML);
                sqlCommand.Parameters.AddWithValue("@CreatedBy", UserId);
                return DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand);
            }

        }
        public DataSet GetMOMListData()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetMeetingList;
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet EditMeeting(Nullable<Int64> Id, Search_MoM obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.EditMeeting;
                sqlCommand.Parameters.AddWithValue("@MoMId", Id);
                sqlCommand.Parameters.AddWithValue("@CallFor", obj.callFor);
                sqlCommand.Parameters.AddWithValue("@SearchText", obj.SearchText);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet EditActions(Int64 Id,Int64 MeetingId)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_Actions;
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                sqlCommand.Parameters.AddWithValue("@MeetingId", MeetingId);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet InsertRemoveActions(Int64 UserId, AddNotificationAction obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.AddUpdateNotificationAction;
                sqlCommand.Parameters.AddWithValue("@UserId", UserId);
                sqlCommand.Parameters.AddWithValue("@NotificationId", obj.NotificationId);
                sqlCommand.Parameters.AddWithValue("@MeetingDate", obj.Meetingdate);
                sqlCommand.Parameters.AddWithValue("@NotificationActions", obj.ActionXML);
                sqlCommand.Parameters.AddWithValue("@MeetingNote", obj.MeetingNote);
                sqlCommand.Parameters.AddWithValue("@NotificationGroup", obj.NotificationGroup);
                sqlCommand.Parameters.AddWithValue("@MeetingId", obj.MeetingId);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }

        }
        public DataSet UpdateMeetingDate(Int64? Id, string MeetingDate)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.UpdateMeetingDate;
                sqlCommand.Parameters.AddWithValue("@MoMId", Id);
                sqlCommand.Parameters.AddWithValue("@MeetingDate", MeetingDate);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public bool EndMeeting(Int64? Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.EndMeeting;
                sqlCommand.Parameters.AddWithValue("@MomId", Id);
                int i = DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand);
                bool result = i > 0 ? true : false;
                return result;
            }
        }
        public DataTable ValidateMeetingDate(string date, Nullable<Int64> MoMId)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.CheckIfOpenMeetingExists;
                sqlCommand.Parameters.AddWithValue("@date", date);
                sqlCommand.Parameters.AddWithValue("@MoMId", MoMId);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
    }
}
