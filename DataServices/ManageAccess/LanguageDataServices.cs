using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitiesManagers;
using BusinessObjects.ManageAccess;

namespace DataServices.ManageAccess
{
    public class LanguageDataServices
    {
        public DataSet GetLanguageList()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetLanguageList;
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public DataSet GetLanguageDetails(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetLanguageDetails;
                sqlCommand.Parameters.AddWithValue("@LanguageId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
        public bool AddLanguage(Int64 Id, LanguageDetails obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.AddLanguage;
                sqlCommand.Parameters.AddWithValue("@UserId", Id);
                sqlCommand.Parameters.AddWithValue("@LanguageId", obj.LanguageId);
                sqlCommand.Parameters.AddWithValue("@Language", obj.Language);
                sqlCommand.Parameters.AddWithValue("@IsActive", obj.Status);
                return Convert.ToBoolean(DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand));
            }
        }
        public bool DeleteLanguage(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.DeleteLanguage;
                sqlCommand.Parameters.AddWithValue("@LanguageId", Id);
                return Convert.ToBoolean(DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand));
            }
        }
        public DataTable DuplicateLanguage(Int64 Id,string text)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.DuplicateLanguage;
                sqlCommand.Parameters.AddWithValue("@LanguageId", Id);
                sqlCommand.Parameters.AddWithValue("@Language", text);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
    }
}
