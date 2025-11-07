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
        public DbSet<Terreno> Terrenos { get; set; }
        public DbSet<TipoCultivo> TipoCultivos { get; set; }
        public DbSet<Cultivo> Cultivos { get; set; }
        public DbSet<TipoProceso> TipoProcesos { get; set; }
        public DbSet<ProcesoAgricola> ProcesosAgricolas { get; set; }
        public DbSet<DetallePreparacionTerreno> DetallesPreparacionTerreno { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar todas las configuraciones
            modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
            modelBuilder.ApplyConfiguration(new AgricultorConfiguration());
            modelBuilder.ApplyConfiguration(new TerrenoConfiguration());
            modelBuilder.ApplyConfiguration(new TipoCultivoConfiguration());
            modelBuilder.ApplyConfiguration(new CultivoConfiguration());
            modelBuilder.ApplyConfiguration(new TipoProcesoConfiguration());
            modelBuilder.ApplyConfiguration(new ProcesoAgricolaConfiguration()); 
            modelBuilder.ApplyConfiguration(new DetallePreparacionTerrenoConfiguration());
        }
    }
}