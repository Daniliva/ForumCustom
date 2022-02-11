using ForumCustom.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForumCustom.BLL.Contract.Manager
{
    public interface ICommentManager
    {
        Task Delete(CommentInfo comment, TopicInfo info);

        Task<bool> ChangeComment(CommentInfo commentInfo);

        Task<List<CommentInfo>> GetByNickName(string nickName);

        Task<CommentInfo> GetByNickNameId(string nickName, int id);

        Task<TopicInfo> GetTopicByIdComment(int id);

        Task<CommentInfo> GetById(int id);
    }
}