using System.ComponentModel.DataAnnotations;

namespace Proyecto_3.DTOs.Habitaciones
{
    public class HabitacionCreateDto
    {
        [Required]
        [MaxLength(255)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public int TipoMascotaId { get; set; }
    }
}