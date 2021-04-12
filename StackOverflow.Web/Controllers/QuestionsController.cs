using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackOverflow.Data;
using Microsoft.Extensions.Configuration;
using StackOverflow.Web.Models;

namespace StackOverflow.Web.Controllers
{
    public class QuestionsController : Controller
    {
        private string _connectionString;
        public QuestionsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        [Authorize]
        public IActionResult Ask()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddQuestion(Question q, List<string> tags)
        {
            q.DatePosted = DateTime.Now;
            AccountRepository adb = new AccountRepository(_connectionString);
            q.UserId = adb.GetByEmail(User.Identity.Name).Id;
            QuestionsRepository db = new (_connectionString);
            db.AddQuestion(q, tags);
            return Redirect("/home/index");
        }
        public IActionResult ViewQuestion(int id)
        {
            QuestionsRepository db = new(_connectionString);
            AccountRepository adb = new(_connectionString);
            Question question = db.GetQuestion(id);
            QuestionViewModel vm = new()
            {
                Question = question,
                AskerName = db.GetUserNameById(question.UserId)
            };
            if (User.Identity.IsAuthenticated)
            {
                var userId = adb.GetByEmail(User.Identity.Name).Id;
                vm.CanLike = db.CanLike(id, userId);
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult AddAnswer(Answer a)
        {
            a.DatePosted = DateTime.Now;
            AccountRepository adb = new AccountRepository(_connectionString);
            a.UserId = adb.GetByEmail(User.Identity.Name).Id;
            QuestionsRepository db = new(_connectionString);
            db.AddAnswer(a);
            return Redirect($"/questions/viewquestion?id={a.QuestionId}");
        }
        [HttpPost]
        public IActionResult AddLike(int questionId)
        {
            AccountRepository adb = new AccountRepository(_connectionString);
            var userId = adb.GetByEmail(User.Identity.Name).Id;
            QuestionsRepository db = new(_connectionString);
            Likes like = new()
            {
                QuestionId = questionId,
                UserId = userId
            };
            db.AddLike(like);
            return Json(null);
        }
        public IActionResult GetLikes(int questionId)
        {
            var db = new QuestionsRepository(_connectionString);
            return Json(db.GetLikes(questionId));
        }
    }
}
