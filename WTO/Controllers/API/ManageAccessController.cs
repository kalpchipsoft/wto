using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessObjects.ManageAccess;
using BusinessService.ManageAccess;

namespace WTO.Controllers.API
{
    public class ManageAccessController : ApiController
    {
        #region "Users"
        [HttpPost]
        public IHttpActionResult GetUserDetails(Int64 Id)
        {
            UserBusinessService objUBS = new UserBusinessService();
            return Ok(objUBS.UserDetails(Id));
        }

        [HttpPost]
        public IHttpActionResult AddUser(UserInfo obj)
        {
            UserBusinessService objUBS = new UserBusinessService();
            return Ok(objUBS.AddUser(obj));
        }

        [HttpPost]
        public IHttpActionResult DeleteUser(Int64 Id)
        {
            UserBusinessService objUBS = new UserBusinessService();
            return Ok(objUBS.DeleteUser(Id));
        }
        #endregion

        #region "Country"
        [HttpPost]
        public IHttpActionResult GetCountryDetails(Int64 Id)
        {
            CountryBusinessService objCBS = new CountryBusinessService();
            return Ok(objCBS.CountryDetails(Id));
        }

        [HttpPost]
        public IHttpActionResult AddCountry(Country obj)
        {
            CountryBusinessService objCBS = new CountryBusinessService();
            return Ok(objCBS.AddCountry(obj));
        }

        [HttpPost]
        public IHttpActionResult DeleteCountry(Int64 Id)
        {
            CountryBusinessService objCBS = new CountryBusinessService();
            return Ok(objCBS.DeleteCountry(Id));
        }
        #endregion

        #region "StakeHolders"
        [HttpPost]
        public IHttpActionResult GetStakeHolderDetails(Int64 Id)
        {
            StakeHolderBusinessService objSHBS = new StakeHolderBusinessService();
            return Ok(objSHBS.StakeHolderDetails(Id));
        }

        [HttpPost]
        public IHttpActionResult AddStakeHolder(StakeHolderInfo obj)
        {
            StakeHolderBusinessService objSHBS = new StakeHolderBusinessService();
            return Ok(objSHBS.AddStakeHolder(obj));
        }

        [HttpPost]
        public IHttpActionResult DeleteStakeHolder(Int64 Id)
        {
            StakeHolderBusinessService objSHBS = new StakeHolderBusinessService();
            return Ok(objSHBS.DeleteStakeHolder(Id));
        }
        #endregion

        #region "Translators"
        [HttpPost]
        public IHttpActionResult GetTranslatorDetails(Int64 Id)
        {
            TranslatorBusinessService objTBS = new TranslatorBusinessService();
            return Ok(objTBS.TranslatorDetails(Id));
        }

        [HttpPost]
        public IHttpActionResult AddTranslator(TranslatorInfo obj)
        {
            TranslatorBusinessService objTBS = new TranslatorBusinessService();
            return Ok(objTBS.AddTranslator(obj));
        }

        [HttpPost]
        public IHttpActionResult DeleteTranslator(Int64 Id)
        {
            TranslatorBusinessService objTBS = new TranslatorBusinessService();
            return Ok(objTBS.DeleteTranslator(Id));
        }
        #endregion
    }
}