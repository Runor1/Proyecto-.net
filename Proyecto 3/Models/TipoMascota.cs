using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_3.Models
{
    [Table("tipo_mascotas")]
    public class TipoMascota
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string NombreTipo { get; set; } = string.Empty;

        public ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();

        public ICollection<Habitacion> Habitaciones { get; set; } = new List<Habitacion>();
    }
}