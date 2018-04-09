using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Com.Bateeq.Service.Merchandiser.Lib.Configs
{
    public class MaterialConfig : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.Property(c => c.Code).HasMaxLength(100);
            builder.Property(c => c.Name).HasMaxLength(500);
            builder.Property(c => c.Description).HasMaxLength(3000);
            builder.Property(c => c.Composition).HasMaxLength(500);
            builder.Property(c => c.Construction).HasMaxLength(500);
            builder.Property(c => c.Width).HasMaxLength(500);
            builder.Property(c => c.Yarn).HasMaxLength(500);

            builder
                .HasOne(m => m.Category)
                .WithMany(c => c.Materials)
                .HasForeignKey(m => m.CategoryId)
                .IsRequired();
        }
    }
}
