using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackOverflow.Data;

namespace StackOverflow.Web.Models
{
    public class QuestionViewModel
    {
        public Question Question { get; set; }
        public string AskerName { get; set; }
        public bool CanLike { get; set; }
    }
}
