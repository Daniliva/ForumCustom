namespace ForumCustom.DAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; }
        public virtual Topic Topic { get; set; }
        public virtual Member Member { get; set; }
    }
}