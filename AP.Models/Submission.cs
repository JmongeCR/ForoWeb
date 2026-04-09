using System;

namespace AP.Models
{
    public class Submission
    {
        public int SubmissionId { get; set; }
        public int AssignmentId { get; set; }
        public int StudentId { get; set; }
        public string FileUrl { get; set; }
        public string Comment { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string StudentName { get; set; }
    }
}
