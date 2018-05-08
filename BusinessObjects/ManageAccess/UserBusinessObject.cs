using BusinessObjects.Masters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.ManageAccess
{
    public class AddUser
    {
        public long UserId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public int RoleId { get; set; }

        public Attachment UserImage { get; set; }
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

    public class UserDetails : UserInfo
    {
        public Role UserRole { get; set; }
        public Attachment UserImage { get; set; }
    }

    public class PageLoad_UserList
    {
        public List<UserInfo> UserList { get; set; }
        public long TotalCount { get; set; }
        public List<Role> UserRoles { get; set; }
    }

    public class Attachment
    {
        public string FileName { get; set; }
        public string Content { get; set; }
        public string Path { get; set; }
    }
}
