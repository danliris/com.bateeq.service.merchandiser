using Com.Moonlay.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Com.Bateeq.Service.Merchandiser.Lib.Models;
using Com.Bateeq.Service.Merchandiser.Lib.Configs;

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
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Efficiency> Efficiencies { get; set; }
        public DbSet<SizeRange> SizeRanges { get; set; }
        public DbSet<RelatedSize> RelatedSizes { get; set; }
        public DbSet<CostCalculationRetail> CostCalculationRetails { get; set; }
        public DbSet<CostCalculationRetail_Material> CostCalculationRetail_Materials { get; set; }
        public DbSet<CostCalculationGarment> CostCalculationGarments { get; set; }
        public DbSet<CostCalculationGarment_Material> CostCalculationGarment_Materials { get; set; }
        public DbSet<RO_Retail> RO_Retails { get; set; }
        public DbSet<RO_Retail_SizeBreakdown> RO_RetailSizeBreakdowns { get; set; }
        public DbSet<RO_Garment> RO_Garments { get; set; }
        public DbSet<RO_Garment_SizeBreakdown> RO_Garment_SizeBreakdowns { get; set; }
        public DbSet<RO_Garment_SizeBreakdown_Detail> RO_Garment_SizeBreakdown_Details { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.ApplyConfiguration(new CategoryConfig());
            modelBuilder.ApplyConfiguration(new MaterialConfig());
            modelBuilder.ApplyConfiguration(new UOMConfig());
            modelBuilder.ApplyConfiguration(new SizeConfig());
            modelBuilder.ApplyConfiguration(new RateConfig());
            modelBuilder.ApplyConfiguration(new BuyerConfig());
            modelBuilder.ApplyConfiguration(new EfficiencyConfig());
            modelBuilder.ApplyConfiguration(new SizeRangeConfig());
            modelBuilder.ApplyConfiguration(new RelatedSizeConfig());
            modelBuilder.ApplyConfiguration(new CostCalculationRetailConfig());
            modelBuilder.ApplyConfiguration(new CostCalculationRetail_MaterialConfig());
            modelBuilder.ApplyConfiguration(new CostCalculationGarmentConfig());
            modelBuilder.ApplyConfiguration(new CostCalculationGarment_MaterialConfig());
            modelBuilder.ApplyConfiguration(new RO_RetailConfig());
            modelBuilder.ApplyConfiguration(new RO_Retail_SizeBreakdownConfig());
            modelBuilder.ApplyConfiguration(new RO_GarmentConfig());
            modelBuilder.ApplyConfiguration(new RO_Garment_SizeBreakdownConfig());
            modelBuilder.ApplyConfiguration(new RO_Garment_SizeBreakdown_DetailConfig());
        }
    }
}