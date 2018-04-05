using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.Lib.Configs
{
    public class RelatedSizeConfig : IEntityTypeConfiguration<RelatedSize>
    {
        public void Configure(EntityTypeBuilder<RelatedSize> builder)
        {
            builder.Property(c => c.Code).HasMaxLength(100);

            builder
                .HasOne(rs => rs.Size)
                .WithMany(s => s.RelatedSizes)
                .HasForeignKey(rs => rs.SizeId)
                .IsRequired();
            builder
                .HasOne(rs => rs.SizeRange)
                .WithMany(s => s.RelatedSizes)
                .HasForeignKey(rs => rs.SizeRangeId)
                .IsRequired();
        }
    }
}