using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_3.Models
{
    [Table("habitaciones")]
    public class Habitacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public int TipoMascotaId { get; set; }

        [ForeignKey(nameof(TipoMascotaId))]
        public TipoMascota TipoMascota { get; set; } = null!;

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}