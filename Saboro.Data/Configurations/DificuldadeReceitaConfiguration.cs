using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saboro.Core.Models;

namespace Saboro.Data.Configurations;

public class DificuldadeReceitaConfiguration : IEntityTypeConfiguration<DificuldadeReceita>
{
    public void Configure(EntityTypeBuilder<DificuldadeReceita> builder)
    {
        builder.ToTable(nameof(DificuldadeReceita));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).IsRequired().ValueGeneratedNever();
        builder.Property(x => x.Dificuldade).IsRequired().HasMaxLength(50);

        builder.HasMany(x => x.Receitas).WithOne(x => x.DificuldadeReceita).HasForeignKey(x => x.IdDificuldadeReceita);
    }
}
