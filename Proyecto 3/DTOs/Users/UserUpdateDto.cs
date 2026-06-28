using System.ComponentModel.DataAnnotations;

namespace Proyecto_3.DTOs.Users
{
    public class UserUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        public string? Password { get; set; }

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}