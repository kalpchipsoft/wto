using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Translator
{
    public class Login
    {
        [Required(ErrorMessage = "Please enter username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class LoginResult : CommonResponseModel
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

        public int LoginCount { get; set; }
    }
}
