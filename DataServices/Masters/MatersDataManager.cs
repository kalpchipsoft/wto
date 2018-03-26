using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using BusinessObjects;
using BusinessObjects.Notification;
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
    }
}
