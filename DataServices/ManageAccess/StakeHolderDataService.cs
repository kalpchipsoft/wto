using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool AddStakeHolder(StakeHolderInfo obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.AddStakeHolder;
                sqlCommand.Parameters.AddWithValue("@StakeHolderId", obj.StakeHolderId);
                sqlCommand.Parameters.AddWithValue("@FirstName", obj.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", obj.LastName);
                sqlCommand.Parameters.AddWithValue("@Email", obj.Email);
                sqlCommand.Parameters.AddWithValue("@Mobile", obj.Mobile);
                sqlCommand.Parameters.AddWithValue("@IsActive", obj.Status);
                sqlCommand.Parameters.AddWithValue("@OrgName", obj.OrgName);
                sqlCommand.Parameters.AddWithValue("@Address", obj.Address);
                sqlCommand.Parameters.AddWithValue("@City", obj.City);
                sqlCommand.Parameters.AddWithValue("@State", obj.State);
                sqlCommand.Parameters.AddWithValue("@PIN", obj.PIN);
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
