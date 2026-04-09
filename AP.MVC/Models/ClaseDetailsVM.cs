using AP.Models;
using System.Collections.Generic;

namespace AP.MVC.Models
{
    public class ClaseDetailsVM
    {
        public Clase Clase { get; set; }
        public List<User> Students { get; set; }
        public List<User> AvailableStudents { get; set; }
    }
}
