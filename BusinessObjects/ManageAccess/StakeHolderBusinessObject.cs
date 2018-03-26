using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ManageAccess
{
    public class StakeHolderBusinessObject
    {

    }
    public class StakeHolderInfo
    {
        public long ItemNumber { get; set; }
        public long StakeHolderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int Status { get; set; }
        public string OrgName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PIN { get; set; }
        public bool IsInUse { get; set; }
    }
    public class PageLoad_StakeHolderList
    {
        public List<StakeHolderInfo> StakeHolderList { get; set; }
        public long TotalCount { get; set; }
    }
}
