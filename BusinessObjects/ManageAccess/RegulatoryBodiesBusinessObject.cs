using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ManageAccess
{
    public class RegulatoryBodiesBusinessObject
    {
    }
    public class RegulatoryBodies
    {
        public long ItemNumber { get; set; }
        public long RegulatoryBodyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Emailid { get; set; }
        public string Contact { get; set; }
        public int Status { get; set; }
        public bool IsInUse { get; set; }
    }
    public class AddRegulatoryBodies
    {
        public long RegulatoryBodyId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Emailid { get; set; }
        public string Contact { get; set; }
        public int Status { get; set; }
    }

    public class PageLoad_RegulatoryBodyList
    {
        public List<RegulatoryBodies> RegulatoryBodiesList { get; set; }
        public long TotalCount { get; set; }
    }
}
