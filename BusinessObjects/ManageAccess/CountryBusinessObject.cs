using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.ManageAccess
{
    public class CountryBusinessObject
    {
    }

    public class Country
    {
        public long ItemNumber { get; set; }
        public long CountryId { get; set; }
        public string CountryName { get; set; }
        public int Status { get; set; }
        public bool IsInUse { get; set; }
        public string CountryCode { get; set; }
        public string EnquiryEmail_SPS { get; set; }
        public string EnquiryEmail_TBT { get; set; }
    }

    public class AddCountry
    {
        public long CountryId { get; set; }
        [Required]
        public string CountryName { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public string CountryCode { get; set; }
        public string EnquiryEmail_SPS { get; set; }
        public string EnquiryEmail_TBT { get; set; }
    }

    public class PageLoad_CountryList
    {
        public List<Country> CountryList { get; set; }
        public long TotalCount { get; set; }
    }
}
