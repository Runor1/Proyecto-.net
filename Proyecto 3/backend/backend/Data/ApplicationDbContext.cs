using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Esto mapea tu modelo Cliente a la base de datos
        public DbSet<Cliente> Clientes { get; set; }
    }
}