using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saboro.Core.Models;

namespace Saboro.Data.Configurations;

public class CategoriaFavoritaConfiguration : IEntityTypeConfiguration<CategoriaFavorita>
{
    public void Configure(EntityTypeBuilder<CategoriaFavorita> builder)
    {
        builder.ToTable(nameof(CategoriaFavorita));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.NomeCategoria).IsRequired().HasMaxLength(50);

        builder.HasMany(x => x.Usuarios).WithOne(x => x.CategoriaFavorita).HasForeignKey(x => x.IdCategoriaFavorita);
    }
}
