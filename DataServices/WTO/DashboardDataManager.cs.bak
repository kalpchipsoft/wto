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
    }
}
