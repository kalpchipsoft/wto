using System;
using System.Web.Http;
using BusinessService.Masters;

namespace WTO.Controllers.API.Master
{
    public class MastersController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetHSCode()
        {
            MastersBusinessService objMstr = new MastersBusinessService();
            return Ok(objMstr.GetHSCode());
        }

        [HttpGet]
        public IHttpActionResult GetTranslaters(Int32  Id)
        {
            MastersBusinessService objMstr = new MastersBusinessService();
            return Ok(objMstr.GetTranslater(Id));
        }

        [HttpGet]
        public IHttpActionResult GetLanguages()
        {
            MastersBusinessService objMstr = new MastersBusinessService();
            return Ok(objMstr.GetLanguages());
        }

        [HttpGet]
        public IHttpActionResult IsEmailExists(string Email, string callFor)
        {
            MastersBusinessService objMstr = new MastersBusinessService();
            return Ok(objMstr.CheckIsEmailExists(Email,callFor));
        }

        [HttpGet]
        public IHttpActionResult GetRegulatoryBodies()
        {
            MastersBusinessService objMstr = new MastersBusinessService();
            return Ok(objMstr.GetRegulatoryBodies());
        }

        [HttpGet]
        public IHttpActionResult GetInternalStakeHolder()
        {
            MastersBusinessService objMstr = new MastersBusinessService();
            return Ok(objMstr.GetInternalStakeHolder());
        }
    }
}
