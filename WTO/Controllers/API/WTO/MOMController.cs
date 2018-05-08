using System.Web.Http;
using BusinessObjects.MOM;
using BusinessService.MOM;
using System;

namespace WTO.Controllers.API.WTO
{
    public class MOMController : ApiController
    {
        [HttpPost]
        public IHttpActionResult InsertUpdate_MomData(Int64 Id, AddMoM obj)
        {
            MomBusinessService objAM = new MomBusinessService();
            return Ok(objAM.InsertUpdateMomData(Id, obj));
        }

    }
}