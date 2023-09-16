using InventoryPOS.api.Helpers;
using InventoryPOSApp.Core.Repositories;
using InventoryPOSApp.Core.Services.Interfaces;
using InventoryPOSApp.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using InventoryPOS.DataStore;

//bootstrap the application, use top lvl statements instead of public static void Main()
//todo: Create static partial classes for bootstrapping and tidy code up

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddUserSecrets<Program>()
    .Build();

builder.Services.AddControllers();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DBContext>();

builder.Services.AddDbContext<DBContext>();

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredLength = 5;
    opt.Password.RequireDigit = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
});

//DI
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IInventoryRepo, InventoryRepo>();
builder.Services.AddScoped<IPromotionsRepository, PromotionRepository>();
builder.Services.AddScoped<IPromotionsService, PromotionsService>();
builder.Services.AddScoped<ISalesRepository, SalesRepository>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IStoreManagementService, StoreManagementService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddCors(options =>
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

var secretBytes = Encoding.UTF8.GetBytes(config["SecretKey"]);
var key = new SymmetricSecurityKey(secretBytes);

builder.Services.AddAuthentication(opts =>
{
    opts.DefaultAuthenticateScheme = "Bearer";
    opts.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer("Bearer", config =>
{
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidAudience = "https://localhost:5001",
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = key,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5)
    };
    config.ForwardSignIn = null;//"CookieAuth";
    config.ForwardChallenge = null;// "CookieAuth";
})
.AddCookie("CookieAuth", config => //cookie schema config
{
    config.Cookie.Name = "ShopOwner";
    config.LoginPath = "";
    config.Events.OnRedirectToLogin = (context) =>
    {
        return Task.CompletedTask;
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("InventoryPosPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();