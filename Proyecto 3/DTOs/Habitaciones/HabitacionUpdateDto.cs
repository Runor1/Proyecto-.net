using System.ComponentModel.DataAnnotations;

namespace Proyecto_3.DTOs.Habitaciones
{
    public class HabitacionUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public int TipoMascotaId { get; set; }
    }
}