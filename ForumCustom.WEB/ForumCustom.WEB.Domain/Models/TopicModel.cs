using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ForumCustom.WEB.Domain.Models
{
    public class TopicModel
    {
        public int Id { get; set; }

        [StringLength(20, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public string Name { get; set; }

        [StringLength(200, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 4)]
        public string Text { get; set; }

        public string Nickname { get; set; }
        public List<CommentModel> Comments { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }
}