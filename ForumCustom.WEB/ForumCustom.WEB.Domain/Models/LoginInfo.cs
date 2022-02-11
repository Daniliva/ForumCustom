using System.ComponentModel.DataAnnotations;

namespace ForumCustom.WEB.Domain.Models
{
    public class LoginInfo
    {
        [Required(ErrorMessage = "�� ������ Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "�� ������ Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}