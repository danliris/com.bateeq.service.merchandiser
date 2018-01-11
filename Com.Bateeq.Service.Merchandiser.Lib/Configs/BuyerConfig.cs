using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Com.Bateeq.Service.Merchandiser.Lib.Configs
{
    public class BuyerConfig : IEntityTypeConfiguration<Buyer>
    {
        public void Configure(EntityTypeBuilder<Buyer> builder)
        {
            builder.Property(c => c.Code).HasMaxLength(100);
            builder.Property(c => c.Name).HasMaxLength(500);
            builder.Property(c => c.Email).HasMaxLength(500);
            builder.Property(c => c.Address1).HasMaxLength(3000);
            builder.Property(c => c.Address2).HasMaxLength(3000);
        }
    }
}
