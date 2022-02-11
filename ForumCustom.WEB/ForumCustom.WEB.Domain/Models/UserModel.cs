using System;

namespace ForumCustom.WEB.Domain.Models
{
    public class UserModel

    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public DateTime ModifyTime { get; set; }
        public string Roles { get; set; }
    }
}