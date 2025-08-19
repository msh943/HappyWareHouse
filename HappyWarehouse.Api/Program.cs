using HappyWarehouse.Infrastructure.Auth;
using HappyWarehouse.Infrastructure.Data;
using HappyWarehouse.Infrastructure.Repositories;
using HappyWarehouse.Infrastructure.Seed;
using HappyWarehouse.Service.IService;
using HappyWarehouse.Service.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(ctx.Configuration)
      .Enrich.FromLogContext();
});
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IWarehouseItemService, WarehouseItemService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var jwt = builder.Configuration.GetSection("Jwt");
var issuer = jwt["Issuer"] ?? "HappyWarehouse";
var audience = jwt["Audience"] ?? "HappyWarehouseAudience";
var key = jwt["Key"] ?? "CHANGEME_SUPER_SECRET_KEY_123456789";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await DatabaseSeeder.SeedAsync(db);
}

app.Run();
