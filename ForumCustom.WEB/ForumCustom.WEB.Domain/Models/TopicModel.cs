using System.Collections.Generic;

namespace ForumCustom.WEB.Domain.Models
{
    public class TopicModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Nickname { get; set; }
        public List<CommentModel> Comments { get; set; }
        public string Status { get; set; }
        public string Comment { get; set; }
    }
}