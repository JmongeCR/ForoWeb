using AP.Models;
using System.Collections.Generic;

namespace AP.MVC.Models
{
    public class CreateClaseVM
    {
        public string Name { get; set; }
        public int CourseId { get; set; }
        public int ProfessorId { get; set; }

        public List<Course> Courses { get; set; } = new List<Course>();
        public List<User> Professors { get; set; } = new List<User>();
    }
}
