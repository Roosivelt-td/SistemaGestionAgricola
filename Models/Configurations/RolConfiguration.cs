using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGestionAgricola.Models.Entities;

namespace SistemaGestionAgricola.Models.Configurations
{
    public class RolConfiguration : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("Roles");
            
            builder.HasKey(r => r.Id);
            
            builder.Property(r => r.Nombre)
                .IsRequired()
                .HasMaxLength(50);
                
            builder.Property(r => r.Descripcion)
                .HasMaxLength(200);
                
            builder.Property(r => r.CreatedAt)
                .IsRequired();
            
            // Seed data
            builder.HasData(
                new Rol { Id = 1, Nombre = "admin", Descripcion = "Administrador del sistema" },
                new Rol { Id = 2, Nombre = "agricultor", Descripcion = "Usuario agricultor" },
                new Rol { Id = 3, Nombre = "supervisor", Descripcion = "Supervisor de cultivos" }
            );
        }
    }
}