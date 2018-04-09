using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Com.Bateeq.Service.Merchandiser.Lib.Configs
{
    public class EfficiencyConfig : IEntityTypeConfiguration<Efficiency>
    {
        public void Configure(EntityTypeBuilder<Efficiency> builder)
        {
            builder.Property(m => m.Code).HasMaxLength(100);
            builder.Property(m => m.Name).HasMaxLength(500);
        }
    }
}
