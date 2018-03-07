using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Com.Bateeq.Service.Merchandiser.Lib.Configs
{
    public class CostCalculationRetail_MaterialConfig : IEntityTypeConfiguration<CostCalculationRetail_Material>
    {
        public void Configure(EntityTypeBuilder<CostCalculationRetail_Material> builder)
        {
            builder.Property(c => c.Code).HasMaxLength(100);
            builder.Property(c => c.CategoryName).HasMaxLength(500);
            builder.Property(c => c.MaterialName).HasMaxLength(500);
            builder.Property(c => c.Description).HasMaxLength(3000);
            builder.Property(c => c.UOMQuantityName).HasMaxLength(500);
            builder.Property(c => c.UOMPriceName).HasMaxLength(500);
            builder.Property(c => c.Information).HasMaxLength(500);
        }
    }
}
