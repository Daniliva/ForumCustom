using ForumCustom.BLL.Contract.Exceptions;
using ForumCustom.BLL.Contract.Transform;
using ForumCustom.BLL.DTO;
using ForumCustom.DAL.Entities;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ForumCustom.BLL.Transform
{
    public class TopicTransform : ITransform<Topic, TopicInfo>
    {
        public Topic Transform(TopicInfo item)
        {
            try
            {
                if (string.IsNullOrEmpty(item.Status))
                {
                    item.Status = GetEnumExtraction(typeof(Status), (int)Status.Done);
                }
                return new Topic() { Id = item.Id, Name = item.Name, Text = item.Text, Status = (Status)Enum.Parse(typeof(Status), item.Status, true) };
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }
        }

        private string GetEnumExtraction(Type enumType, int id)
        {
            var enumName = Enum.GetName(enumType, id);
            if (enumName == null)
            {
                var names = Enum.GetNames(enumType);
                enumName = names.LastOrDefault();
            }

            return Regex.Replace(enumName, @"([A-Z])", " $1").Trim();
        }

        public TopicInfo Transform(Topic item)
        {
            try
            {
                return new TopicInfo() { Id = item.Id, Name = item.Name, Text = item.Text, Status = GetEnumExtraction(typeof(Status), (int)item.Status), Nickname = item.Member?.Nickname };
            }
            catch (NullReferenceException e)
            {
                throw new ChangeException("");
            }
        }
    }
}