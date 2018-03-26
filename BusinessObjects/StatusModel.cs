using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BusinessObjects
{
    public class CommonResponseModel
    {
        public StatusType StatusType { get; set; }
        public MessageType MessageType { get; set; }

        public string Status
        {
            get
            {
                return StatusType.GetType().GetMember(StatusType.ToString()).First().GetCustomAttribute<DisplayAttribute>().GetName();
            }
        }
        public string Message
        {
            get
            {
                return MessageType.GetType().GetMember(MessageType.ToString()).First().GetCustomAttribute<DisplayAttribute>().GetName();
            }
        }
    }

    public enum StatusType
    {
        [Display(Name = "success")]
        SUCCESS,
        [Display(Name = "failure")]
        FAILURE
    }

    public enum MessageType
    {
        [Display(Name = "")]
        NO_MESSAGE,
        [Display(Name = "Wrong user-id.")]
        WRONG_USERID,
        [Display(Name = "Please enter correct user id")]
        WRONG_USERNAME,
        [Display(Name = "Please enter correct password")]
        WRONG_PASSWORD,
        [Display(Name = "Record has been deleted successfully")]
        RECORD_DELETED,
        [Display(Name = "Something went wrong. Please try again")]
        TRY_AGAIN,
    }
}
