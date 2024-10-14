using Microsoft.EntityFrameworkCore;
using NatechAPI.Data;
using NatechAPI.Models.Config;
using NatechAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ExternalApiSettings>(
    builder.Configuration.GetSection("ExternalApi"));

//Configure DBContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Configure the Services
builder.Services.AddScoped<IExternalApiService, ExternalApiService>();
builder.Services.AddScoped<ICatService, CatsService>();

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

app.Run();
