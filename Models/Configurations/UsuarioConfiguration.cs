using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGestionAgricola.Models.Entities;

namespace SistemaGestionAgricola.Models.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.HasKey(u => u.Id);
            
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(u => u.PasswordHash) // CAMBIADO de Password a PasswordHash
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(u => u.Rol)
                .IsRequired()
                .HasMaxLength(20);
            
            builder.Property(u => u.Nombre)
                .IsRequired()
                .HasMaxLength(100);
            
            builder.Property(u => u.Apellidos)
                .HasMaxLength(100);
            
            builder.Property(u => u.Telefono)
                .HasMaxLength(20);
            
            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime");
            
            builder.Property(u => u.UpdatedAt)
                .IsRequired()
                .HasColumnType("datetime");
            
            builder.HasIndex(u => u.Email)
                .IsUnique();

            // Relación uno-a-uno con Agricultor
            builder.HasOne(u => u.Agricultor)
                .WithOne(a => a.Usuario)
                .HasForeignKey<Agricultor>(a => a.UsuarioId);

            // Seed data para usuario admin
            builder.HasData(
                new Usuario 
                { 
                    Id = 1, 
                    Email = "admin@sistema.com", 
                    PasswordHash = "$2a$11$rL5A2H5Y4X3eB7V8C9dQZOB7nT2C4E6F7G8H9I0J1K2L3M4N5O6P7Q8R9S0T", // "admin123"
                    Rol = "admin",
                    Nombre = "Administrador Principal",
                    Apellidos = "Sistema",
                    Telefono = "123456789",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            );
        }
    }
}