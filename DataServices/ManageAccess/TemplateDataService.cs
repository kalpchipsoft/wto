using BusinessObjects;
using BusinessObjects.ManageAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitiesManagers;

namespace DataServices.ManageAccess
{
    public class TemplateDataService
    {
        public DataSet GetTemplateList()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetTemplatesList;
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet GetTemplateDetails(Int32 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetTemplateDetails;
                sqlCommand.Parameters.AddWithValue("@TemplateId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataTable InsertUpdateTemplate(Int64 Id, AddTemplate obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.InsertUpdateTemplate;
                sqlCommand.Parameters.AddWithValue("@UserId", Id);
                sqlCommand.Parameters.AddWithValue("@TemplateId", obj.TemplateId);
                sqlCommand.Parameters.AddWithValue("@TemplateType", obj.TemplateType);
                sqlCommand.Parameters.AddWithValue("@TemplateFor", obj.TemplateFor);
                sqlCommand.Parameters.AddWithValue("@Subject", obj.Subject);
                sqlCommand.Parameters.AddWithValue("@Body", obj.Body);
                sqlCommand.Parameters.AddWithValue("@IsActive", obj.TemplateStatus);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public bool DeleteTemplate(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.DeleteTemplate;
                sqlCommand.Parameters.AddWithValue("@TemplateId", Id);
                return Convert.ToBoolean(DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand));
            }
        }

        public DataTable TemplateFields()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.MailFields;
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }
    }
}
