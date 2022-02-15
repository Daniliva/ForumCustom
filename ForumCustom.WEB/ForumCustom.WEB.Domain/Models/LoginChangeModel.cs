using System.ComponentModel.DataAnnotations;

namespace ForumCustom.WEB.Domain.Models
{
    public class LoginChangeModel
    {
        [Required(ErrorMessage = "Enter Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Enter Login")]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public string NewLogin { get; set; }

        [Required(ErrorMessage = "Enter Password")]
        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}