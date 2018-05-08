using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.ManageAccess
{
    public class AddStakeHolder
    {
        public long StakeHolderId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public string OrgName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PIN { get; set; }
        [Required]
        public string HSCodes { get; set; }
    }

    public class StakeHolder
    {
        public long StakeHolderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StakeHolderName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public string Email { get; set; }
    }
    public class StakeHolderInfo: StakeHolder
    {
        public long ItemNumber { get; set; }
        public string Mobile { get; set; }
        public int Status { get; set; }
        public string OrgName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PIN { get; set; }
        public bool IsInUse { get; set; }
        public string HSCodes { get; set; }
    }
    public class PageLoad_StakeHolderList
    {
        public List<StakeHolderInfo> StakeHolderList { get; set; }
        public long TotalCount { get; set; }
    }
}
