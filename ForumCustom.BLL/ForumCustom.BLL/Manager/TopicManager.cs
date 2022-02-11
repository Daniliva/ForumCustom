using ForumCustom.BLL.Contract.Manager;
using ForumCustom.BLL.Contract.Transform;
using ForumCustom.BLL.DTO;
using ForumCustom.BLL.Transform;
using ForumCustom.DAL.Contract.Interfaces;
using ForumCustom.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ForumCustom.BLL.Manager
{
    public class TopicManager : ITopicManager
    {
        private readonly ITransform<Topic, TopicInfo> _topicTransform;
        private readonly ITransform<Comment, CommentInfo> _commentTransform;
        private readonly IRepository<Member> _memberRepository;

        private readonly IRepository<Topic> _topicRepository;

        public TopicManager(IRepository<Member> memberRepository, IRepository<Topic> topicRepository)
        {
            _topicTransform = new TopicTransform();
            _memberRepository = memberRepository;
            _topicRepository = topicRepository;
            _commentTransform = new CommentTransform();
        }

        public async Task<List<TopicInfo>> GetAll()
        {
            var topics = await _topicRepository.GetAll();
            return topics.Select(x => _topicTransform.Transform(x)).ToList();
        }

        public async Task<TopicInfo> GetById(int id)
        {
            var topics = await _topicRepository.Get(id);
            var returnV = _topicTransform.Transform(topics);
            returnV.Comments = topics.Comments?.Select(x => _commentTransform.Transform(x)).ToList();
            return returnV;
        }

        public async Task<List<TopicInfo>> GetByNickName(string nickName)
        {
            var topics = await _topicRepository.FindAsync(x => x.Member.Nickname == nickName);
            var returnV = topics.Select(x => _topicTransform.Transform(x)).ToList();

            return returnV;
        }

        public async Task<bool> AddTopic(TopicInfo topicInfo, UserInfo userInfo)
        {
            // topicInfo.Status = Status.Done;
            var topic = _topicTransform.Transform(topicInfo);
            var memberFind = _memberRepository
                .FindAsync(member => member.User.Id == userInfo.UserId && member.User.Login == userInfo.Login)
                .Result;
            topic.Member = memberFind.FirstOrDefault();
            topic.Status = Status.Done;
            await _topicRepository.Add(topic);
            return true;
        }

        public async Task<bool> ChangeTopic(TopicInfo topicInfo)
        {
            // topicInfo.Status = Status.Done;
            var topic = await _topicRepository.Get(topicInfo.Id);
            var topicChange = _topicTransform.Transform(topicInfo);

            topic.Status = topicChange.Status;
            topic.Name = topicChange.Name;
            topic.Text = topicChange.Text;
            await _topicRepository.Update(topic);
            return true;
        }

        public async Task<bool> AddCommentToTopic(TopicInfo topicInfo, CommentInfo comment)
        {
            // topicInfo.Status = Status.Done;
            var topic = await _topicRepository.Get(topicInfo.Id);
            var commentNew = _commentTransform.Transform(comment);
            commentNew.Member = _memberRepository.FindAsync(x => x.Nickname == comment.Nickname).Result.First();

            if (topic.Comments != null)
                topic.Comments.Add(commentNew);
            else
            {
                topic.Comments = new List<Comment>();
                topic.Comments.Add(commentNew);
            }
            await _topicRepository.Update(topic);
            return true;
        }

        public async Task Delete(TopicInfo item)
        {
            var topics = await _topicRepository.FindAsync(x => x.Id == item.Id);
            await _topicRepository.Delete(topics.First().Id);
        }

        public async Task<Dictionary<int, string>> GetStatusDictionary() => await Task.Run(() => GetDictionaryByEnum(typeof(Status)));

        private Dictionary<int, string> GetDictionaryByEnum(Type type)
        {
            var dictionary = new Dictionary<int, string>();
            var names = Enum.GetNames(type);

            for (var i = 0; i < names.Length; i++)
            {
                dictionary.Add(i, Regex.Replace(names[i], @"([A-Z])", " $1").Trim());
            }

            return dictionary;
        }
    }
}