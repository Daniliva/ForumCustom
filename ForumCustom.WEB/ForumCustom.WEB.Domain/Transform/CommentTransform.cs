using ForumCustom.BLL.DTO;
using ForumCustom.WEB.Domain.Models;

namespace ForumCustom.WEB.Domain.Transform
{
    public class CommentTransform : ITransform<CommentModel, CommentInfo>
    {
        public CommentModel Transform(CommentInfo item)
        {
            return new CommentModel() { Comment = item.Comment, Id = item.Id, Nickname = item.Nickname };
        }
    }
}