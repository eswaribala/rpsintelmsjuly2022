using CatalogAPI.Contexts;
using CatalogAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<CatalogContext>(options =>
options.UseSqlServer(configuration.
GetConnectionString("Catalog_Conn_String")));
// Add services to the container.
//DI singelton,scoped or transient
builder.Services.AddScoped<ICatalogRepo, CatalogRepo>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(1, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
    x.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
});
builder.Services.AddSwaggerGen();

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
