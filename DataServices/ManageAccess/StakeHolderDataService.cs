using System;
using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using BusinessObjects.ManageAccess;
using UtilitiesManagers;

namespace DataServices.ManageAccess
{
    public class StakeHolderDataService
    {
        public DataSet GetStakeHoldersList()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetStakeHoldersList;
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet GetStakeHolderDetails(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetStakeHolderDetails;
                sqlCommand.Parameters.AddWithValue("@StakeHolderId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public bool AddStakeHolder(Int64 Id, AddStakeHolder obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.AddStakeHolder;
                sqlCommand.Parameters.AddWithValue("@UserId", Id);
                sqlCommand.Parameters.AddWithValue("@StakeHolderId", obj.StakeHolderId);
                sqlCommand.Parameters.AddWithValue("@StakeHolderName", obj.StakeHolderName);
                sqlCommand.Parameters.AddWithValue("@Email", obj.Email);
                sqlCommand.Parameters.AddWithValue("@IsActive", obj.Status);
                sqlCommand.Parameters.AddWithValue("@OrgName", obj.OrgName);
                sqlCommand.Parameters.AddWithValue("@HSCodes", obj.HSCodes);
                sqlCommand.Parameters.AddWithValue("@@Designation", obj.Designation);
                return Convert.ToBoolean(DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand));
            }
        }

        public bool DeleteStakeHolder(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.DeleteStakeHolder;
                sqlCommand.Parameters.AddWithValue("@StakeHolderId", Id);
                return Convert.ToBoolean(DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand));
            }
        }
    }
}
