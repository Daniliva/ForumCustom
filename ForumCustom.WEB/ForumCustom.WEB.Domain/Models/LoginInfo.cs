using System.ComponentModel.DataAnnotations;

namespace ForumCustom.WEB.Domain.Models
{
    public class LoginInfo
    {
        [Required(ErrorMessage = "Enter Login")]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}