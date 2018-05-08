using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.ManageAccess
{
    public class AddTranslator
    {
        public long TranslatorId { get; set; }
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
        public string LanguageIds { get; set; }
        public string Password { get; set; }
        [Required]
        public bool IsWelcomeMailSent { get; set; }
    }

    public class TranslatorInfo
    {
        public long TranslatorId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TranslatorName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
    }

    public class TranslatorDetails : TranslatorInfo
    {
        public long ItemNumber { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int Status { get; set; }
        public string Languages { get; set; }
        public bool IsInUse { get; set; }
        public string LanguageIds { get; set; }
        public string Password { get; set; }
        public bool IsWelcomeMailSent { get; set; }
    }

    public class PageLoad_TranslatorList
    {
        public List<TranslatorDetails> TranslatorList { get; set; }
        public long TotalCount { get; set; }
    }
}
