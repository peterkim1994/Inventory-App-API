using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryPOS.api.Helpers;
using InventoryPOS.DataStore;
using InventoryPOSApp.Core.Repositories;
using InventoryPOSApp.Core.Services;
using InventoryPOSApp.Core.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Inventory.api
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
            services.AddControllers();
            services.AddDbContext<DBContext>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IInventoryRepo, InventoryRepo>();
            services.AddScoped<IPromotionsRepository, PromotionRepository>();
            services.AddScoped<IPromotionsService, PromotionsService>();
            services.AddScoped<ISalesRepository, SalesRepository>();
            services.AddScoped<ISalesService, SalesService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles));
            services.AddCors(options =>
            {
                options.AddPolicy("InventoryPosPolicy",
                builder =>
                {
                    builder.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("InventoryPosPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
