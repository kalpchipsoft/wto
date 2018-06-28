using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Notification
{
    public class RelatedMaterial
    {
        public Int64 MaterialId { get; set; }
        public Int64 NotificationId { get; set; }
        public string MaterialType { get; set; }
        public string MaterialNumber { get; set; }
        public string MaterialDescription { get; set; }
        public string DateOfMaterial { get; set; }
        public EditAttachment Attachment { get; set; }
    }

    public class AddRelatedMaterial
    {
        [Required]
        public Int64 NotificationId { get; set; }
        [Required]
        public string MaterialType { get; set; }
        [Required]
        public string MaterialNumber { get; set; }
        [Required]
        public string MaterialDescription { get; set; }
        [Required]
        public string DateOfMaterial { get; set; }

        [Required]
        public Attachment Attachment { get; set; }
    }

    public class AddRelatedMaterial_Output
    {
        public string Message { get; set; }
        public Int64 MaterialId { get; set; }
    }
}
