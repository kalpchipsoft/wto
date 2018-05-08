using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Notification
{
    public class Notification_Template_Search
    {
        public string TemplateType { get; set; }
        public string TemplateFor { get; set; }
    }

    public class Notification_Template
    {
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
