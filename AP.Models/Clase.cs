using System;

namespace AP.Models
{
    public class Clase
    {
        public int ClaseId { get; set; }
        public int CourseId { get; set; }
        public int ProfessorId { get; set; }
        public string Name { get; set; }
        public string CourseName { get; set; }
        public string ProfessorName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
