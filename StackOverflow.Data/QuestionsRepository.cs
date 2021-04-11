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
                .Include(q => q.QuestionsTags)
                .ThenInclude(qt => qt.Tag)
                .ToList();
        }
    }
}
