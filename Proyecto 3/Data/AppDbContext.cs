using Microsoft.EntityFrameworkCore;
using Proyecto_3.Models;
using backend.Models;


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
        public DbSet<Mascota> Mascotas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Cita>()
                .HasOne(c => c.User)
                .WithMany(u => u.Citas)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Mascota)
                .WithMany(m => m.Citas)
                .HasForeignKey(c => c.MascotaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Cita>()
                .HasOne(c => c.Habitacion)
                .WithMany(h => h.Citas)
                .HasForeignKey(c => c.HabitacionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    
    modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Nombre = "admin",
                Correo = "admin@gmail.com",
                Password = "1234",
                Role = "ADMIN",
                Activo = true
            },
            new User
            {
                Id = 2,
                Nombre = "user",
                Correo = "user@gmail.com",
                Password = "1234",
                Role = "USER",
                Activo = true
            }
);
    }
}
            
        
    


