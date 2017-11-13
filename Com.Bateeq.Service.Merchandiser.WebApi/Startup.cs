using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Com.Bateeq.Service.Merchandiser.Lib;
using Microsoft.EntityFrameworkCore;
using Com.Bateeq.Service.Merchandiser.Lib.Services;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.AccessTokenValidation;
using IdentityModel;

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
            //string connectionString = Configuration.GetConnectionString("DefaultConnection") ?? Configuration["DefaultConnection"];
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=com.bateeq.db.merchandiser;Trusted_Connection=True;";
            services
                .AddDbContext<CoreDbContext>(options => options.UseSqlServer(connectionString))
                .AddTransient<CategoryService>()
                .AddApiVersioning(options =>
                {
                    options.ReportApiVersions = true;
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                });


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
                .AddAuthorization(options =>
                {
                    options.AddPolicy("service.core.read", (policyBuilder) =>
                    {
                        policyBuilder.RequireClaim("scope", "service.core.read");
                    });
                })
                .AddJsonFormatters();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
