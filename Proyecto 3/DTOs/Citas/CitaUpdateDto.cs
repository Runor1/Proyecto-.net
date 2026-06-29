using System.ComponentModel.DataAnnotations;

namespace Proyecto_3.DTOs.Citas
{
    public class CitaUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaSalida { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int MascotaId { get; set; }

        [Required]
        public int HabitacionId { get; set; }
    }
}