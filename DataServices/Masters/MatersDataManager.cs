using System;
using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using UtilitiesManagers;

namespace DataServices.Masters
{
    public class MatersDataManager
    {
        public DataTable GetHSCodes()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetHSCodeMaster;
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet GetTranslaters(Int64 LanguageId)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetTranslatorsList;
                sqlCommand.Parameters.AddWithValue("@TranslatorId", 0);
                sqlCommand.Parameters.AddWithValue("@LanguageId", LanguageId);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataTable GetLanguages()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetLanguageMaster;
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataTable CheckIsEmailExists(string Email, string callFor)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.CheckIsEmailExists;
                sqlCommand.Parameters.AddWithValue("@Email", Email);
                sqlCommand.Parameters.AddWithValue("@Callfor", callFor);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
    }
}
