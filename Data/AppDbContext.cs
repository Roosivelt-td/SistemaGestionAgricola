using Microsoft.EntityFrameworkCore;
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

            // Configuración de Usuario (existente)
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Password).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Rol).IsRequired().HasMaxLength(20);
                entity.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Telefono).HasMaxLength(20);
                entity.Property(u => u.CreatedAt).IsRequired().HasColumnType("datetime");
                entity.Property(u => u.UpdatedAt).IsRequired().HasColumnType("datetime");
                entity.HasIndex(u => u.Email).IsUnique();
            });

            // Configuración de Agricultor (NUEVO)
            modelBuilder.Entity<Agricultor>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.Property(a => a.Dni).IsRequired().HasMaxLength(20);
                entity.Property(a => a.Direccion).HasColumnType("text");
                entity.Property(a => a.Experiencia).HasColumnType("text");
                entity.Property(a => a.CreatedAt).IsRequired().HasColumnType("datetime");
                entity.Property(a => a.UpdatedAt).IsRequired().HasColumnType("datetime");

                // Relación uno-a-uno con Usuario
                entity.HasOne(a => a.Usuario)
                    .WithOne(u => u.Agricultor)
                    .HasForeignKey<Agricultor>(a => a.UsuarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Índice único para DNI
                entity.HasIndex(a => a.Dni).IsUnique();

                // Índice único para UsuarioId (relación 1:1)
                entity.HasIndex(a => a.UsuarioId).IsUnique();
            });
        }
    }
}