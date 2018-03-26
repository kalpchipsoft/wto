using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using UtilitiesManagers;

namespace DataServices
{
    public class LoginDataManger
    {
        public DataTable ValidateUser(string username)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Login_Validate;
                sqlCommand.Parameters.AddWithValue("@Username", username);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataTable GetUserDetails(long UserId)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Login_Validate;
                sqlCommand.Parameters.AddWithValue("@UserId", UserId);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
    }
}
