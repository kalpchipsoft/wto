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
    public class TranslatorDataService
    {
        public DataSet GetTranslatorsList()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetTranslatorsList;
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet GetTranslatorDetails(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetTranslatorDetails;
                sqlCommand.Parameters.AddWithValue("@TranslatorId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public bool AddTranslator(TranslatorInfo obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.AddTranslator;
                sqlCommand.Parameters.AddWithValue("@TranslatorId", obj.TranslatorId);
                sqlCommand.Parameters.AddWithValue("@FirstName", obj.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", obj.LastName);
                sqlCommand.Parameters.AddWithValue("@Email", obj.Email);
                sqlCommand.Parameters.AddWithValue("@Mobile", obj.Mobile);
                sqlCommand.Parameters.AddWithValue("@IsActive", obj.Status);
                return Convert.ToBoolean(DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand));
            }
        }

        public bool DeleteTranslator(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.DeleteTranslator;
                sqlCommand.Parameters.AddWithValue("@TranslatorId", Id);
                return Convert.ToBoolean(DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand));
            }
        }
    }
}
