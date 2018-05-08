using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using BusinessObjects.Notification;
using UtilitiesManagers;

namespace DataServices.WTO
{
    public class DashboardDataManager
    {
        public DataTable GetDashboard_DiscussionCounts(Dashboard obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Dashboard_PendingDiscussionCount;
                sqlCommand.Parameters.AddWithValue("@UserId", obj.UserId);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet GetDashboard_ActionsCounts(Dashboard obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Dashboard_PendingActionCount;
                sqlCommand.Parameters.AddWithValue("@UserId", obj.UserId);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
    }
}
