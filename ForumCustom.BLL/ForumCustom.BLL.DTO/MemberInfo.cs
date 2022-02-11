using System;

namespace ForumCustom.BLL.DTO
{
    public class MemberInfo : ICloneable
    {
        public int MemberId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public object Clone()
        {
            return new MemberInfo()
            {
                MemberId = MemberId,
                CreateTime = CreateTime,
                DateOfBirth = DateOfBirth,
                Email = Email,
                FirstName = FirstName,
                IsActive = IsActive,
                LastName = LastName,
                ModifyTime = ModifyTime,
                NickName = NickName
            };
        }
    }
}