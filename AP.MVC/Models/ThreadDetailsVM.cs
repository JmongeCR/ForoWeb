using System.Collections.Generic;
using AP.Models;

namespace APMVC.Models
{
    public class ThreadDetailsVM
    {
        public Thread Thread { get; set; }
        public List<Reply> Replies { get; set; }
    }
}