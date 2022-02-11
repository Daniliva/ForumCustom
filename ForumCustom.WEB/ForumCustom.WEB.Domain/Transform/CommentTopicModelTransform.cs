using ForumCustom.BLL.DTO;
using ForumCustom.WEB.Domain.Models;

namespace ForumCustom.WEB.Domain.Transform
{
    public class CommentTopicModelTransform : ITransform<CommentTopicModel, TopicInfo>
    {
        public CommentTopicModel Transform(TopicInfo item)
        {
            return new CommentTopicModel() { Name = item.Name, Text = item.Text, Status = item.Status, NickNameAuthor = item.Nickname };
        }
    }
}