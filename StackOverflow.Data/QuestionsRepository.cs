using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StackOverflow.Data
{
    public class QuestionsRepository
    {
        private readonly string _connectionString;

        public QuestionsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public List<Question> GetQuestions()
        {
            using var ctx = new QuestionsDbContext(_connectionString);
            return ctx.Questions.Include(q => q.Answers)
                .Include(q => q.Likes)
                .Include(q => q.QuestionsTags)
                .ThenInclude(qt => qt.Tag)
                .ToList();
        }
        public void AddQuestion(Question q, List<string> tags)
        {
            using var ctx = new QuestionsDbContext(_connectionString);
            ctx.Questions.Add(q);
            ctx.SaveChanges();
            foreach (string tag in tags)
            {
                Tag t = GetTag(tag);
                int tagId;
                if (t == null)
                {
                    tagId = AddTag(tag);
                }
                else
                {
                    tagId = t.TagId;
                }
                ctx.QuestionsTags.Add(new QuestionsTags
                {
                    QuestionId = q.Id,
                    TagId = tagId
                });
            }
            ctx.SaveChanges();
        }
        private Tag GetTag(string name)
        {
            using var ctx = new QuestionsDbContext(_connectionString);
            return ctx.Tags.FirstOrDefault(t => t.TagName == name);
        }
        private int AddTag(string name)
        {
            using var ctx = new QuestionsDbContext(_connectionString);
            var tag = new Tag { TagName = name };
            ctx.Tags.Add(tag);
            ctx.SaveChanges();
            return tag.TagId;
        }
        public string GetUserNameById(int id)
        {
            var ctx = new QuestionsDbContext(_connectionString);
            return ctx.Users.Where(u => u.Id == id).Select(u => u.Name).FirstOrDefault();
        }
        public Question GetQuestion(int id)
        {
            using var ctx = new QuestionsDbContext(_connectionString);
            return ctx.Questions.Include(q  => q.Answers)
                .ThenInclude(a => a.User)
                .Include(q => q.Likes)
                .Include(q => q.QuestionsTags)
                .ThenInclude(qt => qt.Tag)
                .FirstOrDefault(q => q.Id == id);
        }
        public void AddAnswer(Answer a)
        {
            using var ctx = new QuestionsDbContext(_connectionString);
            ctx.Answers.Add(a);
            ctx.SaveChanges();
        }
        public void AddLike(Likes like)
        {
            var ctx = new QuestionsDbContext(_connectionString);
            ctx.Likes.Add(like);
            ctx.SaveChanges();
        }
        public int GetLikes(int questionId)
        {
            var ctx = new QuestionsDbContext(_connectionString);
            return ctx.Likes.Where(l => l.QuestionId == questionId).Count();
        }
        public bool CanLike(int questionId, int userId)
        {
            var ctx = new QuestionsDbContext(_connectionString);
            return !ctx.Likes.Any(l => l.UserId == userId && l.QuestionId == questionId); 
        }

    }
}
