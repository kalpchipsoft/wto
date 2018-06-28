using System;
using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using BusinessObjects.ManageAccess;
using UtilitiesManagers;

namespace DataServices.ManageAccess
{
    public class CountryDataService
    {
        public DataSet GetCountriesList()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetCountriesList;
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet GetCountryDetails(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetCountryDetails;
                sqlCommand.Parameters.AddWithValue("@CountryId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public bool AddCountry(Int64 Id, AddCountry obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.AddCountry;
                sqlCommand.Parameters.AddWithValue("@UserId", Id);
                sqlCommand.Parameters.AddWithValue("@CountryId", obj.CountryId);
                sqlCommand.Parameters.AddWithValue("@CountryName", obj.CountryName);
                sqlCommand.Parameters.AddWithValue("@IsActive", obj.Status);
                sqlCommand.Parameters.AddWithValue("@CountryCode", obj.CountryCode);
                sqlCommand.Parameters.AddWithValue("@SPSEnquiryEmailId", obj.EnquiryEmail_SPS);
                sqlCommand.Parameters.AddWithValue("@TBTEnquiryEmailId", obj.EnquiryEmail_TBT);
                return Convert.ToBoolean(DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand));
            }
        }

        public bool DeleteCountry(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.DeleteCountry;
                sqlCommand.Parameters.AddWithValue("@CountryId", Id);
                return Convert.ToBoolean(DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand));
            }
        }
        public DataTable CheckDuplicateCountryData(int Id, string Callfor, string text)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetDupCountryCode;
                sqlCommand.Parameters.AddWithValue("@Id", Id);
                sqlCommand.Parameters.AddWithValue("@Callfor", Callfor);
                sqlCommand.Parameters.AddWithValue("@text", text);
                return DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

    }
}
