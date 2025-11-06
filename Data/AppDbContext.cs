using Microsoft.EntityFrameworkCore;
using SistemaGestionAgricola.Models.Configurations;
using SistemaGestionAgricola.Models.Entities;

namespace SistemaGestionAgricola.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Agricultor> Agricultores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar todas las configuraciones
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new AgricultorConfiguration());
        }
    }
}