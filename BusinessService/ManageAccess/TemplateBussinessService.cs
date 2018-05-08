using BusinessObjects.ManageAccess;
using BusinessObjects.Masters;
using DataServices.ManageAccess;
using System;
using System.Collections.Generic;
using System.Data;

namespace BusinessService.ManageAccess
{
    public class TemplateBussinessService
    {
        TemplateDataService objTDS;
        public TemplateList TemplateList()
        {
            TemplateList objTL = new TemplateList();
            objTDS = new TemplateDataService();
            DataSet ds = objTDS.GetTemplateList();
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<Template> TemplateList = new List<Template>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        Template objT = new Template();
                        objT.TemplateId = Convert.ToInt32(dr["TemplateId"]);
                        objT.TemplateType = Convert.ToString(dr["TemplateType"]);
                        objT.TemplateFor = Convert.ToString(dr["TemplateFor"]);
                        objT.TemplateStatus = Convert.ToBoolean(dr["IsActive"]);
                        TemplateList.Add(objT);
                    }
                    objTL.Templates = TemplateList;
                }

                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<TemplateFor> TemplateForList = new List<TemplateFor>();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        TemplateFor objT = new TemplateFor();
                        objT.Value = Convert.ToString(dr["Value"]);
                        objT.Text = Convert.ToString(dr["Text"]);
                        TemplateForList.Add(objT);
                    }
                    objTL.TemplateForList = TemplateForList;
                }
            }
            return objTL;
        }

        public TemplateDetails TemplateDetails(Int32 Id)
        {
            objTDS = new TemplateDataService();
            TemplateDetails obj = new TemplateDetails();
            DataSet ds = objTDS.GetTemplateDetails(Id);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.TemplateId = Convert.ToInt32(ds.Tables[tblIndx].Rows[0]["TemplateId"]);
                    obj.TemplateType = Convert.ToString(ds.Tables[tblIndx].Rows[0]["TemplateType"]);
                    obj.TemplateFor = Convert.ToString(ds.Tables[tblIndx].Rows[0]["TemplateFor"]);
                    obj.Subject = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Subject"]);
                    obj.Body = Convert.ToString(ds.Tables[tblIndx].Rows[0]["Body"]);
                    obj.TemplateStatus = Convert.ToBoolean(ds.Tables[tblIndx].Rows[0]["IsActive"]);
                }
            }
            return obj;
        }

        public Result InsertUpdateTemplate(Int32 Id, AddTemplate obj)
        {
            Result objR = new Result();
            objR.Status = "Failure";
            objR.Message = "Something went wrong. Please try again.";
            objTDS = new TemplateDataService();
            DataTable dt = objTDS.InsertUpdateTemplate(Id, obj);
            if (dt != null && dt.Rows.Count > 0)
            {
                objR.Status = "success";
                objR.Message = Convert.ToString(dt.Rows[0]["Message"]);
                objR.Id = Convert.ToInt64(dt.Rows[0]["TemplateId"]);
            }
            return objR;
        }

        public bool DeleteTemplate(Int32 Id)
        {
            objTDS = new TemplateDataService();
            return objTDS.DeleteTemplate(Id);
        }

        public List<TemplateField> GetTemplateFields()
        {
            List<TemplateField> TemplateFields = new List<TemplateField>();
            objTDS = new TemplateDataService();
            DataTable dt = objTDS.TemplateFields();
            if(dt!=null && dt.Rows.Count > 0)
            {
                foreach(DataRow dr in dt.Rows)
                {
                    TemplateField objT = new TemplateField();
                    objT.Text = Convert.ToString(dr["Text"]);
                    objT.Value = Convert.ToString(dr["Value"]);
                    TemplateFields.Add(objT);
                }
            }
            return TemplateFields;
        }
    }
}
