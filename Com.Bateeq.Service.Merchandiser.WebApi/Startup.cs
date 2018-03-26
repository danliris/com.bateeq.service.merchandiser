using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Com.Bateeq.Service.Merchandiser.Lib;
using Microsoft.EntityFrameworkCore;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.AccessTokenValidation;
using IdentityModel;
using Newtonsoft.Json.Serialization;
using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Microsoft.WindowsAzure.Storage.Auth;
using Com.Bateeq.Service.Merchandiser.Lib.Interfaces;
using Com.Bateeq.Service.Merchandiser.Lib.Services.AzureStorage;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Com.Bateeq.Service.Merchandiser.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("DefaultConnection") ?? Configuration["DefaultConnection"];

            services
                .AddDbContext<MerchandiserDbContext>(options => options.UseSqlServer(connectionString))
                .AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });

            services
                .AddTransient<AzureImageService>();

            services
                .AddTransient<CategoryService>()
                .AddTransient<MaterialService>()
                .AddTransient<UOMService>()
                .AddTransient<SizeService>()
                .AddTransient<RateService>()
                .AddTransient<BuyerService>()
                .AddTransient<EfficiencyService>()
                .AddTransient<SizeRangeService>()
                .AddTransient<RelatedSizeService>()
                .AddTransient<CostCalculationRetailService>()
                .AddTransient<CostCalculationRetail_MaterialService>()
                .AddTransient<CostCalculationGarmentService>()
                .AddTransient<CostCalculationGarment_MaterialService>()
                .AddTransient<RO_RetailService>()
                .AddTransient<RO_Retail_SizeBreakdownService>()
                .AddTransient<RO_GarmentService>()
                .AddTransient<RO_Garment_SizeBreakdownService>()
                .AddTransient<RO_Garment_SizeBreakdown_DetailService>();

            var Secret = Configuration.GetValue<string>("Secret") ?? Configuration["Secret"];
            var Key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = false,
                        IssuerSigningKey = Key
                    };
                });

            services
                .AddMvcCore()
                .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver())
                .AddAuthorization(options =>
                {
                    options.AddPolicy("service.core.read", (policyBuilder) =>
                    {
                        policyBuilder.RequireClaim("scope", "service.core.read");
                    });
                })
                .AddJsonFormatters();

            services.AddCors(options => options.AddPolicy("MerchandiserPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<MerchandiserDbContext>();
                context.Database.Migrate();
            }
            app.UseAuthentication();
            app.UseCors("MerchandiserPolicy");
            app.UseMvc();
        }
    }
}
