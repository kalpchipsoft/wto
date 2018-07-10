using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using BusinessObjects.Translator;
using System.Data;
using System.Data.SqlClient;
using UtilitiesManagers;
using DataServices;

namespace BusinessService
{
   public class GlobalErrorBussinessServices
    {

        public int GlobalError(string excepMsg, string source)
        {
            GlobalErrorDataServices objGDS = new GlobalErrorDataServices();
            return (objGDS.SaveError(excepMsg, source));
            //UserInfo objUI = new UserInfo();
            //LoginDataManger objLDM = new LoginDataManger();
            //DataTable dt = objLDM.GetUserDetails(Id);
            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    objUI.UserId = Convert.ToInt64(dt.Rows[0]["UserId"]);
            //    objUI.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
            //    objUI.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
            //    objUI.ImageName = Convert.ToString(dt.Rows[0]["Image"]);
            //}
            //else
            //{
            //    objUI.StatusType = StatusType.FAILURE;
            //    objUI.MessageType = MessageType.WRONG_USERID;
            //}

        }
    }
}
