using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saboro.Core.Models;

namespace Saboro.Data.Configurations;

public class ReceitaConfiguration : IEntityTypeConfiguration<Receita>
{
    public void Configure(EntityTypeBuilder<Receita> builder)
    {
        builder.ToTable(nameof(Receita));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(x => x.IdUsuario).IsRequired();
        builder.Property(x => x.IdDificuldadeReceita).IsRequired();
        builder.Property(x => x.IdCategoriaFavorita).IsRequired();
        builder.Property(x => x.TituloReceita).IsRequired().HasMaxLength(200);
        builder.Property(x => x.DescricaoReceita).IsRequired().HasMaxLength(2000);
        builder.Property(x => x.TempoPreparo).IsRequired();
        builder.Property(x => x.QtdPorcoes).IsRequired();
        builder.Property(x => x.DataCadastro).IsRequired().HasColumnType("DATE").HasDefaultValueSql("CURRENT_DATE");
        builder.Property(x => x.UsuarioUltimaAlteracao).IsRequired(false);
        builder.Property(x => x.DataUltimaAlteracao).IsRequired(false).HasColumnType("DATE").HasDefaultValueSql("CURRENT_DATE");

        builder.HasOne(x => x.CategoriaFavorita).WithMany(x => x.Receitas).HasForeignKey(x => x.IdCategoriaFavorita);
        builder.HasOne(x => x.DificuldadeReceita).WithMany(x => x.Receitas).HasForeignKey(x => x.IdDificuldadeReceita);
        builder.HasOne(x => x.Usuario).WithMany(x => x.Receitas).HasForeignKey(x => x.IdUsuario);

    }
}
