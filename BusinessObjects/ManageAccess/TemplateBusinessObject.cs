using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ManageAccess
{
    public class AddTemplate
    {
        public int TemplateId { get; set; }
        [Required]
        public string TemplateFor { get; set; }
        [Required]
        public string TemplateType { get; set; }
        [Required]
        public bool TemplateStatus { get; set; }

        public string Subject { get; set; }
        [Required]
        public string Body { get; set; }

        [Required]
        public string NotificationType { get; set; }
    }
    public class Template
    {
        public int TemplateId { get; set; }
        public string TemplateFor { get; set; }
        public string TemplateType { get; set; }
        public bool TemplateStatus { get; set; }
        public string NotificationType { get; set; }
    }

    public class TemplateDetails : Template
    {
        public string Subject { get; set; }
        public string Body { get; set; }
    }

    public class TemplateList
    {
        public List<TemplateFor> TemplateForList { get; set; }

        public List<Template> Templates { get; set; }
    }

    public class TemplateFor
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }

    public class TemplateField
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
