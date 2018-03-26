using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

    public class PageLoad_CountryList
    {
        public List<Country> CountryList { get; set; }
        public long TotalCount { get; set; }
    }
}
