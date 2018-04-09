using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Com.Bateeq.Service.Merchandiser.Lib.Configs
{
    public class RO_GarmentConfig : IEntityTypeConfiguration<RO_Garment>
    {
        public void Configure(EntityTypeBuilder<RO_Garment> builder)
        {
            builder.Property(c => c.Code).HasMaxLength(100);
            builder.Property(c => c.Instruction).HasMaxLength(3000);
            builder.Ignore(c => c.ImagesFile);

            builder
                .HasMany(ro => ro.RO_Garment_SizeBreakdowns)
                .WithOne(sb => sb.RO_Garment)
                .HasForeignKey(sb => sb.RO_GarmentId)
                .IsRequired();
            builder
                .HasOne(ro => ro.CostCalculationGarment)
                .WithOne(c => c.RO_Garment)
                .HasForeignKey<CostCalculationGarment>(c => c.RO_GarmentId)
                .IsRequired(false);
        }
    }
}