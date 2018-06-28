using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.ManageAccess
{
    public class AddStakeHolder
    {
        public long StakeHolderId { get; set; }
        [Required]
        public string StakeHolderName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public string OrgName { get; set; }
        [Required]
        public string HSCodes { get; set; }
        [Required]
        public string Designation { get; set; }
    }

    public class StakeHolder
    {
        public long StakeHolderId { get; set; }
        public string StakeHolderName { get; set; }
        public string Email { get; set; }
    }
    public class StakeHolderInfo : StakeHolder
    {
        public long ItemNumber { get; set; }
        public int Status { get; set; }
        public string OrgName { get; set; }
        public string HSCodes { get; set; }
        public bool IsInUse { get; set; }
        public string Designation { get; set; }
    }
    public class PageLoad_StakeHolderList
    {
        public List<StakeHolderInfo> StakeHolderList { get; set; }
        public long TotalCount { get; set; }
    }
}
