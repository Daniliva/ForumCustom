using ForumCustom.BLL.Contract.Exceptions;
using ForumCustom.BLL.Contract.Manager;
using ForumCustom.BLL.Contract.Transform;
using ForumCustom.BLL.DTO;
using ForumCustom.BLL.Transform;
using ForumCustom.DAL.Contract.Interfaces;
using ForumCustom.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumCustom.BLL.Manager
{
    public class CommentManager : ICommentManager
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly ITransform<Comment, CommentInfo> _commentTransform;
        private readonly IRepository<Topic> _topicRepository;
        private ITransform<Topic, TopicInfo> _topicTransform;

        public CommentManager(IRepository<Comment> commentRepository, IRepository<Topic> topicRepository)
        {
            _commentRepository = commentRepository;
            _topicRepository = topicRepository;
            _commentTransform = new CommentTransform();
            _topicTransform = new TopicTransform();
        }

        public async Task Delete(CommentInfo comment, TopicInfo info)
        {
            var topic = await _topicRepository.Get(info.Id);
            var commentRemove = await _commentRepository.FindAsync(x => x.Id == comment.Id);

            if (topic.Comments != null && commentRemove != null)
            {
                topic.Comments.Remove(commentRemove.First());
            }
            await _topicRepository.Update(topic);
            await _commentRepository.Delete(comment.Id);
        }

        public async Task<bool> ChangeComment(CommentInfo commentInfo)
        {
            var comment = await _commentRepository.Get(commentInfo.Id);
            comment.Text = commentInfo.Comment;
            await _commentRepository.Update(comment);
            return true;
        }

        public async Task<List<CommentInfo>> GetByNickName(string nickName)
        {
            var topics = await _commentRepository.FindAsync(x => x.Member.Nickname == nickName);
            var returnV = topics.Select(x => _commentTransform.Transform(x)).ToList();

            return returnV;
        }

        public async Task<List<CommentInfo>> GetAll()
        {
            var topics = await _commentRepository.GetAll();

            try
            {
                var returnV = topics.Select(x => _commentTransform.Transform(x)).ToList();
                return returnV;
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }
        }

        public async Task<CommentInfo> GetById(int id)
        {
            try
            {
                var topics = await _commentRepository.FindAsync(x => x.Id == id);
                var returnV = topics.Select(x => _commentTransform.Transform(x)).ToList();

                return returnV.First();
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }
        }

        public async Task<TopicInfo> GetTopicByIdComment(int id)
        {
            try
            {
                var topics = await _commentRepository.FindAsync(x => x.Id == id);
                var topic = topics.FirstOrDefault()?.Topic;
                return _topicTransform.Transform(topic);
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }
        }

        public async Task<CommentInfo> GetByNickNameId(string nickName, int id)
        {
            try
            {
                var topics = await _commentRepository.FindAsync(x => x.Member.Nickname == nickName && x.Id == id);
                var returnV = topics.Select(x => _commentTransform.Transform(x)).ToList();
                return returnV.FirstOrDefault();
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }
        }
    }
}