using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Com.Bateeq.Service.Merchandiser.Lib.Configs
{
    public class RO_Garment_SizeBreakdown_DetailConfig : IEntityTypeConfiguration<RO_Garment_SizeBreakdown_Detail>
    {
        public void Configure(EntityTypeBuilder<RO_Garment_SizeBreakdown_Detail> builder)
        {
            builder.Property(c => c.Code).HasMaxLength(100);
            builder.Property(c => c.Information).HasMaxLength(3000);
            builder.Property(c => c.SizeName).HasMaxLength(500);
        }
    }
}