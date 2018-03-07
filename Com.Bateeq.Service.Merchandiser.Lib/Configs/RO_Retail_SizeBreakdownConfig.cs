using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Configs
{
    public class RO_Retail_SizeBreakdownConfig : IEntityTypeConfiguration<RO_Retail_SizeBreakdown>
    {
        public void Configure(EntityTypeBuilder<RO_Retail_SizeBreakdown> builder)
        {
            builder.Property(c => c.Code).HasMaxLength(100);
            builder.Property(c => c.StoreId).HasMaxLength(100);
            builder.Property(c => c.StoreCode).HasMaxLength(100);
            builder.Property(c => c.StoreName).HasMaxLength(500);
        }
    }
}
