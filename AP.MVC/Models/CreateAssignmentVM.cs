using AP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AP.MVC.Models
{
    public class CreateAssignmentVM
    {
        [Required(ErrorMessage = "El titulo es obligatorio")]
        public string Title { get; set; }

        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string Description { get; set; }

        [Required(ErrorMessage = "La fecha limite es obligatoria")]
        public DateTime DueDate { get; set; }

        public int ClaseId { get; set; }

        public List<Clase> ProfessorClases { get; set; } = new List<Clase>();
    }
}
