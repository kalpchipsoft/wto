using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using BusinessObjects.Notification;
using UtilitiesManagers;

namespace DataServices.WTO
{
    public class NotificationListDataManager
    {
        public DataSet Masters()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetNotificationListMaters;
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet Notifications(GetNotificationList obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetNotificationsList;
                sqlCommand.Parameters.AddWithValue("@PageIndex", obj.PageIndex);
                sqlCommand.Parameters.AddWithValue("@PageSize", obj.PageSize);
                sqlCommand.Parameters.AddWithValue("@Noti_Number", obj.Noti_Number);
                sqlCommand.Parameters.AddWithValue("@Noti_Country", obj.Noti_Country);
                sqlCommand.Parameters.AddWithValue("@Noti_FinalDate_From", obj.FinalDateOfComments_From);
                sqlCommand.Parameters.AddWithValue("@Noti_FinalDate_To", obj.FinalDateOfComments_To);
                sqlCommand.Parameters.AddWithValue("@Noti_Type", obj.Noti_Type);
                sqlCommand.Parameters.AddWithValue("@Noti_Stage", obj.Noti_Status);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
    }
}
