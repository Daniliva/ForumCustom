using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ForumCustom.WEB.Domain.Models
{
    public class UserRoleModel

    {
        public UserRoleModel()
        {
            SelectedSubjects = new List<string>();
        }

        public int UserId { get; set; }
        public string Login { get; set; }
        public DateTime ModifyTime { get; set; }
        public string Roles { get; set; }
        public List<string> SelectedSubjects { get; set; }
    }
}