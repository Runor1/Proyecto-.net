namespace Proyecto_3.Models
{
    public class TipoMascota
    {
        public int Id { get; set; }

        public string NombreTipo { get; set; } = string.Empty;

        public ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();

        public ICollection<Habitacion> Habitaciones { get; set; } = new List<Habitacion>();
    }
}