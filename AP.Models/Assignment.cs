using System;
using System.ComponentModel.DataAnnotations;

namespace AP.Models
{
    public class Assignment
    {
        public int AssignmentId { get; set; }
        public int ProfessorId { get; set; }

        [Required(ErrorMessage = "La clase es obligatoria.")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(150, ErrorMessage = "El título no puede superar los 150 caracteres.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La fecha de entrega es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        public string ClassName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
