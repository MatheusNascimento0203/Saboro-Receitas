using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saboro.Core.Models;

namespace Saboro.Data.Configurations;

public class IngredienteReceitaConfiguration : IEntityTypeConfiguration<IngredienteReceita>
{
    public void Configure(EntityTypeBuilder<IngredienteReceita> builder)
    {
        builder.ToTable(nameof(IngredienteReceita));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(x => x.IdReceita).IsRequired();
        builder.Property(x => x.DescricaoIngrediente).IsRequired().HasMaxLength(200);

        builder.HasOne(x => x.Receita).WithMany(x => x.Ingredientes).HasForeignKey(x => x.IdReceita);
    }
}
