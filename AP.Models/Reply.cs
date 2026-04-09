using System;
using System.ComponentModel.DataAnnotations;

namespace AP.Models
{
    public class Reply
    {
        public int ReplyId { get; set; }
        public int ThreadId { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "La respuesta es obligatoria.")]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
    }
}
