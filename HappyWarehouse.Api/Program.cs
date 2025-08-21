using AutoMapper;
using HappyWarehouse.Api;
using HappyWarehouse.Infrastructure.Auth;
using HappyWarehouse.Infrastructure.Data;
using HappyWarehouse.Infrastructure.Repositories;
using HappyWarehouse.Infrastructure.Seed;
using HappyWarehouse.Service.IService;
using HappyWarehouse.Service.Mapping;
using HappyWarehouse.Service.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo { Title = "HappyWarehouse.Api", Version = "v1" });

    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste your JWT here. No need to add the word Bearer."
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var logsDir = Path.Combine(builder.Environment.ContentRootPath, "Logs");
Directory.CreateDirectory(logsDir);

builder.Host.UseSerilog((ctx, lc) =>
{
    lc.MinimumLevel.Information()
      .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
      .Enrich.FromLogContext()
      .WriteTo.File(
          path: Path.Combine(logsDir, "app-.log"),
          rollingInterval: RollingInterval.Day,
          retainedFileCountLimit: 14,
          shared: true,
          outputTemplate:
              "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {RequestPath} {Message:lj}{NewLine}{Exception}")
      .WriteTo.File(
          formatter: new CompactJsonFormatter(),
          path: Path.Combine(logsDir, "app-.clef"),
          rollingInterval: RollingInterval.Day,
          retainedFileCountLimit: 14,
          shared: true);
});

builder.Services.AddDbContext<AppDbContext>(options =>
{

    var raw = builder.Configuration.GetConnectionString("Sqlite")
              ?? "Data Source={ContentRoot}/App_Data/happywarehouse.db";
    var withRoot = raw.Replace("{ContentRoot}", builder.Environment.ContentRootPath);


    var csb = new SqliteConnectionStringBuilder(withRoot);
    var dbFile = csb.DataSource!;

    if (!Path.IsPathRooted(dbFile))
        dbFile = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, dbFile));

    var dir = Path.GetDirectoryName(dbFile)!;
    Directory.CreateDirectory(dir);

    csb.DataSource = dbFile;
    options.UseSqlite(csb.ToString());
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IWarehouseItemService, WarehouseItemService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<ILookupsService, LookupsService>();
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

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            NameClaimType = JwtRegisteredClaimNames.Sub,
            RoleClaimType = ClaimTypes.Role
        };
    });

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingConfig>());

builder.Services.AddAuthorization();
var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

var enableSwagger = app.Configuration.GetValue<bool>("Swagger:Enabled");
if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "HappyWarehouse API v1");
        o.RoutePrefix = "swagger";
        o.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseSerilogRequestLogging(o =>
{
    o.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0} ms";
});
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await DatabaseSeeder.SeedAsync(db);
}

app.Run();
