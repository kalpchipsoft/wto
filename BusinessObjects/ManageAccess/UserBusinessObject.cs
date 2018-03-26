using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects.ManageAccess
{
    public class UserBusinessObject
    {
        
    }

    public class UserInfo
    {
        public long ItemNumber { get; set; }
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int Status { get; set; }

    }

    public class PageLoad_UserList
    {
        public List<UserInfo> UserList { get; set; }
        public long TotalCount { get; set; }
    }
}
