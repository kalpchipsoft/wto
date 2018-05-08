using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        [Display(Name = "The user id provided by you is invalid.")]
        WRONG_USERID,
        [Display(Name = "The username provided by you is invalid.")]
        WRONG_USERNAME,
        [Display(Name = "The password you've entered is incorrect.")]
        WRONG_PASSWORD,
        [Display(Name = "Record has been deleted successfully")]
        RECORD_DELETED,
        [Display(Name = "Something went wrong. Please try again")]
        TRY_AGAIN,
    }
}
