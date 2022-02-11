using ForumCustom.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ForumCustom.BLL.Contract.Manager
{
    public interface ITopicManager
    {
        Task<List<TopicInfo>> GetAll();

        Task<TopicInfo> GetById(int id);

        Task<List<TopicInfo>> GetByNickName(string nickName);

        Task<bool> AddTopic(TopicInfo topicInfo, UserInfo userInfo);

        Task<bool> AddCommentToTopic(TopicInfo topicInfo, CommentInfo comment);

        Task<Dictionary<int, string>> GetStatusDictionary();

        Task<bool> ChangeTopic(TopicInfo topicInfo);

        Task Delete(TopicInfo topic);
    }
}