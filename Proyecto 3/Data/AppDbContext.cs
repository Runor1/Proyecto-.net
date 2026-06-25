using Microsoft.EntityFrameworkCore;
using Proyecto_3.Models;

namespace Proyecto_3.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<TipoMascota> TipoMascotas { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Habitacion> Habitaciones { get; set; }
        public DbSet<Mascota> mascotas { get; set; }

    }
}
