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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Password).IsRequired().HasMaxLength(255);
                entity.Property(u => u.Rol).IsRequired().HasMaxLength(20);
                entity.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Telefono).HasMaxLength(20);
                
                // Remover los DEFAULT VALUES que causan problemas
                entity.Property(u => u.CreatedAt)
                    .IsRequired()
                    .HasColumnType("datetime");
                
                entity.Property(u => u.UpdatedAt)
                    .IsRequired()
                    .HasColumnType("datetime");
                
                entity.HasIndex(u => u.Email).IsUnique();
            });
        }
    }
}