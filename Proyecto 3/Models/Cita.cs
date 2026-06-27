using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_3.Models
{
    [Table("citas")]
    public class Cita
    {
        [Key]
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

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [ForeignKey(nameof(MascotaId))]
        public Mascota Mascota { get; set; } = null!;

        [ForeignKey(nameof(HabitacionId))]
        public Habitacion Habitacion { get; set; } = null!;
    }
}