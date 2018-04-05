using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Com.Bateeq.Service.Merchandiser.Lib.Configs
{
    public class RO_Garment_SizeBreakdownConfig : IEntityTypeConfiguration<RO_Garment_SizeBreakdown>
    {
        public void Configure(EntityTypeBuilder<RO_Garment_SizeBreakdown> builder)
        {
            builder.Property(c => c.Code).HasMaxLength(100);
            builder.Property(c => c.ColorId).HasMaxLength(500);
            builder.Property(c => c.ColorName).HasMaxLength(500);

            builder
                .HasMany(sb => sb.RO_Garment_SizeBreakdown_Details)
                .WithOne(sbd => sbd.RO_Garment_SizeBreakdown)
                .HasForeignKey(sbd => sbd.RO_Garment_SizeBreakdownId)
                .IsRequired();
        }
    }
}