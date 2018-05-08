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

                #region "User List"
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
                        objUI.Status = Convert.ToInt16(dr["IsActive"]);
                        objUserList.Add(objUI);
                    }
                    obj.UserList = objUserList;
                }
                #endregion

                #region "Users Count"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.TotalCount = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["TotalCount"]);
                }
                #endregion

                #region "Users Roles List"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<BusinessObjects.Masters.Role> UserRoleList = new List<BusinessObjects.Masters.Role>();
                    foreach(DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        BusinessObjects.Masters.Role objR = new BusinessObjects.Masters.Role();
                        objR.RoleId = Convert.ToInt32(dr["RoleId"]);
                        objR.RoleName = Convert.ToString(dr["Role"]);
                        UserRoleList.Add(objR);
                    }
                    obj.UserRoles = UserRoleList;
                }
                #endregion
            }
            return obj;
        }

        public UserDetails UserDetails(Int64 Id)
        {
            UserDetails obj = new UserDetails();
            CommonHelper objCH = new CommonHelper();
            DataSet ds = objUDS.GetUserDetails(Id);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;

                #region "User List"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.UserId = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["UserId"]);
                    obj.FirstName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["FirstName"]);
                    obj.LastName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["LastName"]);
                    obj.Password = Convert.ToString(objCH.DecryptData(ds.Tables[tblIndx].Rows[0]["Password"]));
                    obj.Email = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Email"]);
                    obj.Mobile = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Mobile"]);
                    obj.Status = Convert.ToInt16(ds.Tables[tblIndx].Rows[0]["IsActive"]);

                    BusinessObjects.Masters.Role objR = new BusinessObjects.Masters.Role();
                    objR.RoleId = Convert.ToInt32(ds.Tables[tblIndx].Rows[0]["RoleId"]);
                    objR.RoleName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Role"]);
                    obj.UserRole = objR;

                    Attachment objA = new Attachment();
                    objA.FileName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Image"]);
                    objA.Path = "/Attachments/UserImage/" + obj.UserId + "_" + Convert.ToString(ds.Tables[tblIndx].Rows[0]["Image"]);
                    obj.UserImage = objA;
                }
                #endregion

                #region "Users Count"
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    
                }
                #endregion
            }
            return obj;
        }

        public bool AddUser(Int64 Id,AddUser obj)
        {
            CommonHelper objCH = new CommonHelper();
            long RndmNumbr = objCH.GetRandomNumber(100000, 999999);
            obj.Password = objCH.EncryptData(RndmNumbr);
            obj.UserId = objUDS.AddUser(Id,obj);
            if (obj.UserId > 0)
            {
                #region "Attachments"
                if (obj.UserImage != null && obj.UserImage.Content != "")
                {
                    try
                    {
                        byte[] bytes = null;
                        if (obj.UserImage.Content.IndexOf(',') >= 0)
                        {
                            var myString = obj.UserImage.Content.Split(new char[] { ',' });
                            bytes = Convert.FromBase64String(myString[1]);
                        }
                        else
                            bytes = Convert.FromBase64String(obj.UserImage.Content);

                        if (obj.UserImage.FileName.Length > 0 && bytes.Length > 0)
                        {
                            string filePath = System.Web.HttpContext.Current.Server.MapPath("/Attachments/UserImage/" + obj.UserId + "_" + obj.UserImage.FileName);
                            System.IO.File.WriteAllBytes(filePath, bytes);
                        }
                    }
                    catch (Exception ex) { }
                }
                #endregion

                return true;
            }
            else
                return false;
        }

        public bool DeleteUser(Int64 Id)
        {
            return objUDS.DeleteUser(Id);
        }
    }
}