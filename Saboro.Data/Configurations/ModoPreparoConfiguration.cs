using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saboro.Core.Models;

namespace Saboro.Data.Configurations;

public class ModoPreparoConfiguration : IEntityTypeConfiguration<ModoPreparoReceita>
{
    public void Configure(EntityTypeBuilder<ModoPreparoReceita> builder)
    {
        builder.ToTable(nameof(ModoPreparoReceita));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Property(x => x.IdReceita).IsRequired();
        builder.Property(x => x.Ordem).IsRequired();
        builder.Property(x => x.Descricao).IsRequired().HasMaxLength(200);

        builder.HasOne(x => x.Receita).WithMany(x => x.ModoPreparoReceitas).HasForeignKey(x => x.IdReceita);
    }
}