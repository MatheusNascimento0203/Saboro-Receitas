using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saboro.Core.Models;

namespace Saboro.Data.Configurations;

public class NivelCulinarioConfiguration : IEntityTypeConfiguration<NivelCulinario>
{
    public void Configure(EntityTypeBuilder<NivelCulinario> builder)
    {
        builder.ToTable(nameof(NivelCulinario));

        builder.HasKey(n => n.Id);

        builder.Property(x => x.NomeNivel).IsRequired().HasMaxLength(50);

        builder.HasMany(x => x.Usuarios).WithOne(x => x.NivelCulinario).HasForeignKey(x => x.IdNivelCulinario);
    }
}
