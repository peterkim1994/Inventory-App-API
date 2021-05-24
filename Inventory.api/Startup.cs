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
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(opt =>
            //    {
            //        //whos the audience for this / reasourceId in appSettings
            //        opt.Audience = Configuration["AAD:ResourceId"];
            //        //whos giving web tokens on our behalf
            //        opt.Authority = $"{Configuration["AAD:InstanceId"]}{Configuration["AAD:TentantId"]}";
            //    });
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

            var secretBytes = Encoding.UTF8.GetBytes(Configuration["SecretKey"]);
            var key = new SymmetricSecurityKey(secretBytes);
          
       
            //cookie hander -- implementation of IAuthenticationHandler, which will be injected in app.useAthenicateion()
            services.AddAuthentication("OAuth")//checking if token recieved is valid
                .AddJwtBearer("OAuth", config =>
                {
                    config.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration["JwtTokenParam:Issuer"], 
                        ValidAudience = Configuration["JwtTokenParam:Audience"],
                        IssuerSigningKey = key
                    };
                });//action for config and authentication scheme


                //.AddCookie("CookieAuth", config => //cookie schema config
                //{
                //    config.Cookie.Name = "ShopOwner";                
                //    config.LoginPath = "/login";
                ////    config.ReturnUrlParameter = "http://localhost:5001/inventory";
                //    config.Events.OnRedirectToLogin = (context) =>
                //    {
                //       context.HttpContext.Response.Redirect("http://localhost:3000/login");
                //       return Task.CompletedTask;
                //    };
                    

                //});
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
