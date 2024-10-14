using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PrintingSystem.Db;
using PrintingSystem.Db.Implementations;
using PrintingSystem.Db.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<PrintingSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddTransient<IOfficeRepository, OfficeRepository>();
builder.Services.AddTransient<IPrintingDeviceRepository, PrintingDeviceRepository>();
builder.Services.AddTransient<IInstallationRepository, InstallationRepository>();
builder.Services.AddTransient<ISessionRepository, SessionRepository>();

builder.Services.AddMemoryCache();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Printing Device API",
        Version = "v1",
        Description = "ƒокументаци€ по Web API дл€ работы с устройствами печати."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Printing Device API v1");
    c.RoutePrefix = string.Empty;
});

app.UseAuthorization();

app.MapControllers();

app.Run();
