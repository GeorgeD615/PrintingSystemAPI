using Microsoft.EntityFrameworkCore;
using PrintingSystem.Db;
using PrintingSystem.Db.Implementations;
using PrintingSystem.Db.Interfaces;

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
