using AP.Models;
using System.Collections.Generic;

namespace AP.MVC.Models
{
    public class AssignmentDetailsVM
    {
        public Assignment Assignment { get; set; }
        public List<User> AssignedStudents { get; set; }
        public List<Submission> Submissions { get; set; }
        public Submission MySubmission { get; set; }
    }
}
