using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ForumCustom.DAL.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        [ForeignKey("UserId ")]
        public virtual User User { get; set; }
    }
}