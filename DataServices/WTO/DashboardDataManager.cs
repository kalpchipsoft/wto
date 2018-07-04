using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using BusinessObjects.Notification;
using UtilitiesManagers;

namespace DataServices.WTO
{
    public class DashboardDataManager
    {
        public DataSet GetDashboard_PendingCounts(Dashboard obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Dashboards_PendingCounts;
                sqlCommand.Parameters.AddWithValue("@UserId", obj.UserId);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet GetNotificationGraphData(DashboardSearch obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Dashboards_NotificationGraphData;
                sqlCommand.Parameters.AddWithValue("@DateFrom", obj.DateFrom);
                sqlCommand.Parameters.AddWithValue("@DateTo", obj.DateTo);
                sqlCommand.Parameters.AddWithValue("@CallFor", obj.CallFor);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet GetHsCodeGraphData(DashboardSearch obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Dashboards_NotificationCountByHsCode;
                sqlCommand.Parameters.AddWithValue("@DateFrom", obj.DateFrom);
                sqlCommand.Parameters.AddWithValue("@DateTo", obj.DateTo);
                sqlCommand.Parameters.AddWithValue("@HSCodes", obj.HSCode);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet GetHsCodeGraphDataCountryWise(DashboardSearch obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Dashboards_NotificationCountByCountry;
                sqlCommand.Parameters.AddWithValue("@DateFrom", obj.DateFrom);
                sqlCommand.Parameters.AddWithValue("@DateTo", obj.DateTo);
                sqlCommand.Parameters.AddWithValue("@HSCodes", obj.HSCode);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet GetNotificationCountRequestResponse(DashboardSearch obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Dashboards_NotificationCountRequestResponse;
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet GetDashboardPendingCount_Action(DashboardSearch obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Dashboards_NotificationCountByAction;
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet GetNotificationCountProcessingStatus(Dashboard obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Dashboards_PendingCounts_Discussion;
                sqlCommand.Parameters.AddWithValue("@UserId", obj.UserId);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet GetNotificationGraphDataWeekly(DashboardSearch obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Dashboards_NotificationGraphDataWeekly;
                sqlCommand.Parameters.AddWithValue("@StartDate", obj.DateFrom);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet GetNotificationGraphDataMonthly(DashboardSearch obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Dashboards_NotificationGraphDataMonthly;
                sqlCommand.Parameters.AddWithValue("@StartDate", obj.DateFrom);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
    }
}
