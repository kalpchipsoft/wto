using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter userid")]
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