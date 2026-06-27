using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto_3.Models
{
    [Table("mascotas")]
    public class Mascota
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public decimal Peso { get; set; }

        [Required]
        public decimal Altura { get; set; }

        public string? Descripcion { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int TipoMascotaId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        [ForeignKey(nameof(TipoMascotaId))]
        public TipoMascota TipoMascota { get; set; } = null!;

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}