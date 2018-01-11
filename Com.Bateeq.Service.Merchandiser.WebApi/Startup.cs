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
                .AddTransient<CategoryService>()
                .AddTransient<MaterialService>()
                .AddTransient<UOMService>()
                .AddTransient<SizeService>()
                .AddTransient<OTLService>()
                .AddTransient<BuyerService>()
                .AddTransient<EfficiencyService>();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.ApiName = "com.bateeq.service.merchandiser";
                    options.ApiSecret = "secret";
                    options.Authority = "https://localhost:44350";
                    options.RequireHttpsMetadata = false;
                    options.NameClaimType = JwtClaimTypes.Name;
                    options.RoleClaimType = JwtClaimTypes.Role;
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

            //services.AddMvc();
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
