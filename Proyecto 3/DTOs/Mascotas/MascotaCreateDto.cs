using System.ComponentModel.DataAnnotations;

namespace Proyecto_3.DTOs.Mascotas
{
    public class MascotaCreateDto
    {
        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 9999)]
        public decimal Peso { get; set; }

        [Required]
        [Range(0.01, 9999)]
        public decimal Altura { get; set; }

        public string? Descripcion { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int TipoMascotaId { get; set; }
    }
}