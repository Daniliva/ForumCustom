using System;
using System.ComponentModel.DataAnnotations;

namespace ForumCustom.WEB.Domain.Models
{
    public class MemberModel
    {
        private DateTime _dateOfBirth;

        public int MemberId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime ModifyTime { get; set; }

        [Required(ErrorMessage = "Enter First Name")]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Enter  Last Name")]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Enter  Nick Name")]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public string NickName { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                DateTime date2 = new DateTime(1900, 1, 1, 0, 0, 0);
                int result = DateTime.Compare(value, date2);

                if (result < 0)
                    _dateOfBirth = date2;
                else if (result == 0)
                    _dateOfBirth = value;
                else
                    _dateOfBirth = value;
            }
        }
    }
}