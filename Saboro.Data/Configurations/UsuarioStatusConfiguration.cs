using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saboro.Core.Models;

namespace Saboro.Data.Configurations;

public class UsuarioStatusConfiguration
{
    public void Configure(EntityTypeBuilder<UsuarioStatus> builder)
    {
        builder.ToTable(nameof(UsuarioStatus));

        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.NomeStatus).IsRequired().HasMaxLength(50);

        builder.HasMany(x => x.Usuarios).WithOne(x => x.UsuarioStatus).HasForeignKey(x => x.IdUsuarioStatus);
    }
}
