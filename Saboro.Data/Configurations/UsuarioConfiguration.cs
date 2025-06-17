using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saboro.Core.Models;

namespace Saboro.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable(nameof(Usuario));

        builder.HasKey(n => n.Id);

        builder.Property(x => x.IdCategoriaFavorita).IsRequired();
        builder.Property(x => x.IdNivelCulinario).IsRequired();
        builder.Property(x => x.NomeCompleto).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(254).HasConversion(x => x.ToLower(), x => x);
        builder.Property(x => x.Senha).IsRequired().HasMaxLength(60);
        builder.Property(x => x.Biografia).IsRequired(false).HasMaxLength(500);
        builder.Property(x => x.UsuarioCadastro).IsRequired();
        builder.Property(x => x.DataCadastro).IsRequired().HasColumnType("DATE").HasDefaultValueSql("CURRENT_DATE");
        builder.Property(x => x.UsuarioUltimaAlteracao).IsRequired(false);
        builder.Property(x => x.DataUltimaAlteracao).IsRequired(false).HasColumnType("DATE").HasDefaultValueSql("CURRENT_DATE");
        builder.Property(x => x.DataDesativacao).IsRequired(false);
        builder.Property(x => x.TentativasInvalidas).HasDefaultValue(0);

        builder.HasOne(x => x.CategoriaFavorita).WithMany(x => x.Usuarios).HasForeignKey(x => x.IdCategoriaFavorita);
        builder.HasOne(x => x.NivelCulinario).WithMany(x => x.Usuarios).HasForeignKey(x => x.IdNivelCulinario);
    }
}