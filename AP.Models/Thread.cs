using System;
using System.ComponentModel.DataAnnotations;

namespace AP.Models
{
    public class Thread
    {
        public int ThreadId { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage = "El titulo es obligatorio.")]
        [StringLength(150, ErrorMessage = "El titulo no puede superar los 150 caracteres.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "El mensaje es obligatorio.")]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; }
        public bool IsClosed { get; set; }
    }
}
