using System;

namespace AP.Models
{
    public class Reply
    {
        public int ReplyId { get; set; }
        public int ThreadId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}