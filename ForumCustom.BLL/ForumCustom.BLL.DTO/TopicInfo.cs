using System.Collections.Generic;

namespace ForumCustom.BLL.DTO
{
    public class TopicInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Nickname { get; set; }
        public List<CommentInfo> Comments { get; set; }
        public string Status { get; set; }
    }
}