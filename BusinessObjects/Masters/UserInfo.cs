using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.Masters
{
    public class UserInfo : CommonResponseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public long UserId { get; set; }
        public String ImageName { get; set; }
        public string Image
        {
            get
            {
                return Convert.ToString(UserId) + "_" + ImageName;
            }
        }
    }
}
