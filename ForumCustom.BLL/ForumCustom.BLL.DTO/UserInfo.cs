using System;
using System.Collections.Generic;

namespace ForumCustom.BLL.DTO
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string Login { get; set; }
        public DateTime ModifyTime { get; set; }
        public IEnumerable<string> Role { get; set; }
    }
}