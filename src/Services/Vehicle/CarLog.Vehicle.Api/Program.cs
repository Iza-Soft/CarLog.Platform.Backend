using CarLog.Vehicle.Application;
using CarLog.Vehicle.Infrastructure;
using CarLog.Vehicle.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;
using System.Reflection;

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
});

#endregion

#region Add Problem Details

builder.Services.AddProblemDetails();

#endregion

#region Add Layers

builder.Services.AddApplication();

builder.Services.AddInfrastructure(builder.Configuration);

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

var app = builder.Build();

#region Configure the HTTP request pipeline

app.UseHttpsRedirection();

app.UseCors("AllowAll");

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
