using System;
using System.Collections.Generic;

namespace ForumCustom.DAL.Entities
{
    public class User
    {
        public User()
        {
            //Roles = new List<RoleUser>();
        }

        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public DateTime ModifyTime { get; set; }

        public virtual List<RoleUser> RoleUsers { get; set; }
    }
}