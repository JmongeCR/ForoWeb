using System.Collections.Generic;
using AP.Models;

namespace AP.MVC.Models
{
    public class ThreadDetailsVM
    {
        public Thread Thread { get; set; }
        public List<Reply> Replies { get; set; }
    }
}
