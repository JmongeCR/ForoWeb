using System;

namespace AP.Models
{
    public class Thread
    {
        public int ThreadId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}