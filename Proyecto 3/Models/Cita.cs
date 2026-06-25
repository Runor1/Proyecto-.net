namespace Proyecto_3.Models
{
    public class Cita
    {
        public int Id { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaSalida { get; set; }

        public int UserId { get; set; }

        public int MascotaId { get; set; }

        public int HabitacionId { get; set; }

        public User User { get; set; } = null!;

        public Mascota Mascota { get; set; } = null!;

        public Habitacion Habitacion { get; set; } = null!;
    }
}