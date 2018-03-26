using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataServices.Masters;

namespace BusinessService.Masters
{
    public class MastersBusinessService
    {
        public DataTable GetHSCode()
        {
            MatersDataManager objMDM = new MatersDataManager();
            return objMDM.GetHSCodes();
        }
    }
}
