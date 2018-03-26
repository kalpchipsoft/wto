using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusinessObjects;
using BusinessObjects.Notification;
using BusinessService.Notification;
using DataServices.WTO;

namespace BusinessService.Notification
{
    public class NotificationBusinessService
    {
        public AddNoti_Result InsertUpdateNotification(AddNotification obj)
        {
            AddNoti_Result objR = new AddNoti_Result();
            AddNotificationDataManager objDM = new AddNotificationDataManager();
            DataSet ds = objDM.InsertUpdate_Notification(obj);
            if (ds != null && ds.Tables.Count > 0)
            {
                objR.StatusType = StatusType.SUCCESS;
                objR.MessageType = MessageType.NO_MESSAGE;
            }
            else
            {
                objR.StatusType = StatusType.FAILURE;
                objR.MessageType = MessageType.TRY_AGAIN;
            }

            return objR;
        }

        public EditNotification PageLoad_EditNotification(Int64 Id)
        {
            EditNotification objR = new EditNotification();
            AddNotificationDataManager objDM = new AddNotificationDataManager();
            DataSet ds = objDM.Edit_Notification(Id);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<Country> CountryList = new List<Country>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        Country objCountry = new Country();
                        objCountry.CountryId = Convert.ToInt64(dr["CountryId"]);
                        objCountry.Name = Convert.ToString(dr["Country"]);
                        CountryList.Add(objCountry);
                    }
                    objR.CountryList = CountryList;
                }
                //tblIndx++;
                //if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                //{
                //    List<HSCodes> HSCodeList = new List<HSCodes>();
                //    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                //    {
                //        HSCodes objHSCode = new HSCodes();
                //        objHSCode.HSCode = Convert.ToInt64(dr["HSCode"]);
                //        objHSCode.HSCode1 = Convert.ToInt64(dr["HSCode1"]);
                //        objHSCode.HSCode2 = Convert.ToInt64(dr["HSCode2"]);
                //        objHSCode.Description = Convert.ToString(dr["Description"]);
                //        HSCodeList.Add(objHSCode);
                //    }
                //    objR.HSCodeList = HSCodeList;
                //}
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    objR.UserId = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["CreatedBy"]);
                    objR.NotificationId = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["NotificationId"]);
                    objR.Noti_Type = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Type"]);
                    objR.Noti_Status = Convert.ToInt16(ds.Tables[tblIndx].Rows[0]["Status"]);
                    objR.Noti_Number = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Number"]);
                    objR.Noti_Date = Convert.ToDateTime(ds.Tables[tblIndx].Rows[0]["DateOfNotification"]);
                    objR.FinalDateOfComments = Convert.ToDateTime(ds.Tables[tblIndx].Rows[0]["FinalDateOfComment"]);
                    objR.SendResponseBy = Convert.ToDateTime(ds.Tables[tblIndx].Rows[0]["SendResponseBy"]);
                    objR.Noti_Country = Convert.ToInt32(ds.Tables[tblIndx].Rows[0]["CountryId"]);
                    objR.Title = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Title"]);
                    objR.ResponsibleAgency = Convert.ToString(ds.Tables[tblIndx].Rows[0]["AgencyResponsible"]);
                    objR.Noti_UnderArticle = Convert.ToString(ds.Tables[tblIndx].Rows[0]["UnderArticle"]);
                    objR.ProductsCovered = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Products"]);
                    objR.Description = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Description"]);
                    objR.HSCodes = Convert.ToString(ds.Tables[tblIndx].Rows[0]["HSCode"]);
                }

                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<HSCodes> SelectedHSCodesList = new List<HSCodes>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        HSCodes objHSCodes = new HSCodes();
                        objHSCodes.HSCode = Convert.ToString(dr["HSCode"]);
                        objHSCodes.Text = Convert.ToString(dr["Description"]);
                        SelectedHSCodesList.Add(objHSCodes);
                    }
                    objR.SelectedHSCodes = SelectedHSCodesList;
                }

                List<Noti_Type> Noti_TypeList = new List<Noti_Type>();
                Noti_Type objType = new Noti_Type();
                objType.Id = 1;
                objType.Type = "Draft";
                Noti_TypeList.Add(objType);
                objType = new Noti_Type();
                objType.Id = 2;
                objType.Type = "Final";
                Noti_TypeList.Add(objType);
                objR.Noti_TypeList = Noti_TypeList;
            }
            return objR;
        }
    }
}
