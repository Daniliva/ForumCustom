using ForumCustom.BLL.DTO;
using ForumCustom.WEB.Domain.Models;
using System.Linq;

namespace ForumCustom.WEB.Domain.Transform
{
    public class TopicTransform : ITransform<TopicModel, TopicInfo>
    {
        private readonly CommentTransform _commentTransform;

        public TopicTransform()
        {
            this._commentTransform = new CommentTransform();
        }

        public TopicModel Transform(TopicInfo item)
        {
            return new TopicModel() { Id = item.Id, Name = item.Name, Text = item.Text, Status = item.Status, Nickname = item.Nickname, Comments = item.Comments.Select(x => _commentTransform.Transform(x)).ToList() };
        }
    }
}