using BusinessObjects;
using BusinessObjects.Translator;
using System.Data;
using System.Data.SqlClient;
using UtilitiesManagers;

namespace DataServices
{
   public class GlobalErrorDataServices
    {

        public int SaveError(string Subject, string excepMsg )
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.SaveGlobalErrorDetail;
                sqlCommand.Parameters.AddWithValue("@Subject", Subject);
                sqlCommand.Parameters.AddWithValue("@excepMsg", excepMsg);
                return DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
    }
}
