using System.ComponentModel.DataAnnotations;

namespace ForumCustom.WEB.Domain.Models
{
    public class CommentModel
    {
        public int Id { get; set; }

        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public string Comment { get; set; }

        public string Nickname { get; set; }
    }
}