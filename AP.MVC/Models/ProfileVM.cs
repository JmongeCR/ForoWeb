using System.Collections.Generic;
using AP.Models;

namespace AP.MVC.Models
{
    public class ProfileVM
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string PhotoUrl { get; set; }
        public List<Clase> Clases { get; set; } = new List<Clase>();
    }
}