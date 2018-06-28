using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ManageAccess
{
    public class LanguageBusinessObject
    {

    }
    public class LanguageDetails
    {
        public long LanguageId { get; set; }
        public string Language { get; set; }
        public int Status { get; set; }
    }

    public class LanguageList
    {
        public List<LanguageDetails> languagelist { get; set; }
        public long TotalCount { get; set; }
    }
}
