using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using BusinessObjects.Notification;
using UtilitiesManagers;

namespace DataServices.WTO
{
    public class AddNotificationDataManager
    {
        public DataSet InsertUpdate_Notification(AddNotification obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_AddUpdate;
                sqlCommand.Parameters.AddWithValue("@UserId", obj.UserId);
                sqlCommand.Parameters.AddWithValue("@NotificationId", obj.NotificationId);
                sqlCommand.Parameters.AddWithValue("@Noti_Type", obj.Noti_Type);
                sqlCommand.Parameters.AddWithValue("@Noti_Number", obj.Noti_Number);                
                sqlCommand.Parameters.AddWithValue("@Noti_Status", obj.Noti_Status);                
                sqlCommand.Parameters.AddWithValue("@Noti_Date", obj.Noti_Date);
                sqlCommand.Parameters.AddWithValue("@DateOfComments", obj.FinalDateOfComments);
                sqlCommand.Parameters.AddWithValue("@SendResponseBy", obj.SendResponseBy);
                sqlCommand.Parameters.AddWithValue("@Noti_Country", obj.Noti_Country);
                sqlCommand.Parameters.AddWithValue("@Noti_Title", obj.Title);
                sqlCommand.Parameters.AddWithValue("@ResponsibleAgency", obj.ResponsibleAgency);
                sqlCommand.Parameters.AddWithValue("@Noti_UnderArticle", obj.Noti_UnderArticle);
                sqlCommand.Parameters.AddWithValue("@ProductsCovered", obj.ProductsCovered);
                sqlCommand.Parameters.AddWithValue("@HSCodes", obj.HSCodes);
                sqlCommand.Parameters.AddWithValue("@Description", obj.Description);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet Edit_Notification(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Notification_Edit;
                sqlCommand.Parameters.AddWithValue("@NotificationId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
    }
}
