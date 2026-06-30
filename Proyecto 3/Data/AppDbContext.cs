using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_3.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }
        public DbSet<User> Users{ get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

}
    

