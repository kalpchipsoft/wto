using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using BusinessObjects.Masters;
using DataServices;
using UtilitiesManagers;

namespace BusinessServices
{
    public class LoginBusinessService
    {
        public LoginResult ValidateUser(LoginModel model)
        {
            LoginResult objR = new LoginResult();
            LoginDataManger objLDM = new LoginDataManger();
            DataTable dt = objLDM.ValidateUser(model.UserName);
            if (dt != null && dt.Rows.Count > 0)
            {
                CommonHelper objCH = new CommonHelper();
                if (model.Password == objCH.DecryptData(Convert.ToString(dt.Rows[0]["Pwd"])))
                {
                    objR.UserId = Convert.ToInt64(dt.Rows[0]["UserId"]);

                    objR.StatusType = StatusType.SUCCESS;
                    objR.MessageType = MessageType.NO_MESSAGE;
                }
                else
                {
                    objR.StatusType = StatusType.FAILURE;
                    objR.MessageType = MessageType.WRONG_PASSWORD;
                }
            }
            else
            {
                objR.StatusType = StatusType.FAILURE;
                objR.MessageType = MessageType.WRONG_USERNAME;
            }
            return objR;
        }

        public UserInfo GetUserDetails(Int64 Id)
        {
            UserInfo objUI = new UserInfo();
            LoginDataManger objLDM = new LoginDataManger();
            DataTable dt = objLDM.GetUserDetails(Id);
            if (dt != null && dt.Rows.Count > 0)
            {
                objUI.UserId = Convert.ToInt64(dt.Rows[0]["UserId"]);
                objUI.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                objUI.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                objUI.ImageName = Convert.ToString(dt.Rows[0]["Image"]);
            }
            else
            {
                objUI.StatusType = StatusType.FAILURE;
                objUI.MessageType = MessageType.WRONG_USERID;
            }
            return objUI;
        }
    }
}