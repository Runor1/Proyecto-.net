using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("users")] // Mapea directamente a la tabla original de Laravel
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(150)]
        public string Correo { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "USER"; // Los roles de tu formulario: ADMIN, EMPLOYEE, USER

        public bool Activo { get; set; } = true; // Para el borrado lógico
    }
}