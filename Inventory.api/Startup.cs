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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddIdentity<IdentityUser,IdentityRole>()
                .AddEntityFrameworkStores<DBContext>();
            services.AddDbContext<DBContext>();
            services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 5;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            });

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

            var secretBytes = Encoding.UTF8.GetBytes(Configuration["SecretKey"]);
            var key = new SymmetricSecurityKey(secretBytes);
          
       
            //cookie hander -- implementation of IAuthenticationHandler, which will be injected in app.useAthenicateion()
            services.AddAuthentication("Bearer")//checking if token recieved is valid
                .AddJwtBearer("Bearer", config =>
                {
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["JwtTokenParam:Issuer"], 
                        ValidAudience = Configuration["JwtTokenParam:Audience"],
                        IssuerSigningKey = key
                    };
                    config.ForwardAuthenticate = "CookieAuth";
                    config.ForwardSignIn = "CookieAuth";
                    config.ForwardDefault = "CookieAuth";   

                });

            //action for config and authentication scheme
            services.AddAuthentication("CookieAuth")
                .AddCookie("CookieAuth", config => //cookie schema config
            {
                config.Cookie.Name = "ShopOwner";                       
                config.Events.OnRedirectToLogin = (context) =>                {
                    context.HttpContext.Response.Redirect("http://localhost:3000/login");
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

            //app.UseStatusCodePages( context => {
            //    var request =  context.HttpContext.Request;
            //    var response =  context.HttpContext.Response;

            //    if (response.StatusCode == 401)
            //    {
            //       response.Redirect("/Home/Login?returnUrl=" + request.Path);
            //    }
            //     return  Task.CompletedTask;
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
