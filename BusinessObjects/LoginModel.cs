using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LoginResult : CommonResponseModel
    {
        public long UserId { get; set; }

        public string UserName { get; set; }
    }
}