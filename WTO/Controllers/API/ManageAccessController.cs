using System;
using System.Web.Http;
using BusinessObjects.ManageAccess;
using BusinessService.ManageAccess;
using WTO.Handler;

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
        public IHttpActionResult AddUser(Int64 Id, AddUser obj)
        {
            UserBusinessService objUBS = new UserBusinessService();
            return Ok(objUBS.AddUser(Id, obj));
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
        public IHttpActionResult AddCountry(Int64 Id, AddCountry obj)
        {
            CountryBusinessService objCBS = new CountryBusinessService();
            return Ok(objCBS.AddCountry(Id, obj));
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
        public IHttpActionResult AddStakeHolder(Int64 Id, AddStakeHolder obj)
        {
            StakeHolderBusinessService objSHBS = new StakeHolderBusinessService();
            return Ok(objSHBS.AddStakeHolder(Id, obj));
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
        public IHttpActionResult AddTranslator(Int64 Id, AddTranslator obj)
        {
            TranslatorBusinessService objTBS = new TranslatorBusinessService();
            return Ok(objTBS.AddTranslator(Id, obj));
        }

        [HttpPost]
        public IHttpActionResult DeleteTranslator(Int64 Id)
        {
            TranslatorBusinessService objTBS = new TranslatorBusinessService();
            return Ok(objTBS.DeleteTranslator(Id));
        }

        [HttpPost]
        public IHttpActionResult SendWelcomeMailToTranslator(Int32 Id)
        {
            TranslatorBusinessService objTBS = new TranslatorBusinessService();

            if (Id > 0)
            {
                TranslatorDetails objT = new TranslatorDetails();
                objT = objTBS.SendWelcomeMail(Id);
                if (objT != null)
                {
                    SendMail objMail = new SendMail();
                    string MailBody = objTBS.MailbodyForTranslator(objT);
                    objMail.SendAsyncEMail("Ashvini.chipsoft@gmail.com", "", "", "", "WTO - World Trade Organization", "Welcome to WTO", MailBody, null);
                }
            }

            return Ok();
        }
        #endregion

        #region "Templates"
        [HttpPost]
        public IHttpActionResult GetTemplateDetails(Int32 Id)
        {
            TemplateBussinessService objTBS = new TemplateBussinessService();
            return Ok(objTBS.TemplateDetails(Id));
        }

        [HttpPost]
        public IHttpActionResult AddUpdateTemplate(Int32 Id, AddTemplate obj)
        {
            TemplateBussinessService objTBS = new TemplateBussinessService();
            return Ok(objTBS.InsertUpdateTemplate(Id, obj));
        }

        [HttpPost]
        public IHttpActionResult DeleteTemplate(Int32 Id)
        {
            TemplateBussinessService objTBS = new TemplateBussinessService();
            return Ok(objTBS.DeleteTemplate(Id));
        }

        [HttpGet]
        public IHttpActionResult GetTemplateFields()
        {
            TemplateBussinessService objTBS = new TemplateBussinessService();
            return Ok(objTBS.GetTemplateFields());
        }
        #endregion
    }
}