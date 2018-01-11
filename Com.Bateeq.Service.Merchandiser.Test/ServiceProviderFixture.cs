using Com.Bateeq.Service.Merchandiser.Lib;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Com.Bateeq.Service.Merchandiser.Test.DataUtils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Com.Bateeq.Service.Merchandiser.Test
{
    public class ServiceProviderFixture : IDisposable
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public ServiceProviderFixture()
        {
            /* For unit test by local machine (using localdb) */
            //string connectionString = "Server=(localdb)\\mssqllocaldb;Database=com.bateeq.db.merchandiser.test;Trusted_Connection=True;";

            /* For unit test by Travis CI (using docker container mssql) */
            string connectionString = "Server=localhost,1401;Database=com.bateeq.db.merchandiser.test;User=sa;password=Standar123;MultipleActiveResultSets=true;";

            this.ServiceProvider = new ServiceCollection()

                .AddDbContext<MerchandiserDbContext>((serviceProvider, options) =>
                {
                    options.UseSqlServer(connectionString);
                }, ServiceLifetime.Transient)
                .AddTransient<CategoryService>(provider => new CategoryService(provider))
                .AddTransient<MaterialService>(provider => new MaterialService(provider))
                .AddTransient<UOMService>(provider => new UOMService(provider))
                .AddTransient<SizeService>(provider => new SizeService(provider))
                .AddTransient<OTLService>(provider => new OTLService(provider))
                .AddTransient<BuyerService>(provider => new BuyerService(provider))
                .AddTransient<EfficiencyService>(provider => new EfficiencyService(provider))
                .AddTransient<CategoryServiceDataUtil>()
                .BuildServiceProvider();
            
            MerchandiserDbContext dbContext = ServiceProvider.GetService<MerchandiserDbContext>();
            dbContext.Database.Migrate();
        }

        public void Dispose()
        {
        }
    }

    [CollectionDefinition("ServiceProviderFixture collection")]
    public class ServiceProviderFixtureCollection : ICollectionFixture<ServiceProviderFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
