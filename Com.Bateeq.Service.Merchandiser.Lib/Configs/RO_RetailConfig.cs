using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Configs
{
    public class RO_RetailConfig : IEntityTypeConfiguration<RO_Retail>
    {
        public void Configure(EntityTypeBuilder<RO_Retail> builder)
        {
            builder.Property(c => c.Code).HasMaxLength(100);
            builder.Property(c => c.ColorId).HasMaxLength(500);
            builder.Property(c => c.ColorName).HasMaxLength(500);
            builder.Property(c => c.Instruction).HasMaxLength(3000);
            builder.Ignore(c => c.ImagesFile);

            builder
                .HasMany(ro => ro.RO_Retail_SizeBreakdowns)
                .WithOne(ro_rsb => ro_rsb.RO_Retail)
                .HasForeignKey(ro_rsb => ro_rsb.RO_RetailId)
                .IsRequired();
            builder
                .HasOne(ro => ro.CostCalculationRetail)
                .WithOne(c => c.RO_Retail)
                .HasForeignKey<CostCalculationRetail>(c => c.RO_RetailId)
                .IsRequired(false);
        }
    }
}
