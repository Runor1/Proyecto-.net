namespace Proyecto_3.Models
{
    public class Habitacion
    {
        public int Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public int TipoMascotaId { get; set; }

        public TipoMascota TipoMascota { get; set; } = null!;

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}