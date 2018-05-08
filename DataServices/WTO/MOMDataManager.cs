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
        public DataSet GetNotificationListForMom(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetNotificationListForMom;
                sqlCommand.Parameters.AddWithValue("@MoMId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public Int32 InsertUpdateMomData(Int64 UserId, AddMoM obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.InsertUpdateMom;
                sqlCommand.Parameters.AddWithValue("@MoMId", obj.MoMId);
                sqlCommand.Parameters.AddWithValue("@MeetingDate", obj.MeetingDate);
                sqlCommand.Parameters.AddWithValue("@MoMdetails", obj.MoMDetailXML);
                sqlCommand.Parameters.AddWithValue("@CreatedBy", UserId);
                return DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand);
            }

        }
        public DataSet GetMOMListData()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetMOMListData;
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
    }
}
