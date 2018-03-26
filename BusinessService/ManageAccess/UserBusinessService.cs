using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BusinessObjects.ManageAccess;
using DataServices.ManageAccess;
using UtilitiesManagers;

namespace BusinessService.ManageAccess
{
    public class UserBusinessService
    {
        UserDataService objUDS = new UserDataService();
        public PageLoad_UserList UsersList()
        {
            CommonHelper objCH = new CommonHelper();
            PageLoad_UserList obj = new PageLoad_UserList();
            DataSet ds = objUDS.GetUsersList();
            if (ds != null && ds.Tables.Count > 0)
            {                
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<UserInfo> objUserList = new List<UserInfo>();
                    UserInfo objUI = new UserInfo();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        objUI = new UserInfo();
                        objUI.ItemNumber = Convert.ToInt64(dr["ItemNumber"]);
                        objUI.UserId = Convert.ToInt64(dr["UserId"]);
                        objUI.FirstName = Convert.ToString(dr["FirstName"]);
                        objUI.LastName = Convert.ToString(dr["LastName"]);
                        objUI.Password = Convert.ToString(objCH.DecryptData(dr["Password"]));
                        objUI.Email = Convert.ToString(dr["Email"]);
                        objUI.Mobile = Convert.ToString(dr["Mobile"]);
                        objUI.Status = Convert.ToInt16(dr["Active"]);
                        objUserList.Add(objUI);
                    }
                    obj.UserList = objUserList;
                }
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.TotalCount = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["TotalCount"]);
                }
            }
            return obj;
        }

        public UserInfo UserDetails(Int64 Id)
        {
            UserInfo obj = new UserInfo();
            CommonHelper objCH = new CommonHelper();
            DataSet ds = objUDS.GetUserDetails(Id);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.UserId = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["UserId"]);
                    obj.FirstName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["FirstName"]);
                    obj.LastName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["LastName"]);
                    obj.Password = Convert.ToString(objCH.DecryptData(ds.Tables[tblIndx].Rows[0]["Password"]));
                    obj.Email = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Email"]);
                    obj.Mobile = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Mobile"]);
                    obj.Status = Convert.ToInt16(ds.Tables[tblIndx].Rows[0]["Active"]);
                }
            }
            return obj;
        }

        public bool AddUser(UserInfo obj)
        {
            long RndmNumbr = GetRandomNumber(100000, 999999);
            CommonHelper objCH = new CommonHelper();
            obj.Password = objCH.EncryptData(RndmNumbr);
            return objUDS.AddUser(obj);
        }

        public bool DeleteUser(Int64 Id)
        {
            return objUDS.DeleteUser(Id);
        }

        //Function to get random number
        private static readonly Random getrandom = new Random();
        public static long GetRandomNumber(int min, int max)
        {
            lock (getrandom) // synchronize
            {
                return getrandom.Next(min, max);
            }
        }
    }
}