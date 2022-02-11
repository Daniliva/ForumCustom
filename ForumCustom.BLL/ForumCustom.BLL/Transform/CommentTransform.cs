using ForumCustom.BLL.Contract.Exceptions;
using ForumCustom.BLL.Contract.Transform;
using ForumCustom.BLL.DTO;
using ForumCustom.DAL.Entities;
using System;

namespace ForumCustom.BLL.Transform
{
    public class CommentTransform : ITransform<Comment, CommentInfo>
    {
        public Comment Transform(CommentInfo item)
        {
            try
            {
                return new Comment() { Text = item.Comment, Id = item.Id };
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }
        }

        public CommentInfo Transform(Comment item)
        {
            try
            {
                return new CommentInfo() { Comment = item.Text, Id = item.Id, Nickname = item.Member?.Nickname };
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }
        }
    }
}