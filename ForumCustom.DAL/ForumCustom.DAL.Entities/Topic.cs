using System.Collections.Generic;

namespace ForumCustom.DAL.Entities
{
    public class Topic
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public virtual Member Member { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public Status Status { get; set; }
    }
}