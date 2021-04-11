using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackOverflow.Data;

namespace StackOverflow.Web.Models
{
    public class IndexViewModel
    {
        public List<Question> Questions { get; set; }
    }
}
