using System.ComponentModel.DataAnnotations;

namespace Proyecto_3.DTOs.Users
{
    // Este sirve para MANDAR los datos del perfil a React
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    // Este sirve para RECIBIR los datos cuando el cliente le dé "Guardar"
    public class UserProfileUpdateDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string? Password { get; set; } // El signo ? significa que puede ser nulo o vacío
    }
}