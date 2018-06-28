﻿using System;
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
    public class CountryBusinessService
    {
        CountryDataService objCDS = new CountryDataService();
        public PageLoad_CountryList CountriesList()
        {
            CommonHelper objCH = new CommonHelper();
            PageLoad_CountryList obj = new PageLoad_CountryList();
            DataSet ds = objCDS.GetCountriesList();
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    List<Country> CountryList = new List<Country>();
                    Country objC = new Country();
                    foreach (DataRow dr in ds.Tables[tblIndx].Rows)
                    {
                        objC = new Country();
                        objC.ItemNumber = Convert.ToInt64(dr["ItemNumber"]);
                        objC.CountryId = Convert.ToInt64(dr["CountryId"]);
                        objC.CountryName = Convert.ToString(dr["CountryName"]);
                        objC.Status = Convert.ToInt16(dr["IsActive"]);
                        objC.IsInUse = Convert.ToBoolean(dr["IsInUse"]);
                        objC.CountryCode= Convert.ToString(dr["CountryCode"]);
                        objC.EnquiryEmail_SPS = Convert.ToString(dr["SPSEmail"]);
                        objC.EnquiryEmail_TBT = Convert.ToString(dr["TBTEmail"]);
                        CountryList.Add(objC);
                    }
                    obj.CountryList = CountryList;
                }
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.TotalCount = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["TotalCount"]);
                }
            }
            return obj;
        }

        public Country CountryDetails(Int64 Id)
        {
            Country obj = new Country();
            CommonHelper objCH = new CommonHelper();
            DataSet ds = objCDS.GetCountryDetails(Id);
            if (ds != null && ds.Tables.Count > 0)
            {
                int tblIndx = -1;
                tblIndx++;
                if (ds.Tables.Count > tblIndx && ds.Tables[tblIndx] != null && ds.Tables[tblIndx].Rows.Count > 0)
                {
                    obj.CountryId = Convert.ToInt64(ds.Tables[tblIndx].Rows[0]["CountryId"]);
                    obj.CountryName = Convert.ToString(ds.Tables[tblIndx].Rows[0]["CountryName"]);
                    obj.Status = Convert.ToInt16(ds.Tables[tblIndx].Rows[0]["IsActive"]);
                    obj.CountryCode = Convert.ToString(ds.Tables[tblIndx].Rows[0]["CountryCode"]);
                    obj.EnquiryEmail_SPS = Convert.ToString(ds.Tables[tblIndx].Rows[0]["SPSEmail"]); 
                    obj.EnquiryEmail_TBT = Convert.ToString(ds.Tables[tblIndx].Rows[0]["TBTEmail"]); 
                }
            }
            return obj;
        }

        public bool AddCountry(Int64 Id, AddCountry obj)
        {
            return objCDS.AddCountry(Id,obj);
        }

        public bool DeleteCountry(Int64 Id)
        {
            return objCDS.DeleteCountry(Id);
        }
        public DataTable CheckDuplicateCountryData(int id, string Callfor, string text)
        {
            return objCDS.CheckDuplicateCountryData(id, Callfor, text);
        }

    }
}
