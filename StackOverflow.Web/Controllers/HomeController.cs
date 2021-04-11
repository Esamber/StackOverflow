using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StackOverflow.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using StackOverflow.Data;

namespace StackOverflow.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString;
        public HomeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }
        IActionResult Index()
        {
            QuestionsRepository db = new(_connectionString);
            IndexViewModel vm = new IndexViewModel()
            {
                Questions = db.GetQuestions()
            };
            return View(vm);
        }
    }
}
