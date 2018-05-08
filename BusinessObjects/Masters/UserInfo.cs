using System;

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

        public string Role { get; set; }
    }
}
