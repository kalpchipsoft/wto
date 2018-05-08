using BusinessObjects;
using BusinessObjects.Translator;
using System.Data;
using System.Data.SqlClient;
using UtilitiesManagers;

namespace DataServices.Translator
{
    public class TranslatorDataService
    {
        public DataTable ValidateTranslator(string Username,long Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Validate_Translator;
                sqlCommand.Parameters.AddWithValue("@Email", Username);
                sqlCommand.Parameters.AddWithValue("@TranslatorId", Id);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public int LoginTranslator(long Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.Login_Translator;
                sqlCommand.Parameters.AddWithValue("@TranslatorId", Id);
                return DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public int UpdatePassword(long Id,string Password)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.UpdatePassword_Translator;
                sqlCommand.Parameters.AddWithValue("@TranslatorId", Id);
                sqlCommand.Parameters.AddWithValue("@Password", Password);
                return DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet GetDocumentList(SearchDocument obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetDocumentList_Translator;
                sqlCommand.Parameters.AddWithValue("@TranslatorId", obj.TranslatorId);
                sqlCommand.Parameters.AddWithValue("@NotificationNumber", obj.NotificationNumber);
                sqlCommand.Parameters.AddWithValue("@ReceivedOn", obj.ReceivedOn);
                sqlCommand.Parameters.AddWithValue("@TranslationDueBy", obj.SubmissionDueOn);
                sqlCommand.Parameters.AddWithValue("@Status", obj.Status);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public int SaveTranslatedDocument(long Id, UploadDocument obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.UploadTranslatedDocument_Translator;
                sqlCommand.Parameters.AddWithValue("@TranslatorId", Id);
                sqlCommand.Parameters.AddWithValue("@NotificationId", obj.NotificationId);
                sqlCommand.Parameters.AddWithValue("@TranslatedDocumentName", obj.DisplayName);
                sqlCommand.Parameters.AddWithValue("@TranslatedDocument", obj.Document.FileName);
                return DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
    }
}
