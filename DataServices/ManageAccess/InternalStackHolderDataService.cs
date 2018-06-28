using System;
using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using BusinessObjects.ManageAccess;
using UtilitiesManagers;


namespace DataServices.ManageAccess
{
   public  class InternalStackHolderDataService
    {
        public DataSet GetInternalStackHoldersList()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetInternalStackHoldersList;
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet GetInternalStackHoldersDetails(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetInternalStackHolderDetails;
                sqlCommand.Parameters.AddWithValue("@InternalStakeholderId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public bool AddInternalStackHolders(Int64 Id, AddInternalStackHolder obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.AddInternalStackHolder;
                sqlCommand.Parameters.AddWithValue("@UserId", Id);
                sqlCommand.Parameters.AddWithValue("@InternalStakeholderId", obj.InternalStakeholdersId);
                sqlCommand.Parameters.AddWithValue("@Name", obj.Name);
                sqlCommand.Parameters.AddWithValue("@IsActive", obj.Status);
                sqlCommand.Parameters.AddWithValue("@Emailid", obj.Emailid);
                sqlCommand.Parameters.AddWithValue("@OrgName", obj.OrgName);
                sqlCommand.Parameters.AddWithValue("@Designation", obj.Designation);
                return Convert.ToBoolean(DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand));
            }
        }

        public bool DeleteInternalStackHolders(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.DeleteInternalStackHolder;
                sqlCommand.Parameters.AddWithValue("@InternalStackHolderId", Id);
                return Convert.ToBoolean(DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand));
            }
        }
    }
}
