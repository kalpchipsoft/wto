using System;
using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using BusinessObjects.ManageAccess;
using UtilitiesManagers;

namespace DataServices.ManageAccess
{
    public class UserDataService
    {
        public DataSet GetUsersList()
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetUsersList;
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public DataSet GetUserDetails(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.GetUsersList;
                sqlCommand.Parameters.AddWithValue("@UserId", Id);
                return DAL.GetDataSet(ConfigurationHelper.connectionString, sqlCommand);
            }
        }

        public int AddUser(Int64 Id, AddUser obj)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.AddUser;
                sqlCommand.Parameters.AddWithValue("@EmployeeId", Id);
                sqlCommand.Parameters.AddWithValue("@UserId", obj.UserId);
                sqlCommand.Parameters.AddWithValue("@FirstName", obj.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", obj.LastName);
                sqlCommand.Parameters.AddWithValue("@Password", obj.Password);
                sqlCommand.Parameters.AddWithValue("@Email", obj.Email);
                sqlCommand.Parameters.AddWithValue("@Mobile", obj.Mobile);
                sqlCommand.Parameters.AddWithValue("@IsActive", obj.Status);
                sqlCommand.Parameters.AddWithValue("@RoleId", obj.RoleId);

                if (obj.UserImage != null)
                    sqlCommand.Parameters.AddWithValue("@Image", obj.UserImage.FileName);
                return Convert.ToInt32(DAL.GetDataTable(ConfigurationHelper.connectionString, sqlCommand).Rows[0][0]);
            }
        }

        public bool DeleteUser(Int64 Id)
        {
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = Procedures.DeleteUser;
                sqlCommand.Parameters.AddWithValue("@UserId", Id);
                return Convert.ToBoolean(DAL.ExecuteNonQuery(ConfigurationHelper.connectionString, sqlCommand));
            }
        }
    }
}
