using System.ComponentModel.DataAnnotations;

namespace Proyecto_3.DTOs.Users
{
    public class UserCreateDto
    {
        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = string.Empty;
    }
}