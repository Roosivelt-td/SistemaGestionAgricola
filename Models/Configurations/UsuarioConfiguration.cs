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
            
            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);
            
            builder.Property(u => u.Rol)
                .IsRequired()
                .HasMaxLength(20);
            
            builder.Property(u => u.Nombre)
                .IsRequired()
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
        }
    }
}