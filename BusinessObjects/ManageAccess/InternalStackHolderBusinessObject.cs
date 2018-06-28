using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ManageAccess
{
   public class InternalStackHolderBusinessObject
    {
    }
    public class InternalStackHolder
    {
        public long ItemNumber { get; set; }
        public long InternalStakeholdersId { get; set; }
        public string Name { get; set; }
        public string Emailid { get; set; }
        public string OrgName { get; set; }
        public string Designation { get; set; }
        public int Status { get; set; }
        public bool IsInUse { get; set; }
    }
    public class AddInternalStackHolder
    {
        public Int32 InternalStakeholdersId { get; set; }
        public string Name { get; set; }
        public string Emailid { get; set; }
        public string OrgName { get; set; }
        public string Designation { get; set; }
        public int Status { get; set; }
    }
    public class PageLoad_InternalStackHolderList
    {
        public List<InternalStackHolder> InternalStackHolderList { get; set; }
        public long TotalCount { get; set; }
    }
}
