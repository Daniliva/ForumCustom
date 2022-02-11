namespace ForumCustom.DAL.Entities
{
    public class RoleUser
    {
        public int Id { get; set; }
        public Role Role { get; set; }
        public User User { get; set; }
    }
}