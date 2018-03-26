﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    }
}
