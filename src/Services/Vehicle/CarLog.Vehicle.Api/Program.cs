using CarLog.Vehicle.Api.Configuration;
using CarLog.Vehicle.Application;
using CarLog.Vehicle.Infrastructure;
using CarLog.Vehicle.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region Add Serial log

builder.Host.UseSerilog((context, config) => 
{
    config.ReadFrom.Configuration(context.Configuration).Enrich.FromLogContext().WriteTo.Console().WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
});

#endregion

#region Add services to the container.

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();

#endregion

#region Add API Versioning

builder.Services.AddApiVersioning(config => 
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options => 
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

#endregion

#region Add Swagger

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CarLog Vehicle API",
        Version = "v1",
        Description = "API for managing vehicles in CarLog application",
        Contact = new OpenApiContact
        {
            Name = "CarLog Team",
            Email = "support@carlog.com"
        }
    });

    #region Include XML Comments

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    #endregion

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme 
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter only the token (without the 'Bearer ' prefix) — Swagger will add it itself"
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

#endregion

#region Add Problem Details

builder.Services.AddProblemDetails();

#endregion

#region Add Layers

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

#endregion

#region Add CORS

builder.Services.AddCors(options => 
{
    options.AddPolicy("AllowAll", builder => 
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

#endregion

#region Add Health Check

builder.Services.AddHealthChecks().AddDbContextCheck<VehicleDbContext>();

#endregion

builder.Services.AddDbContext<VehicleDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#region JWT Validation

var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>() ?? throw new InvalidOperationException("Jwt settings are missing in the configuration");

builder.Services.AddSingleton(jwtSettings);

builder.Services
       .AddAuthentication(options =>
       {
           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

           options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
       })
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = jwtSettings.Issuer,
               ValidAudience = jwtSettings.Audience,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
               ClockSkew = TimeSpan.FromMinutes(2)
           };
       });

//builder.Services.AddAuthorization(options => 
//{
//    options.AddPolicy("B2CDriver", policy => policy.RequireClaim("tenant_type", "personal"));

//    options.AddPolicy("B2BFeetManager", policy => policy.RequireClaim("tenant_type", "fleet").RequireClaim("role", "manager"));
//});

#endregion

var app = builder.Build();

#region Configure the HTTP request pipeline

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();

    app.MapOpenApi();

    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider.GetRequiredService<VehicleDbContext>();

    dbContext.Database.Migrate();
}

#endregion

app.Run();
