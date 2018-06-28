using System;
using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using BusinessObjects.ManageAccess;
using UtilitiesManagers;


namespace DataServices.ManageAccess
{
   public class RegulatoryBodiesDataService
    {
        public DataSet GetRegulatoryBodiesList()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetRegulatoryBodiesList;
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet GetRegulatoryBodiesDetails(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetRegulatoryBodiesDetails;
                sqlCommand.Parameters.AddWithValue("@RegulatoryBodyId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public bool AddRegulatoryBodies(Int64 Id, AddRegulatoryBodies obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.AddRegulatoryBodies;
                sqlCommand.Parameters.AddWithValue("@UserId", Id);
                sqlCommand.Parameters.AddWithValue("@RegulatoryBodyId", obj.RegulatoryBodyId);
                sqlCommand.Parameters.AddWithValue("@Name", obj.Name);
                sqlCommand.Parameters.AddWithValue("@IsActive", obj.Status);
                sqlCommand.Parameters.AddWithValue("@Emailid", obj.Emailid);
                sqlCommand.Parameters.AddWithValue("@Contact", obj.Contact);
                sqlCommand.Parameters.AddWithValue("@Address", obj.Address);
                return Convert.ToBoolean(DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand));
            }
        }

        public bool DeleteRegulatoryBodies(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.DeleteRegulatoryBodies;
                sqlCommand.Parameters.AddWithValue("@RegulatoryBodyId", Id);
                return Convert.ToBoolean(DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand));
            }
        } 
    }
}
