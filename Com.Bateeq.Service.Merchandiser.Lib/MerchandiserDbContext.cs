using System;
using System.Collections.Generic;
using System.Text;
using Com.Moonlay.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Com.Bateeq.Service.Merchandiser.Lib.Models;

namespace Com.Bateeq.Service.Merchandiser.Lib
{
    public class MerchandiserDbContext : BaseDbContext
    {
        public MerchandiserDbContext(DbContextOptions<MerchandiserDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<UOM> UOMs { get; set; }
        public DbSet<Size> Sizes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}