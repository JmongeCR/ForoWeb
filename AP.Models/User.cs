using System.ComponentModel.DataAnnotations;

namespace AP.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio.")]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingresa un correo valido.")]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        [StringLength(20)]
        public string Role { get; set; }

        [StringLength(255)]
        public string PhotoUrl { get; set; }

        public bool IsActive { get; set; }
    }
}
