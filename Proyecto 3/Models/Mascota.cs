namespace Proyecto_3.Models
{
    public class Mascota
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public decimal Peso { get; set; }

        public decimal Altura { get; set; }

        public string? Descripcion { get; set; }

        public int UserId { get; set; }

        public int TipoMascotaId { get; set; }

        public User User { get; set; } = null!;

        public TipoMascota TipoMascota { get; set; } = null!;

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}