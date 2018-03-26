using System;
using System.Data;
using System.Data.SqlClient;

namespace DataServices
{
    public sealed class DAL
    {
        //To prevent from to prevent instances from being created with "new DAL()"
        private DAL()
        {

        }

        public static DataSet GetDataSet(string connectionString, SqlCommand sqlCommand)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandTimeout = 300;
                    using (DataSet ds = new DataSet())
                    {
                        using (SqlDataAdapter sqlAdopter = new SqlDataAdapter(sqlCommand))
                        {
                            sqlAdopter.Fill(ds);
                        }
                        return ds;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlCommand != null)
                    sqlCommand.Dispose();
            }
        }

        public static DataTable GetDataTable(string connectionString, SqlCommand sqlCommand)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandTimeout = 300;
                    using (DataTable dt = new DataTable())
                    {
                        using (SqlDataAdapter sqlAdopter = new SqlDataAdapter(sqlCommand))
                        {
                            sqlAdopter.Fill(dt);
                        }
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlCommand != null)
                    sqlCommand.Dispose();
            }
        }

        public static object ExecuteScalar(string connectionString, SqlCommand sqlCommand)
        {
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandTimeout = 300;
                    sqlConnection.Open();
                    return sqlCommand.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlCommand != null)
                    sqlCommand.Dispose();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }

        public static int ExecuteNonQuery(string connectionString, SqlCommand sqlCommand)
        {
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(connectionString))
                {
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandTimeout = 300;
                    sqlConnection.Open();
                    return sqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlCommand != null)
                    sqlCommand.Dispose();
                if (sqlConnection != null)
                    sqlConnection.Close();
            }
        }
    }
}
