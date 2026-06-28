using System.ComponentModel.DataAnnotations;

namespace Proyecto_3.DTOs.TipoMascotas
{
    public class TipoMascotaCreateDto
    {
        [Required]
        [MaxLength(255)]
        public string NombreTipo { get; set; } = string.Empty;
    }
}