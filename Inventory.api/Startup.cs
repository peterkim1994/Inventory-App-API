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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Inventory.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DBContext>();

            services.AddDbContext<DBContext>();

            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 5;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            });

            //DI
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IInventoryRepo, InventoryRepo>();
            services.AddScoped<IPromotionsRepository, PromotionRepository>();
            services.AddScoped<IPromotionsService, PromotionsService>();
            services.AddScoped<ISalesRepository, SalesRepository>();
            services.AddScoped<ISalesService, SalesService>();
            services.AddScoped<IStoreManagementService, StoreManagementService>();
            services.AddAutoMapper(typeof(AutoMapperProfiles));

            services.AddCors(options =>
            {
                options.AddPolicy("InventoryPosPolicy",
                builder =>
                {
                    builder.WithOrigins("*")
                        .AllowAnyHeader()                     
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });
            
            var secretBytes = Encoding.UTF8.GetBytes(Configuration["SecretKey"]);
            var key = new SymmetricSecurityKey(secretBytes);

            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = "Bearer";
                opts.DefaultChallengeScheme = "Bearer";
            }).AddJwtBearer("Bearer", config =>
            {
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters()
                {
                    //   ValidIssuer = Configuration["JwtTokenParam:Issuer"],
                    //     ValidAudience = Configuration["JwtTokenParam:Audience"],
                    ValidAudience = "https://localhost:5001", // for postman
                                                             // ValidIssuer = "https://localhost:5001",

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = key,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };            
                config.ForwardSignIn = null;//"CookieAuth";
                config.ForwardChallenge = null;// "CookieAuth";// "CookieAuth";
            })
            .AddCookie("CookieAuth", config => //cookie schema config
            {
                config.Cookie.Name = "ShopOwner";
                config.LoginPath = "";
                config.Events.OnRedirectToLogin = (context) =>
                {
                   // context.HttpContext.Response.Redirect("http://localhost:3000/login");
                    return Task.CompletedTask;
                };
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
