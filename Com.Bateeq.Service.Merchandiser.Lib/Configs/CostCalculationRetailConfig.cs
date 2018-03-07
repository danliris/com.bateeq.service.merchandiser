using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Com.Bateeq.Service.Merchandiser.Lib.Configs
{
    public class CostCalculationRetailConfig : IEntityTypeConfiguration<CostCalculationRetail>
    {
        public void Configure(EntityTypeBuilder<CostCalculationRetail> builder)
        {
            builder.Property(c => c.Code).HasMaxLength(100);
            builder.Property(c => c.RO).HasMaxLength(100);
            builder.Property(c => c.Article).HasMaxLength(500);
            builder.Property(c => c.StyleId).HasMaxLength(100);
            builder.Property(c => c.StyleName).HasMaxLength(500);
            builder.Property(c => c.SeasonId).HasMaxLength(100);
            builder.Ignore(c => c.SeasonCode);
            builder.Property(c => c.SeasonName).HasMaxLength(500);
            builder.Property(c => c.CounterId).HasMaxLength(100);
            builder.Property(c => c.CounterName).HasMaxLength(500);
            builder.Property(c => c.BuyerName).HasMaxLength(500);
            builder.Property(c => c.SizeRangeName).HasMaxLength(500);
            builder.Property(c => c.Description).HasMaxLength(3000);
            builder.Property(c => c.SelectedRounding).HasMaxLength(20);
            builder
                .HasMany(c => c.CostCalculationRetail_Materials)
                .WithOne(cm => cm.CostCalculationRetail)
                .HasForeignKey(cm => cm.CostCalculationRetailId)
                .IsRequired();
            builder
                .HasOne(c => c.RO_Retail)
                .WithOne(ro => ro.CostCalculationRetail)
                .HasForeignKey<RO_Retail>(ro => ro.CostCalculationRetailId)
                .IsRequired();
        }
    }
}
