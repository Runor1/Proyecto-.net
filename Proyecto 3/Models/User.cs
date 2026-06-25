namespace Proyecto_3.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public ICollection<Mascota> Mascotas { get; set; } = new List<Mascota>();

        public ICollection<Cita> Citas { get; set; } = new List<Cita>();
    }
}